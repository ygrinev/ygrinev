using System;

using System.Configuration;

namespace SQLImportExport
{
    public class Program
    {
        public static string CONNECTION = string.Empty;
        public static string COMMAND = string.Empty;
        public static string FILENAME = string.Empty;
        public static string SELECTQUERY = string.Empty;
        public static string TABLENAME = string.Empty;
        public static int KEY_1_COLINDEX = 1;
        public static int KEY_2_COLINDEX = 2;
        public static int KEY_3_COLINDEX = 3;
        public static int KEY_4_COLINDEX = 4;

        public static string UPDATEQUERY_FORMAT="UPDATE {0} SET {1} WHERE {2}";

        public static int UPDATE_1_COLINDEX = 2;
        public static int UPDATE_2_COLINDEX = 3;
        public static int UPDATE_3_COLINDEX = 4;
        public static int UPDATE_4_COLINDEX = 5;

        public static string UPDATE_1_COLTYPE = string.Empty;
                

        static void Main(string[] args)
        {
         

            Console.WriteLine("SQLImportExport " + string.Join("", args));
            if (args.Length == 2)
            {
                GetConfigValues(args);

                if (string.Compare(args[0], "IMPORT", true) == 0)
                {
                    new SQLImportExport().ReadWorkBook(FILENAME + ".xlsx");

                    Console.WriteLine(" FILE : "+ FILENAME + ".SQL CREATED SUCCESSFULLY..");

                }
                else if (string.Compare(args[0], "EXPORT", true) == 0)
                {
                    new SQLImportExport().CreateStringWorkbook(FILENAME + ".xlsx");

                    Console.WriteLine(" FILE : " + FILENAME + ".xlsx CREATED SUCCESSFULLY..");
                }
            }
            else
            {
                /** 
                 SQLImportExport.exe EXPORT Globalization.config

                SQLImportExport.exe IMPORT Globalization.config

                SQLImportExport.exe EXPORT LanguageLocalizationMap.config

                SQLImportExport.exe IMPORT LanguageLocalizationMap.config

                SQLImportExport.exe EXPORT EmailTemplateList.config

                SQLImportExport.exe IMPORT EmailTemplateList.config

                SQLImportExport.exe EXPORT EmailTemplate.config

                SQLImportExport.exe IMPORT EmailTemplate.config
                **/
                Console.WriteLine(" ");
                Console.WriteLine(" INVALID Usage : ");
                Console.WriteLine(" Usage for generating Excel file from DATABASE : SQLImportExport EXPORT   configfile1.config");
                Console.WriteLine(" Usage for generating SQL script from EXCEL FILE : SQLImportExport IMPORT configfile1.config");
                Console.WriteLine(" EXAMPLES :");
                Console.WriteLine(" SQLImportExport.exe IMPORT EmailTemplateList.config");
                Console.WriteLine(" SQLImportExport.exe EXPORT EmailTemplateList.config");
                Console.WriteLine(" ");
            }          

        }

        private static void GetConfigValues(string[] args)
        {
               setConfigFileAtRuntime(args);

                COMMAND = ConfigurationManager.AppSettings["COMMAND"];
                FILENAME = ConfigurationManager.AppSettings["FILENAME"];
                SELECTQUERY = ConfigurationManager.AppSettings["SELECTQUERY"];
                CONNECTION = ConfigurationManager.AppSettings["CONNECTION"];
                TABLENAME = ConfigurationManager.AppSettings["TABLENAME"];

                KEY_1_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["KEY_1_COLINDEX"]);
                KEY_2_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["KEY_2_COLINDEX"]);
                KEY_3_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["KEY_3_COLINDEX"]);
                KEY_4_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["KEY_4_COLINDEX"]);


                UPDATE_1_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["UPDATE_1_COLINDEX"]);
                UPDATE_2_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["UPDATE_2_COLINDEX"]);
                UPDATE_3_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["UPDATE_3_COLINDEX"]);
                UPDATE_4_COLINDEX = Convert.ToInt32(ConfigurationManager.AppSettings["UPDATE_4_COLINDEX"]);
            
           
        }


        protected static void setConfigFileAtRuntime(string[] args)
        {
            string runtimeconfigfile;

            runtimeconfigfile = args[1];

            // Specify config settings at runtime.
            System.Configuration.Configuration config  = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = runtimeconfigfile;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }     
    }
}
