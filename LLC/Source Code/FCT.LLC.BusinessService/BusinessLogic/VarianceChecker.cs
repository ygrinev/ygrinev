using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class VarianceChecker
    {

        private const string DisbursementIdentifier = "PayeeName";

        //CAUTION: Uses Reflection. O 2(N^2) runtime. Use sparingly.
        //Only detects updates to existing values.Does not work for Insert/Delete scenarios

        public static IDictionary<int, IDictionary<string, Variance>> GetVariances(
            IEnumerable<Disbursement> sourceCollection,
            IEnumerable<Disbursement> sinkCollection, string classname, IEnumerable<string> toIgnoreProperties = null)
        {
            IDictionary<int, IDictionary<string, Variance>> varianceCollection =
                new Dictionary<int, IDictionary<string, Variance>>();
            IEnumerable<PropertyInfo> properties = toIgnoreProperties != null
                ? typeof (Disbursement).GetProperties().Where(p => !toIgnoreProperties.Contains(p.Name))
                : typeof (Disbursement).GetProperties();

            int sourceCount = sourceCollection.Count();
            int sinkCount = sinkCollection.Count();
            if (sourceCount != sinkCount)
            {
                return null;
            }
            foreach (var item in sourceCollection)
            {
                IDictionary<string, Variance> variances = new Dictionary<string, Variance>();
                string payeeName=string.Empty;
                foreach (var property in properties)
                {
                    if (property.Name == DisbursementIdentifier)
                    {
                        var itemProperty = item.GetType().GetProperty(property.Name);
                        if (itemProperty.GetValue(item) != null)
                        {
                            payeeName = itemProperty.GetValue(item).ToString();
                        }
                        else
                        {
                            if (item.PayeeType == FeeDistribution.VendorLawyer && string.IsNullOrEmpty(payeeName))
                            {
                                payeeName = FeeDistribution.VendorLawyer;
                            }
                        }
                       
                    }                      
                    else
                    {
                        if (MeetsOtherRestrictions(item, property))
                        {
                            var variance = new Variance() { ParentClassName = classname };
                            var itemProperty = item.GetType().GetProperty(property.Name);
                            variance.PropertyName = property.Name;
                            variance.SourceValue = itemProperty.GetValue(item) != null
                                ? itemProperty.GetValue(item).ToString()
                                : string.Empty;

                            variances.Add(string.Format("{0}.{1}", classname, property.Name), variance);
                        }

                    }
                    
                }
                foreach (var variance in variances)
                {
                    variance.Value.Identifier = payeeName;
                }

                varianceCollection.Add(sourceCount, variances);
                sourceCount--;              
            }

            foreach (var item in sinkCollection)
            {
                foreach (var property in properties)
                {
                    if (property.Name != DisbursementIdentifier && MeetsOtherRestrictions(item,property))
                    {
                        var variances = varianceCollection[sinkCount];
                        var itemProperty = item.GetType().GetProperty(property.Name);
                        string key = string.Format("{0}.{1}", classname, property.Name);
                        Variance variance = variances[key];
                        variance.SinkValue = itemProperty.GetValue(item) != null
                            ? itemProperty.GetValue(item).ToString()
                            : string.Empty;
                        if (string.IsNullOrEmpty(variance.SourceValue) && string.IsNullOrEmpty(variance.SinkValue))
                        {
                            variances.Remove(key);
                        }
                        else if (!string.IsNullOrWhiteSpace(variance.SourceValue) &&
                                 variance.SourceValue.ToLower().Equals(variance.SinkValue.ToLower()))
                        {
                            variances.Remove(key);
                        } 
                    }
                   
                }
                sinkCount--;
            }

            IDictionary<int, IDictionary<string, Variance>> resultset =
                new Dictionary<int, IDictionary<string, Variance>>();
            foreach (var varianceList in varianceCollection)
            {
                if (varianceList.Value.Count > 0)
                {
                    resultset.Add(varianceList);
                }
            }
            return resultset;
        }

        private static bool MeetsOtherRestrictions(Disbursement disbursement, PropertyInfo property)
        {
            if (disbursement.PayeeType == PayeeType.CreditCard && property.Name == "ReferenceNumber")
            {
                return false;
            }
            return true;
        }
    }
}
