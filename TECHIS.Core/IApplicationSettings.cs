using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public interface IApplicationSettings
    {
        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsync();
        IEnumerable<KeyValuePair<string, string>> GetAll();
        Task<string> GetAsync(string key);
        string Get(string key);
        TConfig Get<TConfig>(string sectionName);
        Task<TConfig> GetAsync<TConfig>(string sectionName);
    }
}
