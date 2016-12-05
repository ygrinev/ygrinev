﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IPaymentReportDetailsCollector
    {
        BatchPaymentReport GetBatchPaymentReportDetails(Disbursement disbursement);
    }
}
