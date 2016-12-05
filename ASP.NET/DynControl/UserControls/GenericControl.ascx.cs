////////////////////////// 
//Author: Youqi Ma      //
//Email:youqi@yahoo.com //
////////////////////////// 

using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace nsDynControl
{
    public partial class GenericControl : System.Web.UI.UserControl
    {
        //user may add application level variable here
        //public int Id;
        
        public static SqlDataReader dr;
        public static string connString = ConfigurationManager.ConnectionStrings["DynControlDB"].ToString();
        
        public bool IsRecDelete(CheckBox chkDel)
        {

            if (chkDel.Checked)
                return true;
            else
                return false;

        }

        public void DeleteControls(string strSp)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = connString;
                cn.Open();
                SqlCommand cm = new SqlCommand(strSp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.ExecuteNonQuery();
            }
        }

        public object GetControls(string strSp)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = connString;
                cn.Open();
                SqlCommand cm = new SqlCommand(strSp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                dt.Load(cm.ExecuteReader());
                return dt;               
            }
        }

        public void FillControl(DataRow dw)
        {
        }

        public void SaveControl(string strSPSave)
        {
        }
    }
}