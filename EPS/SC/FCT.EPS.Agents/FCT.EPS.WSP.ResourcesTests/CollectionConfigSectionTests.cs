using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
namespace FCT.EPS.WSP.Resources.Tests
{
    [TestClass()]
    public class CollectionConfigSectionTests
    {
        [TestMethod()]
        public void CollectionConfigSectionTest()
        {
            CollectionConfigSection collectionConfigSection = (CollectionConfigSection)ConfigurationManager.GetSection("CollectionConfigSection");

            foreach (var element in collectionConfigSection.ConfigElements.AsEnumerable())
            {
                Console.WriteLine(element.Key);

                foreach (var subElement in element.SubElements.AsEnumerable())
                {
                    Console.WriteLine(subElement.Key);
                }

            }


            ConfigElement element2 = collectionConfigSection.ConfigElements.First(c => c.Key == "Schedule");
            foreach (var subElement in element2.SubElements.AsEnumerable())
            {
                Console.WriteLine(subElement.Key);
            }

        }
    }
}
