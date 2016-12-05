using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CaseStudy2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string textValidated = this.Context.Request.Unvalidated.Form[txtValidate.UniqueID];
        string textUnValidated = this.Context.Request.Form[txtunValidate.UniqueID];
    }

}