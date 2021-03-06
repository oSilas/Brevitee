using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Brevitee;
using System.Diagnostics;
using Brevitee.Logging;

namespace Brevitee.CommandLine
{
    [Serializable]
    public abstract class CommandLineInterface
    {
        static event ExitDelegate Exiting;
        static event ExitDelegate Exited;

        static ParsedArguments arguments;

        static CommandLineInterface()
        {
            IsolateMethodCalls = true;
            ValidArgumentInfo = new List<ArgumentInfo>();
        }

        /// <summary>
        /// Represents arguments after parsing with a call to ParseArgs.  Arguments should be 
        /// passed in on the command line in the format /&lt;name&gt;:&lt;value&gt;.
        /// </summary>
        protected static ParsedArguments Arguments { get { return arguments; } set { arguments = value; } }

        static List<ArgumentInfo> validArgumentInfo = new List<ArgumentInfo>();

        protected static List<ArgumentInfo> ValidArgumentInfo
        {
            get { return validArgumentInfo; }
            set { validArgumentInfo = value; }
        }
        
        protected static List<ConsoleMenu> otherMenus;
        protected static List<ConsoleMenu> OtherMenus
        {
            get
            {
                return otherMenus;
            }
            set
            {
                otherMenus = value;
            }
        }

        /// <summary>
        /// If false a prompt to confirm to the last menu will be presented
        /// after every selection, if true the last menu will be presented
        /// automatically
        /// </summary>
        protected static bool AutoReturn
        {
            get;
            set;
        }

        /// <summary>
        /// Event fired after command line arguments are parsed by a call to ParseArgs.
        /// </summary>
        protected static event ConsoleArgsParsedDelegate ArgsParsed;

        /// <summary>
        /// Event fired after command line arguments are parsed by a call to ParseArgs
        /// and there was an error.
        /// </summary>
        protected static event ConsoleArgsParsedDelegate ArgsParsedError;

        
        /// <summary>
        /// Checks if the owner of the current process has admin rights,
        /// if not the original command line is rebuilt and run with 
        /// the runas verb set on the startinfo.  The current
        /// process will exit.
        /// </summary>
        public static void EnsureAdminRights()
        {
            if (!WeHaveAdminRights())
            {
                Elevate();
            }
        }

        /// <summary>
        /// Determines if the current process is being run by a user with administrative 
        /// rights
        /// </summary>
        /// <returns></returns>
        public static bool WeHaveAdminRights()
        {
            return UserUtil.CurrentWindowsUserHasAdminRights();
        }

        /// <summary>
        /// Runs the current process again, prompting for admin rights
        /// </summary>
        public static void Elevate()
        {
            Process current = Process.GetCurrentProcess();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Verb = "runas";
            startInfo.FileName = current.MainModule.FileName;
            StringBuilder arguments = new StringBuilder();
            Environment.GetCommandLineArgs().Rest(1, arg =>
            {
                arguments.Append(arg);
                arguments.Append(" ");
            });
            startInfo.Arguments = arguments.ToString();
            Process.Start(startInfo);
            Environment.Exit(0);
        }

        public static bool ConfirmFormat(string format, params object[] args)
        {
            return Confirm(string.Format(format, args));
        }

        public static bool ConfirmFormat(string format, ConsoleColor color, params object[] args)
        {
            return Confirm(string.Format(format, args), color);
        }

        public static bool ConfirmFormat(string format, ConsoleColor color, bool allowQuit, params object[] args)
        {
            return Confirm(string.Format(format, args), color, allowQuit);
        }

        /// <summary>
        /// Prompts the user for [y]es or [n]o returning true for [y] and false for [n].
        /// </summary>
        /// <returns>boolean</returns>
        public static bool Confirm()
        {
            return Confirm("Continue? [y][N]");
        }

        /// <summary>
        /// Prompts the user for [y]es or [n]o returning true for [y] and false for [n].
        /// </summary>
        /// <param name="message">Optional message for the user.</param>
        /// <returns></returns>
        public static bool Confirm(string message)
        {
            return Confirm(message, true);
        }

