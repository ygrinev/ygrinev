using CCListParserRBC.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCListParserRBC
{
    public partial class ImportView : Form
    {
        public ImportView()
        {
            InitializeComponent();
            this.textBox1.TextChanged += (o, eventArgs) => { textBox2.Text = textBox1.Text + ".csv"; };
            //this.textBox1.Text = @"\\fscdprroot\oakville\APPS\Shared\EPS\RBC\FromJin\CCLISTA.???" + "," + @"\\fscdprroot\oakville\APPS\Shared\EPS\RBC\FromJin\CCLISTA.???.downloaded%FTPS";
            //this.textBox1.Text = @"C:\DOC\WORK\TASK\EPS-Merger for CREDIT CARD\TEST\CCLISTA.???";
            this.textBox1.Text = @"\\fscdprroot\oakville\APPS\Shared\EPS\RBC\From Yamini\CCLISTA.???";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            new CCListParser(textBox1.Text, textBox2.Text);
            button1.Enabled = true;
        }
    }
}
