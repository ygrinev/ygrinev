using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.Resources
{
    public class CollectionConfigSection : ConfigurationSection
    {

        [ConfigurationProperty("ConfigElements", IsRequired = true)]
        public ConfigElementsCollection ConfigElements
        {
            get
            {
                return base["ConfigElements"] as ConfigElementsCollection;
            }
        }

    }

    [ConfigurationCollection(typeof(ConfigElement), AddItemName = "ConfigElement")]
    public class ConfigElementsCollection : ConfigurationElementCollection, IEnumerable<ConfigElement>
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var l_configElement = element as ConfigElement;
            if (l_configElement != null)
                return l_configElement.Key;
            else
                return null;
        }

        public ConfigElement this[int index]
        {
            get
            {
                return BaseGet(index) as ConfigElement;
            }
        }

        #region IEnumerable<ConfigElement> Members

        IEnumerator<ConfigElement> IEnumerable<ConfigElement>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, this.Count)
                    select this[i])
                    .GetEnumerator();
        }

        #endregion
    }

    public class ConfigElement : ConfigurationElement
    {

        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get
            {
                return base["key"] as string;
            }
            set
            {
                base["key"] = value;
            }
        }

        [ConfigurationProperty("SubElements")]
        public ConfigSubElementsCollection SubElements
        {
            get
            {
                return base["SubElements"] as ConfigSubElementsCollection;
            }
        }

    }

    [ConfigurationCollection(typeof(ConfigSubElement), AddItemName = "ConfigSubElement")]
    public class ConfigSubElementsCollection : ConfigurationElementCollection, IEnumerable<ConfigSubElement>
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigSubElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var l_configElement = element as ConfigSubElement;
            if (l_configElement != null)
                return l_configElement.Key;
            else
                return null;
        }

        public ConfigSubElement this[int index]
        {
            get
            {
                return BaseGet(index) as ConfigSubElement;
            }
        }

        #region IEnumerable<ConfigSubElement> Members

        IEnumerator<ConfigSubElement> IEnumerable<ConfigSubElement>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, this.Count)
                    select this[i])
                    .GetEnumerator();
        }

        #endregion
    }

    public class ConfigSubElement : ConfigurationElement
    {

        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get
            {
                return base["key"] as string;
            }
            set
            {
                base["key"] = value;
            }
        }

    }
}