        public static bool Confirm(string message, ConsoleColor color)
        {
            return Confirm(message, color, true);
        }
        public static bool Confirm(string message, bool allowQuit)
        {
            return Confirm(message, ConsoleColor.White, allowQuit);
        }

        /// <summary>
        /// Prompts the user for [y]es or [n]o returning true for [y] and false for [n].
        /// </summary>
        /// <param name="message">Optional message for the user.</param>
        /// <param name="allowQuit">If true provides an additional [q]uit option which if selected
        /// will call Environment.Exit(0).</param>
        /// <returns>boolean</returns>
        public static bool Confirm(string message, ConsoleColor color, bool allowQuit)
        {
            Out(message, color);
            if (allowQuit)
            {
                Console.WriteLine(" [q]");
            }
            else
            {
                Console.WriteLine();
            }

            string answer = Console.ReadLine().Trim().ToLower();
            if (answer.Equals("y"))
            {
                return true;
            }

            if (answer.Equals("n"))
            {
                return false;
            }

            if (allowQuit && answer.Equals("q"))
            {
                Environment.Exit(0);
            }

            return false;
        }

        public static int NumberPrompt(string message, ConsoleColor color = ConsoleColor.Cyan)
        {
            string value = Prompt(message, color);
            int result = -1;
            int.TryParse(value, out result);
            return result;
        }

		public static string[] ArrayPrompt(string message, params string[] quitters)
		{
			return ArrayPrompt(message, (IEnumerable<string>)quitters);
		}

		public static string[] ArrayPrompt(string message, IEnumerable<string> quitters)
		{
			List<string> results = new List<string>();
			string entry = Prompt(message);
			while (!quitters.Contains(entry))
			{
				if (!results.Contains(entry) && !string.IsNullOrEmpty(entry))
				{
					results.Add(entry);
				}
			}

			return results.ToArray();
		}

        public static string Prompt(string message)
        {
            return Prompt(message, ConsoleColor.Cyan);
        }

        /// <summary>
        /// Prompts the user for input.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>string</returns>
        public static string Prompt(string message, ConsoleColor textColor)
        {
            return Prompt(message, textColor, false);
        }

        public static string Prompt(string message, ConsoleColor textColor, bool allowQuit)
        {
            return Prompt(message, " >> ", textColor, allowQuit);
        }

        public static string Prompt(string message, string promptTxt, ConsoleColor textColor)
        {
            return Prompt(message, promptTxt, textColor, false);
        }

        public static string Prompt(string message, string promptTxt, ConsoleColor textColor, bool allowQuit)
        {
            return Prompt(message, promptTxt, new ConsoleColorCombo(textColor), allowQuit);
        }

        public static string Prompt(string message, string promptTxt, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            return Prompt(message, promptTxt, new ConsoleColorCombo(textColor, backgroundColor), false);
        }

        public static string Prompt(string message, string promptTxt, ConsoleColor textColor, ConsoleColor backgroundColor, bool allowQuit)
        {
            return Prompt(message, promptTxt, new ConsoleColorCombo(textColor, backgroundColor), allowQuit);
        }

        public static string Prompt(string message, string promptTxt, ConsoleColorCombo colors, bool allowQuit)
        {
            Out(message, colors);
            Console.Write(promptTxt);
            string answer = Console.ReadLine();
            //answer = answer.TruncateFront(message.Length + promptTxt.Length);

            if (allowQuit && answer.ToLowerInvariant().Equals("q"))
            {
                Environment.Exit(0);
            }

            return answer;
        }

        public static void Clear()
        {
            Console.Clear();
        }

        public static void Exit()
        {
            Exit(0);
        }

        public static void Exit(int code)
        {
            ConsoleExtensions.SetTextColor();
            OnExiting(code);
            Environment.Exit(code);
            OnExited(code);
        }

        private static void OnExiting(int code)
        {
            if (Exiting != null)
            {
                Exiting(code);
            }
        }

