import { tblPaymentRequest } from './tblPaymentRequest';

/// <code-import> Place custom imports between <code-import> tags

/// </code-import>

export class tblPaymentNotification {

   /// <code> Place custom code between <code> tags
   
   /// </code>

   // Generated code. Do not place code below this line.
   PaymentNotificationID: number;
   PaymentRequestID: number;
   NotificationType: string;
   ReferenceNumber: string;
   PaymentDate: Date;
   PaymentAmount: number;
   PaymentStatus: string;
   BatchID: string;
   BatchDescription: string;
   NotificationTimeStamp: Date;
   tblPaymentRequest: tblPaymentRequest;
}

