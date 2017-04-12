using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    /// <summary>
    /// A collection if activity messages
    /// </summary>
    /// <typeparam name="T">The Type of the source</typeparam>
    public class ActivityMessages<T>:List<ActivityMessage<T>>
    {
        private T _Source;

        public T Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        public ActivityMessages()
            : base()
        {

        }
        public ActivityMessages(T source) : base() 
        {
            _Source = source;
        }

        public ActivityMessages(T source, int capacity)
            : base(capacity)
        {
 
        }

        public ActivityMessages(T source, IEnumerable<ActivityMessage<T>> collection)
            : base(collection)
        {
            _Source = source;
        }

        public void AddMessage(ActivityMessageTypes type, string data, int classification, string reason, T source)
        {
            Add(new ActivityMessage<T>(type, data, classification, reason, source));
        }

        public void AddMessage(ActivityMessageTypes type, string data, int classification)
        {
            Add(new ActivityMessage<T>(type, data, classification, null, _Source));
        }

        public void AddMessage(ActivityMessageTypes type, string data)
        {
            AddMessage(type, data, 0, null, _Source);
        }

        public void AddInformation(string data)
        {
            AddMessage(ActivityMessageTypes.Information, data, 0, null,_Source);
        }

        public void AddError(string data)
        {
            AddMessage(ActivityMessageTypes.Error, data, 0, null, _Source);
        }

        public void AddWarning(string data)
        {
            AddMessage(ActivityMessageTypes.Warning, data, 0, null, _Source);
        }

        public void AddInformation(string data, string reason)
        {
            AddMessage(ActivityMessageTypes.Information, data, 0, reason, _Source);
        }

        public void AddError(string data, string reason)
        {
            AddMessage(ActivityMessageTypes.Error, data, 0,reason, _Source);
        }

        public void AddWarning(string data, string reason)
        {
            AddMessage(ActivityMessageTypes.Warning, data,0, reason, _Source);
        }
    }
}
