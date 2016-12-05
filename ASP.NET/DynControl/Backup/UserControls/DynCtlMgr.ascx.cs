////////////////////////// 
//Author: Youqi Ma      //
//Email:youqi@yahoo.com //
////////////////////////// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Runtime.Remoting;
using System.Configuration;
using System.Reflection;

namespace nsDynControl
{
    /*ctl_i  control name is zero based, ie. ctl_0, ctl_1..*/
    public partial class DynCtlMgr : System.Web.UI.UserControl
    {
        /*The following section is some names user might need to update if they use different names*/
        string strDomainName = "nsDynControl";
        string strFnDeleteControls = "DeleteControls";
        string strFnGetControls = "GetControls";
        string strFnFillControl = "FillControl";
        string strFnSaveControl = "SaveControl";
        
        
        /*The following section is control parameters set in the aspx page*/
        private string _controlFileName;
        private string _controlClassName;
        private string _controlDeleteSpName;
        private string _controlGetSpName;
        private string _controlSaveSpName;        

        int intDefCnt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopCtls();
            }
            else
            {
                LoadCtls();
            }
        }

        protected void AddCtl(string strNum)
        {
            GenericControl myControl = (GenericControl)LoadControl(_controlFileName);
            myControl.ID = "ctl_" + strNum;            
            pnControl.Controls.Add(myControl);
        }

        protected void DelCtls()
        {
            if (Page.IsValid)
            {
                //to make the code more generic, use reflection to call control method with control class name. 
                ObjectHandle handle = Activator.CreateInstance(null, strDomainName + "." + _controlClassName);
                Object p = handle.Unwrap();
                Type t = p.GetType();
                object[] arr = new[] { _controlDeleteSpName };
                MethodInfo mi = t.GetMethod(strFnDeleteControls);
                mi.Invoke(p, arr);
               
            }
            
        }

        public void SaveCtls()
        {
            int numCtrl = this.Cnt;
            DelCtls();
            for (int i = 0; i < numCtrl; i++)
            {
                Control ctl = (Control)pnControl.FindControl("ctl_" + i);
               
                if (ctl != null)
                    {
                        //to make the code more generic, use reflection to call control method with control class name.   
                        Type t = ctl.GetType();
                        object[] arr = new[] { _controlSaveSpName };
                        MethodInfo mi = t.GetMethod(strFnSaveControl);
                        //note here we use ctl as first parameter in order to access the control elements on the form
                        mi.Invoke(ctl, arr);           
                    }                    
            }
            pnControl.Controls.Clear();
            LoadCtls();
            PopCtls();
        }

        public void LoadCtls()
        {
            intDefCnt = 0;
            this.Cnt = (this.Cnt == 0 ? intDefCnt : this.Cnt);
            for (int intCounter = 0; intCounter < this.Cnt; intCounter++)
            {
                AddCtl(intCounter.ToString());
            }            
        }

        protected void PopCtls()
        {
            try{
            using (SqlConnection cn = new SqlConnection())
            {
                ObjectHandle handle = Activator.CreateInstance(null, strDomainName + "." +_controlClassName);
                Object p = handle.Unwrap();
                Type t = p.GetType();
                MethodInfo mi = t.GetMethod(strFnGetControls);
                object[] arr = new[] { _controlGetSpName };
                //note here we use p as first parameter, this call is using assembly since we still have no control instance at this point
                DataTable dt = (DataTable)mi.Invoke(p, arr);
              
                this.Cnt = (int)dt.Rows.Count;
                if (!IsPostBack) LoadCtls();
                    int i = 0;
                    foreach (DataRow drow in dt.Rows)
                    {
                        Control ctl = pnControl.FindControl("ctl_" + i);
                        if (pnControl.FindControl("ctl_" + i) != null)
                        {
                            //to make the code more generic, use reflection to call control method with control class name. 
                            t = ctl.GetType();
                            mi = t.GetMethod(strFnFillControl);                    
                            arr=new[]{dt.Rows[i]};     
                            //note here we use ctl as first parameter in order to access the control elements on the form
                            mi.Invoke(ctl, arr);
                     
                        }
                        i++;
                    }
            
            }
            }
                    catch (SqlException ex)
                    {
                        throw new Exception("Exception", ex);
                    }
                   
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddCtl(this.Cnt.ToString());
            this.Cnt++;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveCtls();
        }
        #region Properties

        public string ControlFileName
        {
            set
            {
                _controlFileName = value;
            }
        }

        public string ControlClassName
        {
            set
            {
                _controlClassName = value;
            }
        }

        public string ControlDeleteSpName
        {
            set
            {
                _controlDeleteSpName = value;
            }
        }

        public string ControlGetSpName
        {
            set
            {
                _controlGetSpName = value;
            }
        }

        public string ControlSaveSpName
        {
            set
            {
                _controlSaveSpName = value;
            }
        }
        
        
        public int Cnt
        {
            set
            {
                ViewState["cnt"] = value;
            }
            get
            {
                if (ViewState["cnt"] == null)
                    return 1;
                else
                    return (int)(ViewState["cnt"]);
            }
        }

        #endregion
    }     
   
}