        private static void OnExited(int code)
        {
            if (Exited != null)
            {
                Exited(code);
            }
        }
        
        public static void Usage(Assembly assembly)
        {
            FileInfo info = new FileInfo(assembly.Location);
            OutLineFormat("{0} [arguments]", info.Name);

            foreach (ArgumentInfo argInfo in ValidArgumentInfo)
            {
                string valueExample = string.IsNullOrEmpty(argInfo.ValueExample) ? string.Empty : string.Format(":{0}\r\n", argInfo.ValueExample);
                OutLineFormat("/{0}{1}\t\t{2}", argInfo.Name, valueExample, argInfo.Description);
            }
        }

        protected static void AddMenu(Assembly assemblyToAnalyze, string name, char option, ConsoleMenuDelegate menuDelegate)
        {
            AddMenu(assemblyToAnalyze, name, option, menuDelegate, name);
        }

        protected static void AddMenu(Assembly assemblyToAnalyze, string name, char option, ConsoleMenuDelegate menuDelegate, string header)
        {
            if (OtherMenus == null)
            {
                OtherMenus = new List<ConsoleMenu>();
            }

            ConsoleMenu menu = new ConsoleMenu();
            menu.HeaderText = header;
            menu.MenuKey = option;
            menu.MenuWriter = menuDelegate;
            menu.Name = name;
            menu.AssemblyToAnalyze = assemblyToAnalyze;
            AddMenu(menu);
        }

        protected static void StartMainMenu()
        {
            AddMenu(Assembly.GetCallingAssembly(), "Main Menu", 'm', new ConsoleMenuDelegate(ShowMenu), "Select an option below:");
            ShowMenu(Assembly.GetCallingAssembly(), OtherMenus.ToArray(), "Select an option below:");
        }

        protected static void AddMenu(ConsoleMenu menu)
        {
            OtherMenus.Add(menu);
        }

        protected static void ShowMenu(Assembly assemblyToAnalyze, ConsoleMenu[] otherMenus, string headerText)
        {
            List<ConsoleInvokeableMethod> actions = GetActions<ConsoleAction>(assemblyToAnalyze);
            ShowMenu(otherMenus, headerText, actions);
        }

        protected static void ShowMenu<TAttribute, TType>(string headerText) where TAttribute : Attribute, new()
        {
            List<ConsoleInvokeableMethod> actions = GetActions<TAttribute, TType>();
            ShowMenu(OtherMenus.ToArray(), headerText, actions);
        }

        protected static void ShowMenu<TAttribute, TType>(ConsoleMenu[] otherMenus, string headerText)
            where TAttribute : Attribute, new()
        {
            List<ConsoleInvokeableMethod> actions = GetActions<TAttribute, TType>();
            ShowMenu(otherMenus, headerText, actions);
        }
        
        private static void ShowMenu(ConsoleMenu[] otherMenus, string headerText, List<ConsoleInvokeableMethod> actions)
        {
            Console.WriteLine(headerText);
            Console.WriteLine();

            ShowActions(actions);

            Console.Write("| Q -> quit ");

            string answer = ShowSelectedMenuOrReturnAnswer(otherMenus);

            Console.WriteLine();

            try
            {
                InvokeSelection(actions, answer);
            }
            catch (Exception ex)
            {
                OutLine("An error occurred: " + ex.Message, ConsoleColor.Red);
                if (Arguments.Contains("stacktrace"))
                {
                    if (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }

                    Out(ex.StackTrace, ConsoleColor.Red);
                }
            }

            if (AutoReturn)
            {
                ShowMenu(otherMenus, headerText, actions);
            }
            else if (ConfirmFormat("Return to {0}? [y][N] ", headerText))
            {
                ShowMenu(otherMenus, headerText, actions);
            }
        }

