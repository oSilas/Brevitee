// Model is Table
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Brevitee;
using Brevitee.Data;
using Brevitee.Data.Qi;
using Brevitee.Logging;
using Brevitee.Configuration;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Engines;

namespace Brevitee.Encryption
{
    /// <summary>
    /// An encrypted key value store used to prevent
    /// casual access to sensitive data like passwords.
    /// </summary>
	public partial class Vault
	{
        static Database _defaultVaultDatabase;
        static object _defaultVaultDatabaseSync = new object();
        protected static Database DefaultVaultDatabase
        {
            get
            {
                return _defaultVaultDatabaseSync.DoubleCheckLock(ref _defaultVaultDatabase, () => InitializeRepository());
            }
        }

        public static Database InitializeRepository()
        {
            return InitializeRepository(".\\System.vault.sqlite", Log.Default);
        }

        public static Database InitializeRepository(string filePath, ILogger logger = null)
        {
            Database db = null;

            VaultDatabaseInitializer initializer = new VaultDatabaseInitializer(filePath);
            DatabaseInitializationResult result = initializer.Initialize();
            if (!result.Success)
            {
                logger.AddEntry(result.Exception.Message, result.Exception);
            }

            db = result.Database;

            return db;
        }

        protected static internal string Password
        {
            get
            {
                return "287802b5ca734821";
            }
        }

        static Vault _vault;
        static object _vaultSync = new object();
        public static Vault System
        {
            get
            {
                return _vaultSync.DoubleCheckLock(ref _vault, () =>
                {
                    return Retrieve(DefaultVaultDatabase, "System", Password);
                });

            }
        }

        public static Vault Load(FileInfo file, string name)
        {
            return Load(file, name, "".RandomLetters(16)); // password will only be used if the file doesn't exist
        }

        public static Vault Load(FileInfo file, string name, string password, ILogger logger = null)
        {
            if (logger == null)
            {
                logger = Log.Default;
            }

            Database db = InitializeRepository(file.FullName, logger);
            return Retrieve(db, name, password);
        }

        public static Vault Retrieve(string name)
        {
            return Retrieve(DefaultVaultDatabase, name, string.Empty, false);
        }

        /// <summary>
        /// Get the Vault with the specified name using the
        /// specified password to create it if it doesn't exist
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Vault Retrieve(string name, string password)
        {
            return Retrieve(DefaultVaultDatabase, name, password);
        }

        public static Vault Retrieve(Database database, string name, string password, bool create = true)
        {
            Vault result = Vault.OneWhere(c => c.Name == name, database);
            if (result == null && create)
            {
                result = Create(database, name, password);
            }

            result.Decrypt();
            return result;
        }

        /// <summary>
        /// Create a vault in the specified file by the 
        /// specified name.  If the vault already exists
        /// in the specified file the existing vault
        /// will be returned
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Vault Create(FileInfo file, string name)
        {
            string password = GeneratePassword();
            return Create(file, name, password);
        }

        public static Vault Create(FileInfo file, string name, string password)
        {
            Database db = InitializeRepository(file.FullName, Log.Default);
            return Create(db, name, password);
        }

        public static Vault Create(string name)
        {
            string password = GeneratePassword();
            return Create(name, password);
        }

        private static string GeneratePassword()
        {
            SecureRandom random = new SecureRandom();
            string password = random.GenerateSeed(64).ToBase64().First(16);
            return password;
        }

        public static Vault Create(string name, string password)
        {
            Database db = InitializeRepository();
            return Create(db, name, password);
        }

        public static Vault Create(Database database, string name, string password)
        {
            Vault result = Vault.OneWhere(c => c.Name == name, database);
            if (result == null)
            {
                result = new Vault();
                result.Name = name;
                result.Save(database);
                VaultKey key = result.VaultKeysByVaultId.JustOne(database, false);
                AsymmetricCipherKeyPair keys = RsaKeyGen.GenerateKeyPair(RsaKeyLength._1024);
                key.RsaKey = keys.ToPem();
                key.Password = password.EncryptWithPublicKey(keys);
                key.Save(database);
            }

            return result;
        }

        public string ConnectionString
        {
            get
            {
                return Database.ConnectionString;
            }
        }

        Dictionary<string, DecryptedVaultItem> _items;
        object _itemsLock = new object();
        protected Dictionary<string, DecryptedVaultItem> Items
        {
            get
            {
                return _itemsLock.DoubleCheckLock(ref _items, () => new Dictionary<string, DecryptedVaultItem>());
            }
        }

        private void Decrypt()
        {
            _items = null; // will cause it to reinitiailize above
            string password = Key.PrivateKeyDecrypt(Key.Password);
            VaultItemsByVaultId.Each(item =>
            {
                string key = item.Key.AesPasswordDecrypt(password);
                DecryptedVaultItem value = new DecryptedVaultItem(item, Key);
                Items.Add(key, value);//.Value.AesDecrypt(password));
            });
        }
        
        protected VaultKey Key
        {
            get
            {
                return VaultKeysByVaultId.JustOne(Database);
            }
        }

        public string[] Keys
        {
            get
            {
                Decrypt();
                string[] keys = new string[Items.Keys.Count];
                Items.Keys.CopyTo(keys, 0);
                return keys;
            }
        }

        public bool HasKey(string key)
        {
            string ignore;
            return HasKey(key, out ignore);
        }

        public bool HasKey(string key, out string value)
        {
            value = Get(key);
            return !string.IsNullOrEmpty(value);
        }

        public void Set(string key, string value)
        {
            this[key] = value;
        }

        public string Get(string key)
        {
            return this[key];
        }

        object writeLock = new object();
        public string this[string key]
        {
            get
            {
                if (Items.ContainsKey(key))
                {
                    return Items[key].Value;
                }
                else
                {
                    Decrypt();
                    if (Items.ContainsKey(key))
                    {
                        return Items[key].Value;
                    }
                }

                return null;
            }
            set
            {
                lock (writeLock)
                {
                    Decrypt();
                    if (Items.ContainsKey(key))
                    {
                        Items[key].Value = value;
                    }
                    else
                    {
                        VaultItem item = VaultItemsByVaultId.AddNew();
                        string password = Key.PrivateKeyDecrypt(Key.Password);
                        item.Key = key.AesPasswordEncrypt(password);
                        item.Value = value.AesPasswordEncrypt(password);
                        item.Save();
                        Items[key] = new DecryptedVaultItem(item, Key);
                    }
                }
            }
        }

        public Vault Copy(FileInfo file)
        {
            return Copy(file, Name);
        }

        public Vault Copy(FileInfo file, string name)
        {
            Vault copy = Vault.Load(file, name);
            Keys.Each(key =>
            {
                copy[key] = this[key];
            });

            return copy;
        }
	}
}																								
