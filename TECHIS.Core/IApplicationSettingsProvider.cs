using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public interface IApplicationSettingsProvider
    {
        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsync();
        IEnumerable<KeyValuePair<string, string>> GetAll();
        Task<string> GetAsync(string key);
        string Get(string key);
        TValue Get<TValue>(string key);
        TValue Get<TValue>(string key, TValue value);
    }
}
