using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Auction.Configure
{
    public class HousesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("House")]
        public HousesCollection Houses
        {
            get { return ((HousesCollection)(base["House"])); }
        }
    }

    [ConfigurationCollection(typeof(HouseElement))]
    public class HousesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HouseElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HouseElement)(element)).Name;
        }

        public HouseElement this[int idx]
        {
            get { return (HouseElement)BaseGet(idx); }
        }
    }

    public class HouseElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "default", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)(base["name"])); }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Path
        {
            get { return ((string)(base["path"])); }
            set { base["path"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return ((string)(base["type"])); }
            set { base["type"] = value; }
        }
    }
}