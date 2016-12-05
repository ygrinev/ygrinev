using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.Common.DataContracts;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
   [TestFixture]
    public class VarianceCheckerTest
    {
       [Test]
       public void GetVariances_Returns_Variances_By_PayeeName()
       {
           var sourcecollection = new List<Disbursement>
           {
               new Disbursement()
               {
                   PayeeName = "Ishita Inc.",
                   City = "Toronto",
                   PaymentReferenceNumber = "12345",
                   NameOnCheque = "Ishita M",
                   PayeeType = PayeeType.Mortgagee,
                   Instructions = "tweedledee",
                   StreetAddress1 = "No street"
               },
               new Disbursement()
               {
                   PayeeName = "Trex",
                   AgentFirstName = "T",
                   AgentLastName = "Rex",
                   AssessmentRollNumber = "46556565",
                   NameOnCheque = "T Rex",
                   PayeeType = PayeeType.LineOfCredit,
                   AccountAction = "Paydown Account",
                   PaymentReferenceNumber = "09887"
               }
           };
           var targetcollection = new List<Disbursement>
           {
               new Disbursement()
               {
                   PayeeName = "Ishita Inc.",
                   City = "Mississauga",
                   PaymentReferenceNumber = "23451",
                   NameOnCheque = "Ishita M",
                   PayeeType = PayeeType.Mortgagee,
                   Instructions = "tweedledum",
                   StreetAddress1 = "No street"
               },
               new Disbursement()
               {
                   PayeeName = "Trex",
                   AgentFirstName = "Tyrannosaurus",
                   AgentLastName = "Rex",
                   AssessmentRollNumber = "46556565",
                   NameOnCheque = "T Rex",
                   PayeeType = PayeeType.LineOfCredit,
                   AccountAction = "Reduce Account Limit",
                   PaymentReferenceNumber = "09887"
               }
           };

           var results=VarianceChecker.GetVariances(sourcecollection, targetcollection, "Disbursement");
           Assert.AreEqual(results[2].Values.Count, 3);
           Assert.AreEqual(results[1].Values.Count, 2);
       }
    }
}
