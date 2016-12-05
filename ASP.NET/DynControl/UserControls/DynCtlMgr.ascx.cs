////////////////////////// 
//Author: Youqi Ma      //
//Email:youqi@yahoo.com //
////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace nsDynControl
{
    public partial class DynCtlMgr : GenericControl
    {
        private int busModel = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                busModel = int.TryParse((Request.Params[0]??"1").ToString(), out busModel) ? busModel : 1;
                LoadCtls(busModel);
            }
        }

        protected void AddCtl(string controlFileName)
        {
            GenericControl myControl = (GenericControl)LoadControl("~/UserControls/" + controlFileName + ".ascx");
            myControl.ID = "ctl_" + controlFileName;
            Panel pn = new Panel();
            Label lb = new Label();
            lb.CssClass = "title";
            StringBuilder sb = new StringBuilder();
            foreach(char c in controlFileName)
            {
                if (sb.Length > 0 && char.IsUpper(c))
                    sb.Append(" ");
                sb.Append(c);
            }
            lb.Text = sb.ToString();
            pn.Controls.Add(lb);
            pn.CssClass = "box";
            pn.Controls.Add(myControl);
            pnControl.Controls.Add(pn);
        }
        public List<string> GetCtrlList(EnumBusinessModel busModel)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = connString;
                cn.Open();
                SqlCommand cm = new SqlCommand("dbo.spGetCtrlList", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.Add(new SqlParameter("@BusinessModel", busModel));
                DataTable dt = new DataTable();
                dt.Load(cm.ExecuteReader());
                return dt.AsEnumerable().Select(r=>r[0].ToString()).ToList();
            }
        }

        public void LoadCtls(int busModel)
        {
            foreach (string ctrl in GetCtrlList((EnumBusinessModel)busModel))
            {
                AddCtl(ctrl);
            }
        }
        #region Properties
        #endregion

        protected void pnControl_PreRender(object sender, EventArgs e)
        {
            
            foreach (Control c in Page.Controls)
            {

                if(c.GetType().IsSubclassOf(typeof(Table)))
                {
                    ((Panel)c).CssClass = "box";
                }
            }
        }
    }

}