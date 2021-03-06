using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Naizari.Extensions;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
//using Naizari.Testing;
using System.IO;
using Brevitee.CommandLine;
using Brevitee;
using Brevitee.Testing;
using Brevitee.Html;
using System.Web.Mvc;

namespace Brevitee.Html.Tests
{
    public class ParametersBuilderTests: CommandLineTestInterface
    {
        public class TextAreaTestObject
        {
            [TextArea]
            public string Bio { get; set; }
        }

        public class HiddenTestObject
        {
            [Hidden(Default="Monkey")]
            public string Hidden { get; set; }
        }

        public class PasswordTestObject
        {
            [Password]
            public string Password { get; set; }
        }

        public class DropDownObject
        {

            [DropDown("test", Default="two")]
            public string DropDownProp { get; set; }
        }

        public class ReadyOnlyObject
        {
            [ReadOnly]
            public string NoDefault { get; set; }

            [ReadOnly(Default = "Monkey")]
            public string HasDefault { get; set; }
        }

        [UnitTest]
        public static void CreateObjectInputShouldRenderTextArea()
        {
            InputFormBuilder builder = new InputFormBuilder();
            TagBuilder tag = builder.FieldsetFor(typeof(TextAreaTestObject), null).Id("test");
            FileInfo compareToFile = new FileInfo(string.Format(".\\{0}.txt", MethodBase.GetCurrentMethod().Name));
            Compare(tag, compareToFile);
        }

        [UnitTest]
        public static void CreateObjectInputShouldRenderPassword()
        {
            InputFormBuilder builder = new InputFormBuilder();
            TagBuilder tag = builder.FieldsetFor(typeof(PasswordTestObject), null).Id("test");
            FileInfo compareToFile = new FileInfo(string.Format(".\\{0}.txt", MethodBase.GetCurrentMethod().Name));
            Compare(tag, compareToFile);
        }

        [UnitTest]
        public static void CreateObjectInputShouldRenderHidden()
        {
            InputFormBuilder builder = new InputFormBuilder();
            TagBuilder tag = builder.FieldsetFor((Type)typeof(HiddenTestObject), null).Id("test");
            FileInfo compareToFile = new FileInfo(string.Format(".\\{0}.txt", MethodBase.GetCurrentMethod().Name));
            Compare(tag, compareToFile);
        }

        [UnitTest]
        public static void CreateObjectInputShouldRenderReadOnly()
        {
            InputFormBuilder builder = new InputFormBuilder();
            TagBuilder tag = builder.FieldsetFor(typeof(ReadyOnlyObject), null).Id("test");
            FileInfo compareToFile = new FileInfo(string.Format(".\\{0}.txt", MethodBase.GetCurrentMethod().Name));
            Compare(tag, compareToFile);
        }

        [UnitTest]
        public static void CreateObjectInputShouldRenderDropDown()
        {
            Dictionary<string, string> v = new Dictionary<string, string>();
            v.Add("one", "one");
            v.Add("two", "two");
            v.Add("three", "three");
            v.Add("four", "four");
            DropDown.SetOptions("test", v); 

            InputFormBuilder builder = new InputFormBuilder();
            
            TagBuilder tag = builder.FieldsetFor(typeof(DropDownObject), null).Id("test");
            FileInfo compareToFile = new FileInfo(string.Format(".\\{0}.txt", MethodBase.GetCurrentMethod().Name));
            Compare(tag, compareToFile);
        }

        private static void Compare(TagBuilder tag, FileInfo compareToFile)
        {
            string compare = "";
            if (!compareToFile.Exists)
            {
                Out("The comparison file was not found, using result as comparison", ConsoleColor.Yellow);
                using (StreamWriter sw = new StreamWriter(compareToFile.FullName))
                {
                    sw.Write(tag.ToHtml());
                }
            }

            using (StreamReader sr = new StreamReader(compareToFile.FullName))
            {
                compare = sr.ReadToEnd();
            }

            Expect.IsNotNullOrEmpty(compare);
            Expect.AreEqual(compare, tag.ToHtml().ToString());
            Out(compare, ConsoleColor.Cyan);
        }


    }
}
