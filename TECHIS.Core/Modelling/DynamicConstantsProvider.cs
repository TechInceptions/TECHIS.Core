using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public class DynamicConstantsProvider
    {
        private Dictionary<string, string> _ConstantsValues;
        public static string GetVersion()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
        public DynamicConstantsProvider(TECHIS.Core.Collections.NamedGroup<Attribute> dynamicConsts)
        {
            _ConstantsValues = new Dictionary<string, string>();

            for (int i = 0; i < dynamicConsts.Items.Count; i++)
            {
                Attribute atr = dynamicConsts.Items[i];
                _ConstantsValues.Add(atr.Name, atr.Value);
            }
        }

        public virtual string this[string key]
        {
            get
            {
                return _ConstantsValues[key];
            }
        }

        public virtual void Add(string key, string value)
        {
            if (! string.IsNullOrEmpty(key))
            {
                _ConstantsValues[key] = value;
            }
        }

        public virtual void AddRange(Dictionary<string, string> keyValues)
        {
            foreach (var item in keyValues)
            {
                Add(item.Key, item.Value);
            }
        }
    }
}
