﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FCT.LLC.BusinessService.BusinessLogic.FCTURNReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://firstcanadiantitle.com/", ConfigurationName="FCTURNReference.UniqueReferenceNumberSoap")]
    public interface UniqueReferenceNumberSoap {
        
        // CODEGEN: Generating message contract since element name GetUniqueReferenceNumberResult from namespace http://firstcanadiantitle.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://firstcanadiantitle.com/GetUniqueReferenceNumber", ReplyAction="*")]
        FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponse GetUniqueReferenceNumber(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://firstcanadiantitle.com/GetUniqueReferenceNumber", ReplyAction="*")]
        System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponse> GetUniqueReferenceNumberAsync(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest request);
        
        // CODEGEN: Generating message contract since element name GetPolicyNumberResult from namespace http://firstcanadiantitle.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://firstcanadiantitle.com/GetPolicyNumber", ReplyAction="*")]
        FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponse GetPolicyNumber(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://firstcanadiantitle.com/GetPolicyNumber", ReplyAction="*")]
        System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponse> GetPolicyNumberAsync(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest request);
        
        // CODEGEN: Generating message contract since element name strFctUrn from namespace http://firstcanadiantitle.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://firstcanadiantitle.com/GetInvoiceNumber", ReplyAction="*")]
        FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponse GetInvoiceNumber(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://firstcanadiantitle.com/GetInvoiceNumber", ReplyAction="*")]
        System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponse> GetInvoiceNumberAsync(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUniqueReferenceNumberRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUniqueReferenceNumber", Namespace="http://firstcanadiantitle.com/", Order=0)]
        public FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequestBody Body;
        
        public GetUniqueReferenceNumberRequest() {
        }
        
        public GetUniqueReferenceNumberRequest(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://firstcanadiantitle.com/")]
    public partial class GetUniqueReferenceNumberRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public long lngSourceSystemNumber;
        
        public GetUniqueReferenceNumberRequestBody() {
        }
        
        public GetUniqueReferenceNumberRequestBody(long lngSourceSystemNumber) {
            this.lngSourceSystemNumber = lngSourceSystemNumber;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUniqueReferenceNumberResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUniqueReferenceNumberResponse", Namespace="http://firstcanadiantitle.com/", Order=0)]
        public FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponseBody Body;
        
        public GetUniqueReferenceNumberResponse() {
        }
        
        public GetUniqueReferenceNumberResponse(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://firstcanadiantitle.com/")]
    public partial class GetUniqueReferenceNumberResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetUniqueReferenceNumberResult;
        
        public GetUniqueReferenceNumberResponseBody() {
        }
        
        public GetUniqueReferenceNumberResponseBody(string GetUniqueReferenceNumberResult) {
            this.GetUniqueReferenceNumberResult = GetUniqueReferenceNumberResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPolicyNumberRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPolicyNumber", Namespace="http://firstcanadiantitle.com/", Order=0)]
        public FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequestBody Body;
        
        public GetPolicyNumberRequest() {
        }
        
        public GetPolicyNumberRequest(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://firstcanadiantitle.com/")]
    public partial class GetPolicyNumberRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public long lngSourceSystemNumber;
        
        public GetPolicyNumberRequestBody() {
        }
        
        public GetPolicyNumberRequestBody(long lngSourceSystemNumber) {
            this.lngSourceSystemNumber = lngSourceSystemNumber;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPolicyNumberResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPolicyNumberResponse", Namespace="http://firstcanadiantitle.com/", Order=0)]
        public FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponseBody Body;
        
        public GetPolicyNumberResponse() {
        }
        
        public GetPolicyNumberResponse(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://firstcanadiantitle.com/")]
    public partial class GetPolicyNumberResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetPolicyNumberResult;
        
        public GetPolicyNumberResponseBody() {
        }
        
        public GetPolicyNumberResponseBody(string GetPolicyNumberResult) {
            this.GetPolicyNumberResult = GetPolicyNumberResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetInvoiceNumberRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetInvoiceNumber", Namespace="http://firstcanadiantitle.com/", Order=0)]
        public FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequestBody Body;
        
        public GetInvoiceNumberRequest() {
        }
        
        public GetInvoiceNumberRequest(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://firstcanadiantitle.com/")]
    public partial class GetInvoiceNumberRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public long lngSourceSystemNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string strFctUrn;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string fctBillingID;
        
        public GetInvoiceNumberRequestBody() {
        }
        
        public GetInvoiceNumberRequestBody(long lngSourceSystemNumber, string strFctUrn, string fctBillingID) {
            this.lngSourceSystemNumber = lngSourceSystemNumber;
            this.strFctUrn = strFctUrn;
            this.fctBillingID = fctBillingID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetInvoiceNumberResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetInvoiceNumberResponse", Namespace="http://firstcanadiantitle.com/", Order=0)]
        public FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponseBody Body;
        
        public GetInvoiceNumberResponse() {
        }
        
        public GetInvoiceNumberResponse(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://firstcanadiantitle.com/")]
    public partial class GetInvoiceNumberResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetInvoiceNumberResult;
        
        public GetInvoiceNumberResponseBody() {
        }
        
        public GetInvoiceNumberResponseBody(string GetInvoiceNumberResult) {
            this.GetInvoiceNumberResult = GetInvoiceNumberResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface UniqueReferenceNumberSoapChannel : FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UniqueReferenceNumberSoapClient : System.ServiceModel.ClientBase<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap>, FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap {
        
        public UniqueReferenceNumberSoapClient() {
        }
        
        public UniqueReferenceNumberSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UniqueReferenceNumberSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UniqueReferenceNumberSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UniqueReferenceNumberSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponse FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap.GetUniqueReferenceNumber(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest request) {
            return base.Channel.GetUniqueReferenceNumber(request);
        }
        
        public string GetUniqueReferenceNumber(long lngSourceSystemNumber) {
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest inValue = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest();
            inValue.Body = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequestBody();
            inValue.Body.lngSourceSystemNumber = lngSourceSystemNumber;
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponse retVal = ((FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap)(this)).GetUniqueReferenceNumber(inValue);
            return retVal.Body.GetUniqueReferenceNumberResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponse> FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap.GetUniqueReferenceNumberAsync(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest request) {
            return base.Channel.GetUniqueReferenceNumberAsync(request);
        }
        
        public System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberResponse> GetUniqueReferenceNumberAsync(long lngSourceSystemNumber) {
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest inValue = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequest();
            inValue.Body = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetUniqueReferenceNumberRequestBody();
            inValue.Body.lngSourceSystemNumber = lngSourceSystemNumber;
            return ((FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap)(this)).GetUniqueReferenceNumberAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponse FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap.GetPolicyNumber(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest request) {
            return base.Channel.GetPolicyNumber(request);
        }
        
        public string GetPolicyNumber(long lngSourceSystemNumber) {
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest inValue = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest();
            inValue.Body = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequestBody();
            inValue.Body.lngSourceSystemNumber = lngSourceSystemNumber;
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponse retVal = ((FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap)(this)).GetPolicyNumber(inValue);
            return retVal.Body.GetPolicyNumberResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponse> FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap.GetPolicyNumberAsync(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest request) {
            return base.Channel.GetPolicyNumberAsync(request);
        }
        
        public System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberResponse> GetPolicyNumberAsync(long lngSourceSystemNumber) {
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest inValue = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequest();
            inValue.Body = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetPolicyNumberRequestBody();
            inValue.Body.lngSourceSystemNumber = lngSourceSystemNumber;
            return ((FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap)(this)).GetPolicyNumberAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponse FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap.GetInvoiceNumber(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest request) {
            return base.Channel.GetInvoiceNumber(request);
        }
        
        public string GetInvoiceNumber(long lngSourceSystemNumber, string strFctUrn, string fctBillingID) {
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest inValue = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest();
            inValue.Body = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequestBody();
            inValue.Body.lngSourceSystemNumber = lngSourceSystemNumber;
            inValue.Body.strFctUrn = strFctUrn;
            inValue.Body.fctBillingID = fctBillingID;
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponse retVal = ((FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap)(this)).GetInvoiceNumber(inValue);
            return retVal.Body.GetInvoiceNumberResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponse> FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap.GetInvoiceNumberAsync(FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest request) {
            return base.Channel.GetInvoiceNumberAsync(request);
        }
        
        public System.Threading.Tasks.Task<FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberResponse> GetInvoiceNumberAsync(long lngSourceSystemNumber, string strFctUrn, string fctBillingID) {
            FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest inValue = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequest();
            inValue.Body = new FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.GetInvoiceNumberRequestBody();
            inValue.Body.lngSourceSystemNumber = lngSourceSystemNumber;
            inValue.Body.strFctUrn = strFctUrn;
            inValue.Body.fctBillingID = fctBillingID;
            return ((FCT.LLC.BusinessService.BusinessLogic.FCTURNReference.UniqueReferenceNumberSoap)(this)).GetInvoiceNumberAsync(inValue);
        }
    }
}
