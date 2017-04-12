using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public interface IGrouped<T>
    {
        void SetGroup(string groupName, string key, T value);
        T GetGroup(string groupName, string key);
        void SetContext<T2>(List<KeyValuePair<string, T2>> entries);
    }
}
