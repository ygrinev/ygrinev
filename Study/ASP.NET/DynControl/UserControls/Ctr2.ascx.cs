using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace nsDynControl
{
    public partial class Ctr2 : GenericControl
    {
       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PopulateLookups();
        }
        
        public void PopulateLookups()
        {
            //customized code for this control
            dwCategory.Items.Insert(0, new ListItem("- Select -", "-1"));
            dwCategory.Items.Insert(1, new ListItem("Category value1", "1"));
            dwCategory.Items.Insert(2, new ListItem("Category value2", "2"));           
        }

        //using new keyword to overide
        public new void FillControl(DataRow dw)
        {
            //customized code for this control
            dwCategory.SelectedValue =  dw["Category"].ToString();
            txtComment.Text = dw["Comment"].ToString();
        }

        //using new keyword to overide
        public new void SaveControl(string strSPSave)
        {
            try
            {
                if (this.IsRecDelete(chkDel))
                {
                    ((DynCtlMgr)this.Parent.Parent).Cnt--;
                    return;
                }

                using (SqlConnection cn = new SqlConnection())
                {
                    //customized code for this control
                    cn.ConnectionString = strcn;
                    cn.Open();
                    SqlCommand cm = new SqlCommand(strSPSave, cn);
                    cm.Parameters.Add("@Category", SqlDbType.VarChar, 50).Value = dwCategory.SelectedValue;
                    cm.Parameters.Add("@Comment", SqlDbType.VarChar, 50).Value = txtComment.Text;
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.ExecuteNonQuery();
                }

            }
            catch (SqlException ex)
            {
                throw new Exception("Exception", ex);
            }
        }
        
    }
}