        protected static string ShowSelectedMenuOrReturnAnswer(ConsoleMenu[] otherMenus)
        {
            WriteOtherMenuOptions(otherMenus);
            string answer = Console.ReadLine();
            Console.WriteLine();
            if (answer.Trim().ToLower().Equals("q"))
                Environment.Exit(0);

            ShowSelectedMenu(otherMenus, answer);
            return answer;
        }

        private static void WriteOtherMenuOptions(ConsoleMenu[] otherMenus)
        {
            if (otherMenus != null)
            {
                foreach (ConsoleMenu menu in otherMenus)
                {
                    Console.Write(" | " + menu.MenuKey + " -> " + menu.Name);
                }
                Console.WriteLine();
            }
        }

        private static void ShowSelectedMenu(ConsoleMenu[] otherMenus, string answer)
        {
            if (otherMenus != null)
            {
                foreach (ConsoleMenu menu in otherMenus)
                {
                    if (menu.MenuKey.ToString().ToLower().Equals(answer.Trim().ToLower()))
                    {
                        menu.MenuWriter(menu.AssemblyToAnalyze, otherMenus, menu.HeaderText);//menu.Menu.Method.Invoke(menu.AssemblyToAnalyze, otherMenus, menu.HeaderText);
                    }
                }
                Console.WriteLine();
            }
        }

        public static void OutLine()
        {
            Out();
        }

        /// <summary>
        /// Writes a newline character to the console using Console.WriteLine()
        /// </summary>
        public static void Out()
        {
            Console.WriteLine();
        }
        public static void OutLineFormat(string message, params object[] formatArgs)
        {
            OutLine(string.Format(message, formatArgs));
        }

        /// <summary>
        /// Print the specified message in the specified
        /// color to the console using the specified string.format
        /// args to format the message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <param name="formatArgs"></param>
        public static void OutLineFormat(string message, ConsoleColor color, params object[] formatArgs)
        {
            OutLine(string.Format(message, formatArgs), color);
        }

		/// <summary>
		/// Print the specified message in the specified
		/// colors to the console using the specified string.format
		/// args to format the message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="foreground"></param>
		/// <param name="background"></param>
		/// <param name="formatArgs"></param>
        public static void OutLineFormat(string message, ConsoleColor foreground, ConsoleColor background, params object[] formatArgs)
        {
            OutLine(string.Format(message, formatArgs), new ConsoleColorCombo(foreground, background));
        }

        public static void OutLineFormat(string message, ConsoleColorCombo colors, params object[] formatArgs)
        {
            OutLine(string.Format(message, formatArgs), colors);
        }
        //
        public static void OutFormat(string message, params object[] formatArgs)
        {
            Out(string.Format(message, formatArgs));
        }

        public static void OutFormat(string message, ConsoleColor color, params object[] formatArgs)
        {
            Out(string.Format(message, formatArgs), color);
        }

        public static void OutFormat(string message, ConsoleColor foreground, ConsoleColor background, params object[] formatArgs)
        {
            Out(string.Format(message, formatArgs), new ConsoleColorCombo(foreground, background));
        }

        public static void OutFormat(string message, ConsoleColorCombo colors, params object[] formatArgs)
        {
            Out(string.Format(message, formatArgs), colors);
        }

        public static void Out(string message)
        {
            Out(message, ConsoleColor.Gray);
        }

        public static void Out(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();         
        }

        public static void Out(string message, ConsoleColorCombo colors)
        {
            Console.BackgroundColor = colors.BackgroundColor;
            Console.ForegroundColor = colors.ForegroundColor;
            Console.Write(message);
            Console.ResetColor();
        }

        public static void OutLine(string message)
        {
            OutLine(message, ConsoleColor.Gray);
        }

        public static void OutLine(string message, ConsoleColor color)
        {
            Out(message, color);
            Out();
        }

        public static void OutLine(string message, ConsoleColor foreground, ConsoleColor background)
        {
            Out(message, new ConsoleColorCombo(foreground, background));
            Out();
        }

        public static void OutLine(string message, ConsoleColorCombo colors)
        {
            Out(message, colors);
            Out();
        }

