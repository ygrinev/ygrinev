//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using FCT.LLC.Common.DataContracts;
using WcfSerialization = global::System.Runtime.Serialization;


namespace FCT.LLC.BusinessService.Contracts.FaultContracts
{
	/// <summary>
	/// Data Contract Class - ServiceNotAvailableFault
	/// </summary>	
	[WcfSerialization::DataContract(Namespace = "https://schemas.firstcdn.com/LawyerIntegration/ExternalInterface/20071214", Name = "ServiceNotAvailableFault")]
	public partial class ServiceNotAvailableFault 
	{
		private string id;
		private string message;
		private string details;
		private string navigateToUrl;
		private ErrorCode errorCode;
		
		[WcfSerialization::DataMember(Name = "Id", IsRequired = true, Order = 0)]
		public string Id
		{
		  get { return id; }
		  set { id = value; }
		}				
		
		[WcfSerialization::DataMember(Name = "Message", IsRequired = true, Order = 1)]
		public string Message
		{
		  get { return message; }
		  set { message = value; }
		}				
		
		[WcfSerialization::DataMember(Name = "Details", IsRequired = false, Order = 2)]
		public string Details
		{
		  get { return details; }
		  set { details = value; }
		}				
					
		
		[WcfSerialization::DataMember(Name = "NavigateToUrl", IsRequired = false, Order = 3)]
		public string NavigateToUrl
		{
		  get { return navigateToUrl; }
		  set { navigateToUrl = value; }
		}

        [WcfSerialization::DataMember(Name = "ErrorCode", IsRequired = false, Order = 4)]
		public ErrorCode ErrorCode
		{
		  get { return errorCode; }
		  set { errorCode = value; }
		}				
	}
}
