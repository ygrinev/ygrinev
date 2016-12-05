using System;
using System.ServiceModel;
using FCT.EPS.Notification;
using FCT.EPS.WSP.Resources;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.Notification
{
     public class PaymentTrackingBasicHttpWebServiceClient : WebServiceChannelFactory<IPaymentTrackingService, BasicHttpBinding> 
    {
         public PaymentTrackingBasicHttpWebServiceClient(string endpointConfigurationName)
             : base(endpointConfigurationName)
     {

     }
        public PaymentNotificationResponse SubmitRequestToPaymentTrackerWebService(PaymentNotificationRequest paymentTrackerRequest,
                                                                                                string servceEndPoint)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            PaymentNotificationResponse res = null;
                IPaymentTrackingService channel =  GetCachedChannel(servceEndPoint);

                if (channel != null)
                {
                    try
                    {
                        res = channel.NotifyPaymentConfirmation(paymentTrackerRequest);
                    }
                    catch (Exception ex)
                    {
                        if (channel != null)
                        {
                            throw new Exception("Error Calling Payment Tracking Web Service # channel state :  " + ((ICommunicationObject)channel).State.ToString() + " : " + servceEndPoint, ex);
                        }
                        throw new Exception("Error Calling Payment Tracking Web Service : " + servceEndPoint, ex);
                    }
                }
                SolutionTraceClass.WriteLineVerbose("End");
            return res;
        }       
    }
}