        public static void InvokeSelection(List<ConsoleInvokeableMethod> actions, string answer)
        {
            InvokeSelection(actions, answer, "", "");
        }

        protected static void InvokeSelection(List<ConsoleInvokeableMethod> actions, string answer, string header, string footer)
        {
            int ignore;
            InvokeSelection(actions, answer, header, footer, out ignore);
        }

        static MethodInfo _methodToInvoke;
        static object invokeOn;
        static object[] parameters;
        private static void InvokeMethod()
        {
            if (_methodToInvoke == null)
            {
                _methodToInvoke = (MethodInfo)AppDomain.CurrentDomain.GetData("Method");
            }

            if (_methodToInvoke != null)
            {
                object inst = invokeOn == null ? AppDomain.CurrentDomain.GetData("Instance") : invokeOn;
                object[] parms = parameters == null ? (object[])AppDomain.CurrentDomain.GetData("Parameters") : parameters;
                _methodToInvoke.Invoke(inst, parms);
            }
        }

        protected internal static void InvokeInCurrentAppDomain(MethodInfo method, object instance, object[] ps = null)
        {
            // added this method for consistency with InvokeInSeparateAppDomain method
            _methodToInvoke = method;
            invokeOn = instance;
            parameters = ps;

            InvokeMethod();
        }
		
        protected internal static void InvokeInSeparateAppDomain(MethodInfo method, object instance, object[] ps = null)
        {
            AppDomain isolationDomain = AppDomain.CreateDomain("TestAppDomain");
            _methodToInvoke = method;
            invokeOn = instance;
            parameters = ps;

            isolationDomain.SetData("Method", method);
            isolationDomain.SetData("Instance", instance);
            isolationDomain.SetData("Parameters", parameters);
            isolationDomain.DoCallBack(InvokeMethod);
        }

