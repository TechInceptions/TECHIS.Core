using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TECHIS.Core.Text
{
    [Serializable]
    public class StringReplaceInfo
    {
        [ThreadStatic]
        private static StringBuilder _TextBuilder;
        [XmlAttribute]
        public string Old { get; set; }
        [XmlAttribute]
        public string New { get; set; }


        public static string Replace(string value, List<StringReplaceInfo> criList)
        {
            if (criList == null || criList.Count == 0 || string.IsNullOrEmpty(value))
            {
                return value;
            }
            StringBuilder tb = _TextBuilder;
            if (tb == null)
            {
                tb = new StringBuilder();
                _TextBuilder = tb;
            }
            else
            {
                tb.Length = 0;
            }

            tb.Append(value);
            for (int i = 0; i < criList.Count; i++)
            {
                StringReplaceInfo cr = criList[i];

                tb.Replace(cr.Old, cr.New);
            }

            return tb.ToString();
        }
    }
}
