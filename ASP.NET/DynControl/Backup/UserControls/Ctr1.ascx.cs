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
    public partial class Ctr1 : GenericControl
    {        

        //using new keyword to overide
        public new void  FillControl(DataRow dw)
        {
            //customized code for this control
            txtfname.Text = dw["fname"].ToString();
            txtlname.Text = dw["lname"].ToString();       
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
                    cm.Parameters.Add("@fname", SqlDbType.VarChar, 50).Value = txtfname.Text;
                    cm.Parameters.Add("@lname", SqlDbType.VarChar, 50).Value = txtlname.Text;
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.ExecuteNonQuery();
                }

            }
            catch (SqlException ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        //user could overide the existing DeleteControls() here
        //public void DeleteControls(string strSp)
        //{
        //    user's code
        //}

        //user could overide the existing GetControls() here
        //public SqlDataReader GetControls(string strSp)
        //{
        //    user's code
        //}

        
    }
}