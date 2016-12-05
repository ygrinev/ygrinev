/*
 * Author       :   Dhaval Upadhyaya
 * Date         :   20-July-2008
 * About Author :   http://dhavalupadhyaya.wordpress.com/about/
 *  
*/

using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;

[assembly: TagPrefix("myControlLib", "myUcl")]
namespace myControlLib
{
    public class myImageDesigner : ControlDesigner
    {
        myImage objmyImage;
        public override string GetDesignTimeHtml()
        {
            if (objmyImage.ImageUrl.Trim().Length == 0)
            {
                return "<div style=\"background:yellow;width:100px;\"><center>Please set Image URL Property!</center><div>";
            }
            else
            {
                return base.GetDesignTimeHtml();
            }
        }
        public override void Initialize(IComponent component)
        {
            objmyImage = (myImage)component;
            base.Initialize(component);
            return;
        }
    }

    [Designer("myControlLib.myImageDesigner,myControlLib")]
    [ToolboxBitmap(typeof(myImage), "myImageLogo.bmp")]
    [ToolboxData(@"<{0}:myImage runat=""server"" ImageUrl="" "" ImageName="" "" ImageDescription="" "" />")]
    public class myImage : WebControl
    {
        public myImage() { }
        private string _ImageUrl = "";

        [Category("My Custom Properties")]
        [Description("Specify path of the image to be displayed.")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DefaultValue("")]
        [UrlProperty]
        [Bindable(true)]
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }

        private string _ImageName = "";
        [Category("My Custom Properties")]
        [Description("Specify name of the image to be displayed.")]
        public string ImageName
        {
            get { return _ImageName; }
            set { _ImageName = value; }
        }

        private string _ImageDescription = "";
        [Category("My Custom Properties")]
        [Description("Specify description of the image to be displayed.")]
        public string ImageDescription
        {
            get { return _ImageDescription; }
            set { _ImageDescription = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute("onmousemove", "javascript:document.getElementById('" + this.UniqueID + "_divDescription').style.display='';");
            writer.AddAttribute("onmouseout", "javascript:document.getElementById('" + this.UniqueID + "_divDescription').style.display='none';");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, _ImageUrl);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("Name : " + _ImageName);
            writer.RenderEndTag();
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "_divDescription");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("Description : " + _ImageDescription);
            writer.RenderEndTag();
            base.RenderContents(writer);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }
    }
}
