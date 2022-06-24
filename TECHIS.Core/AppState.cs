using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.Core
{
    public static class AppState
    {
        public static DateTime InitializedAt = DateTime.UtcNow;
        public static Collections.ConcurrentPipe<KeyValuePair<string, string>> CurrentState { get; } = new Collections.ConcurrentPipe<KeyValuePair<string, string>>(50);
        public static StringBuilder AsText()
        {
            var cs = CurrentState.ToArray();
            StringBuilder sb = new StringBuilder();
            
            foreach (var kvp in cs)
            {
                sb.Append(kvp.Key).Append(" : ").AppendLine($"'{kvp.Value}'");
            }

            return sb;
        }

        public static IApplicationSettings StartUpApplicationSettings { get; set; }
    }
}
