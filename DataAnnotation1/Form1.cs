using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAnnotation1
{
    public partial class NewCompanyPage : Form
    {
        private Company newCompany = new Company();
        public NewCompanyPage()
        {
            InitializeComponent();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            foreach (TabPage p in tabControl1.TabPages)
            {
                IValidateable obj = (IValidateable)Activator.CreateInstance(Type.GetType("DataAnnotation1." + p.Name.Substring(3)));
                obj.Init(p);
                    //VALIDATION!!
                Validator.ParseNValidate(obj, errors);
            }
            MessageBox.Show(errors.Count < 1 ? "SUCCESS!!" : string.Join("", errors.Select(er => er.ErrorMessage).ToArray()));
        }
    }
}
