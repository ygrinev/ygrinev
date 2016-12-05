using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Wcf;
using FCT.LLC.BusinessService.Contracts;
using FCT.LLC.Common.DataContracts;

namespace TestClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            // Register the channel factory for the service. Make it
            // SingleInstance since you don't need a new one each time.
            builder
                .Register(c => new ChannelFactory<ILLCBusinessService>(
                    new BasicHttpBinding{SendTimeout = TimeSpan.FromMinutes(10)},
                    new EndpointAddress("http://localhost/FCT.LLC.BusinessService/LLCBusinessService.svc")))
                .SingleInstance();

            // Register the service interface using a lambda that creates
            // a channel from the factory. Include the UseWcfSafeRelease()
            // helper to handle proper disposal.
            builder
                .Register(c => c.Resolve<ChannelFactory<ILLCBusinessService>>().CreateChannel())
                .As<ILLCBusinessService>()
                .UseWcfSafeRelease();

            var container = builder.Build();

            using (var lifetime = container.BeginLifetimeScope())
            {
                //var deal = new FundingDeal
                //{
                   
                //    ActingFor = LawyerActingFor.Vendor,
                //    BusinessModel = "easyfund",
                //   ClosingDate = DateTime.Now,
                //    DealType = DealType.PurchaseSale,
                //    DealStatus = DealStatus.Active,
                //    Lawyer =
                //        new Lawyer()
                //        {
                //            LawyerID = 47756,
                //            DealContacts = new ContactCollection() {new Contact() {ContactID = 47731}}
                //        },
                //    Property =
                //        new Property() {StreetNumber = "1600", Address = "Amphitheatre Way", Pins = new PinCollection()},
                //    OtherLawyer =
                //        new Lawyer()
                //        {
                //            LawyerID = 47757,
                //            DealContacts =
                //                new ContactCollection()
                //                {
                //                    new Contact() {ContactID = 47731},
                //                    new Contact() {ContactID = 47786}
                //                }
                //        },
                //    Vendors = new VendorCollection() {new Vendor() {FirstName = "Larry", LastName = "Page", }}
                //};
                var disbursements = new DisbursementCollection()
                {
                    new Disbursement()
                    {
                       DisbursementID = 222,
                       NameOnCheque = "Ishita Mukh.",
                        Action = CRUDAction.Update,
                        PayeeName = "TD Canada Trust-1-2-3",
                        PaymentMethod = "EFT",
                        Amount = 5000,
                        TrustAccountID = 1234,
                        ChainDealID = 13546,
                        PayeeType = "Mortgage Broker",
                        PayeeComments = "Test"


                    },
                     new Disbursement()
                    {
                        DisbursementID = 224,
                        PayeeName = "RBC Royal bank",
                        PayeeComments = "Test",
                        PayeeType = "Vendor Lawyer",
                        Amount = 2000,
                        ChainDealID = 13545,
                        PaymentMethod = "WIRE TRANSFER",
                        Action = CRUDAction.Update,
                    },

                    new Disbursement()
                    {
                        DisbursementID = 216,
                        PayeeType = "Mortgagee",
                        PayeeName = "BMO",
                        Amount = 4000,
                        ChainDealID = 13431,
                        Action = CRUDAction.Update,

                    },
                    new Disbursement()
                    {
                        DisbursementID = 217,
                        PayeeType = "EasyFund Fee",
                        PayeeName = "FCT",
                        FCTFeeSplit = FeeDistribution.SplitEqually,
                        Action = CRUDAction.Update,
                        Province = "ON",
                        ChainDealID = 13431,
                        Amount = Convert.ToDecimal(56.50),
                        PurchaserFee = new Fee()
                        {
                            FeeID = 71
                        },
                        VendorFee = new Fee()
                        {
                            FeeID = 71
                        }
                    }
                };
                var disbursementSumm = new DisbursementSummary()
                {
                    RequiredDepositAmount = 3000,
                    Version = StringToByteArray("0x000000000043DFB9"),
                    // Version = Convert.FromBase64String("0x000000000043D98B"),
                    DisbursementSummaryID = 51

                };
                var req = new SaveDisbursementsRequest
                {
                    Disbursements = disbursements,
                    DisbursementSummary = disbursementSumm,
                    DealID = 13431,
                    UserContext = new UserContext() { UserType = UserType.Lawyer, UserID = "ishitam" }
                };

                //var req = new DisburseFundsRequest
                //{
                //    DealID = 1
                //};
                var client = lifetime.Resolve<ILLCBusinessService>();
                client.SaveDisbursements(req);
                Console.WriteLine("Service call successfully completed");

                Console.ReadLine();
            }
        }

        public static byte[] StringToByteArray(string rowVersion)
        {
            if (rowVersion.Length != 18)
            {
                throw new Exception();
            }
            byte[] retVal = new byte[8];

            for (int index = 2; index < 18; index += 2)
            {
                retVal[(index / 2) - 1] =
                (byte)int.Parse(
                rowVersion.Substring(index, 2),

                System.Globalization.NumberStyles.HexNumber);
            }

            return retVal;
        }
    }
}

