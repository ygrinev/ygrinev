using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCListParserRBC
{
    internal class Address
    {
        private string postalCode = "";
        [RegularExpression(@"^[0-9A-Z ]{6,7}$")]
        public string PostalCode { 
            get 
            { 
                return postalCode; 
            } 
            set 
            {
                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName="PostalCode"});
                postalCode = value;
            } 
        }
    }
    static class Program
    {
        static string GetName<T>(T item) where T : class
        {
            return typeof(T).GetProperties()[0].Name;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get Var name
            int someVar = 0;
            string sName = GetName(new { someVar });
            // Validate
            try
            {
                Address a = new Address() { PostalCode = "V1R5L1"};
            }
            catch (Exception x)
            { 
                string err = x.Message;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ImportView());
        }
    }
}
