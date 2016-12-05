SQLImportExport.exe EXPORT Globalization.config

SQLImportExport.exe IMPORT Globalization.config

SQLImportExport.exe EXPORT LanguageLocalizationMap.config

SQLImportExport.exe IMPORT LanguageLocalizationMap.config

SQLImportExport.exe EXPORT EmailTemplateList.config

SQLImportExport.exe IMPORT EmailTemplateList.config

SQLImportExport.exe EXPORT EmailTemplate.config

SQLImportExport.exe IMPORT EmailTemplate.config

                 * */
                Console.WriteLine(" ");
                Console.WriteLine(" INVALID Usage : ");
                Console.WriteLine(" Usage for generating Excel file from DATABASE : SQLImportExport EXPORT   configfile1.config");
                Console.WriteLine(" Usage for generating SQL script from EXCEL FILE : SQLImportExport IMPORT configfile1.config");
                Console.WriteLine(" EXAMPLES :");
                Console.WriteLine(" SQLImportExport.exe IMPORT EmailTemplateList.config");
                Console.WriteLine(" SQLImportExport.exe EXPORT EmailTemplateList.config");
                Console.WriteLine(" ");
            }          