        protected internal static void InvokeSelection(List<ConsoleInvokeableMethod> actions, string answer, string header, string footer, out int selectedNumber)
        {
            selectedNumber = -1;
            if (int.TryParse(answer.ToString(), out selectedNumber) && (selectedNumber - 1) > -1 && (selectedNumber - 1) < actions.Count)
            {
                selectedNumber = InvokeSelection(actions, header, footer, selectedNumber);
            }
            else
            {
                Console.WriteLine("Invalid entry");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// If true will cause all calls to InvokeSelection to be 
        /// run in a separate AppDomain.  This is primarily for 
        /// UnitTest isolation.
        /// </summary>
        protected internal static bool IsolateMethodCalls
        {
            get;
            set;
        }

        protected internal static int InvokeSelection(List<ConsoleInvokeableMethod> actions, string header, string footer, int selectedNumber)
        {
            selectedNumber -= 1;
            ConsoleInvokeableMethod action = actions[selectedNumber];
            MethodInfo method = action.Method;
            MethodInfo invoke = typeof(ConsoleInvokeableMethod).GetMethod("Invoke");
            object[] parameters = GetParameterInput(method);
            action.Parameters = parameters;

            if (!string.IsNullOrEmpty(header))
            {
                Out(header, ConsoleColor.White);
            }

            try
            {
                if (!method.IsStatic)
                {
                    ConstructorInfo ctor = method.DeclaringType.GetConstructor(Type.EmptyTypes);
                    if (ctor == null)
                        ExceptionHelper.Throw<InvalidOperationException>("Specified non-static method is declared on a type that has no parameterless constructor. {0}.{1}", method.DeclaringType.Name, method.Name);

                    action.Provider = ctor.Invoke(null);
                }

                if (IsolateMethodCalls)
                {
                    InvokeInSeparateAppDomain(invoke, action, parameters);
                }
                else
                {
                    InvokeInCurrentAppDomain(invoke, action, parameters);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                else
                    throw;
            }
            if (!string.IsNullOrEmpty(footer))
                Out(footer, ConsoleColor.White);
            return selectedNumber;
        }

        protected static void ShowActions(List<ConsoleInvokeableMethod> actions)
        {
            for (int i = 1; i <= actions.Count; i++)
            {
                ConsoleInvokeableMethod consoleMethod = actions[i - 1];
                string menuOption = consoleMethod.Information;
                Console.WriteLine("{0}. {1}", i, menuOption);
            }           
        }

        protected static List<ConsoleInvokeableMethod> GetActions(Assembly assemblyToAnalyze)
        {
            return GetActions<ConsoleAction>(assemblyToAnalyze);
        }

        protected static List<ConsoleInvokeableMethod> GetActions<TAttribute, TType>() where TAttribute : Attribute, new()
        {
            return GetActions(typeof(TAttribute), typeof(TType));
        }

        protected static List<ConsoleInvokeableMethod> GetActions<TAttribute>(Type typeWhoseAssemblyWillBeAnalyzed) where TAttribute : Attribute, new()
        {
            return GetActions<TAttribute>(typeWhoseAssemblyWillBeAnalyzed.Assembly);
        }

        protected static List<ConsoleInvokeableMethod> GetActions<TAttribute>(Assembly assemblyToAnalyze) where TAttribute : Attribute, new()
        {
            return GetActions(assemblyToAnalyze, typeof(TAttribute));
        }

        protected static List<ConsoleInvokeableMethod> GetActions(Assembly assemblyToAnalyze, Type attrType)
        {
            List<ConsoleInvokeableMethod> actions = new List<ConsoleInvokeableMethod>();
            Type[] types = assemblyToAnalyze.GetTypes();
            foreach (Type type in types)
            {
                actions.AddRange(GetActions(attrType, type));
            }
            return actions;
        }

        protected static List<ConsoleInvokeableMethod> GetActions(Type attrType, Type type)
        {
            List<ConsoleInvokeableMethod> actions = new List<ConsoleInvokeableMethod>();
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                object action = null;
                if (method.HasCustomAttributeOfType(attrType, out action)) //HasCustomAttributeOfType(method, out action))
                {
                    actions.Add(new ConsoleInvokeableMethod(method, (Attribute)action));
                }
            }

            return actions;
        }

        protected static char Pause()
        {
            return Pause(string.Empty);
        }

        protected static char Pause(string message)
        {
            if (!string.IsNullOrEmpty(message))
                Console.WriteLine(message);

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Q)
                Exit(0);

            return keyInfo.KeyChar;
        }

        protected static object[] GetParameterInput(MethodInfo method)
        {
            return GetParameterInput(method, false);
        }

        protected static object[] GetParameterInput(MethodInfo method, bool generate)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            List<object> parameterValues = new List<object>(parameterInfos.Length);
            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                if (parameterInfo.ParameterType != typeof(string))
                {
                    OutLine(string.Format("The method {0} can't be invoked because it takes parameters that are not of type string.", method.Name)
                        , ConsoleColor.Red);                    
                }

                if (generate)
                    parameterValues.Add("".RandomString(5));
                else
                    parameterValues.Add(GetInput(parameterInfo));
            }
            return parameterValues.ToArray();
        }

        private static string GetInput(ParameterInfo parameter)
        {
            Console.WriteLine("{0}: ", parameter.Name);
            return Console.ReadLine();
        }

        protected static bool HasCustomAttributeOfType<T>(MethodInfo method, out T attribute) where T: Attribute, new()
        {
            return CustomAttributeExtension.HasCustomAttributeOfType<T>(method, out attribute);
        }

        /// <summary>
        /// Makes the specified name a valid command line argument.  Command line
        /// arguments are assumed to be in the format /&lt;name&gt;:&lt;value&gt;.
        /// </summary>
        /// <param name="name"></param>
        public static void AddValidArgument(string name, string description = null)
        {
            AddValidArgument(name, false, description);
        }

