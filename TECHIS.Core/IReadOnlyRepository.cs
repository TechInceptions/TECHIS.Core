
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public interface IReadOnlyRepository
    {
        Task<IEnumerable<TOutput>> GetData<TOutput>(string query, bool isProcedureName = false);
        Task<IEnumerable<TOutput>> GetData<TOutput>(string query, object input, bool isProcedureName = false);
    }
}