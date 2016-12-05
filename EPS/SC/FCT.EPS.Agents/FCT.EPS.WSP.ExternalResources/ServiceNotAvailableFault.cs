
namespace schemas.firstcdn.com.LawyerIntegration.ExternalInterface._20071214
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "ServiceNotAvailableFault", Namespace = "https://schemas.firstcdn.com/LawyerIntegration/ExternalInterface/20071214")]
    public partial class ServiceNotAvailableFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string IdField;

        private string MessageField;

        private string DetailsField;

        private string NavigateToUrlField;

        private string NavigateToUrlMessageField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                this.MessageField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 2)]
        public string Details
        {
            get
            {
                return this.DetailsField;
            }
            set
            {
                this.DetailsField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
        public string NavigateToUrl
        {
            get
            {
                return this.NavigateToUrlField;
            }
            set
            {
                this.NavigateToUrlField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
        public string NavigateToUrlMessage
        {
            get
            {
                return this.NavigateToUrlMessageField;
            }
            set
            {
                this.NavigateToUrlMessageField = value;
            }
        }
    }
}