        /// <summary>
        /// Calls AddValidArgument for every ConsoleAction that has a 
        /// CommandLineSwitch defined
        /// </summary>
        /// <param name="type"></param>
        public static void AddSwitches(Type type)
        {
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                ConsoleAction action = null;
                if (method.HasCustomAttributeOfType<ConsoleAction>(out action))
                {
                    if (!string.IsNullOrEmpty(action.CommandLineSwitch))
                    {
                        AddValidArgument(action.CommandLineSwitch, true, action.Information, action.ValueExample);
                    }
                }
            }
        }

		/// <summary>
		/// Execute the methods on the specified instance that are addorned with ConsoleAction
		/// attributes that have CommandLineSwitch(es) defined that match keys in the
		/// specified ParsedArguments using the specified ILogger to report any switches not
		/// found.  An ExpectFailedException will be thrown if more than one method is found
		/// with a matching CommandLineSwitch defined in ConsoleAction attributes
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="instance"></param>
		/// <param name="logger"></param>
		public static void ExecuteSwitches(ParsedArguments arguments, object instance, ILogger logger = null)
		{
			Expect.IsNotNull(instance, "instance can't be null, use a Type if executing static method");
			ExecuteSwitches(arguments, instance.GetType(), instance, logger);
		}

		/// <summary>
		/// Execute the methods on the specified instance that are addorned with ConsoleAction
		/// attributes that have CommandLineSwitch(es) defined that match keys in the
		/// specified ParsedArguments using the specified ILogger to report any switches not
		/// found.  An ExpectFailedException will be thrown if more than one method is found
		/// with a matching CommandLineSwitch defined in ConsoleAction attributes
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="instance"></param>
		/// <param name="logger"></param>
		public static void ExecuteSwitches(ParsedArguments arguments, Type type, object instance = null, ILogger logger = null)
		{
			foreach (string key in arguments.Keys)
			{
				string commandLineSwitch = key;
				string switchValue = arguments[key];
				MethodInfo[] methods = type.GetMethods();
				List<ConsoleInvokeableMethod> toExecute = new List<ConsoleInvokeableMethod>();
				foreach (MethodInfo method in methods)
				{
					ConsoleAction consoleAction;
					if (method.HasCustomAttributeOfType<ConsoleAction>(out consoleAction))
					{
						if (consoleAction.CommandLineSwitch.Or("").Equals(commandLineSwitch))
						{
							toExecute.Add(new ConsoleInvokeableMethod(method, consoleAction, instance, switchValue));
						}
					}
				}

				Expect.IsFalse(toExecute.Count > 1, "Multiple ConsoleActions found with the specified command line switch: {0}"._Format(commandLineSwitch));

				if (toExecute.Count == 0)
				{
					logger = logger ?? Log.Default;
					logger.AddEntry("Specified command line switch was not found: {0}"._Format(commandLineSwitch));
					continue;
				}

				ConsoleInvokeableMethod methodToInvoke = toExecute[0];
				if (IsolateMethodCalls)
				{
					methodToInvoke.InvokeInSeparateAppDomain();
				}
				else
				{
					methodToInvoke.InvokeInCurrentAppDomain();
				}
			}
		}

        /// <summary>
        /// Makes the specified name a valid command line argument.  Command line
        /// arguments are assumed to be in the format /&lt;name&gt;:&lt;value&gt;.
        /// </summary>
        /// <param name="name">The name of the command line argument.</param>
        /// <param name="allowNull">If true no value for the specified name is necessary.</param>
        public static void AddValidArgument(string name, bool allowNull, string description = null, string valueExample = null)
        {
            ValidArgumentInfo.Add(new ArgumentInfo(name, allowNull, description, valueExample));
        }

        protected static void ParseArgs(string[] args)
        {
            Arguments = new ParsedArguments(args, ValidArgumentInfo.ToArray());
            if (Arguments.Status == ArgumentParseStatus.Error || Arguments.Status == ArgumentParseStatus.Invalid)
            {
                if (ArgsParsedError != null)
                    ArgsParsedError(Arguments);
            }
            else if (Arguments.Status == ArgumentParseStatus.Success)
            {
                if (ArgsParsed != null)
                    ArgsParsed(Arguments);
            }
        }

    }
}
