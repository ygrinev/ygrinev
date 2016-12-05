using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public class PaymentNotificationMapper:IEntityMapper<tblDealFundsAllocation, PaymentNotification>
    {
        public PaymentNotification MapToData(tblDealFundsAllocation tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new PaymentNotification
                {
                    PaymentReferenceNumber = tEntity.ReferenceNumber,
                    PaymentAmount = tEntity.Amount,
                    PaymentDateTime = tEntity.DepositDate,
                    AdditionalInformation = tEntity.WireDepositDetails,
                    PaymentOriginatorName = tEntity.PaymentOriginatorName,
                    PaymentOriginatorAccount = new Account()
                    {
                        AccountNumber = tEntity.AccountNumber,
                        BankNumber = tEntity.BankNumber,
                        TransitNumber = tEntity.BranchNumber
                    }
                };
                return data;
            }
            return null;
        }

        public tblDealFundsAllocation MapToEntity(PaymentNotification tData)
        {
            if (tData != null)
            {
                var entity = new tblDealFundsAllocation()
                {
                    ReferenceNumber = tData.PaymentReferenceNumber,
                    Amount = tData.PaymentAmount,
                    DepositDate = tData.PaymentDateTime,
                    NotificationTimeStamp = DateTime.Now,
                    WireDepositDetails = tData.AdditionalInformation,
                    PaymentOriginatorName = tData.PaymentOriginatorName
                };
                if (tData.PaymentOriginatorAccount != null)
                {
                    entity.AccountNumber = tData.PaymentOriginatorAccount.AccountNumber;
                    entity.BankNumber = tData.PaymentOriginatorAccount.BankNumber;
                    entity.BranchNumber = tData.PaymentOriginatorAccount.TransitNumber;
                }
                return entity;
            }
            return null;

        }
    }
}
