using System;
using System.Text;

namespace TECHIS.Core
{
	/// <summary>
	/// Summary description for ServiceIdentifier.
	/// </summary>
	[Serializable]
	public class ServiceIdentifier
	{
		#region Constants 
		internal const string SCHEMA_URI = "service://";
		#endregion

		#region Fields 
		private Guid	_SessionID;
		private string	_ServiceName;
		private string	_Description;
		private string	_NetworkHost;
		private string	_InstanceID;
		#endregion

		#region Properties 
		
		/// <summary>
		/// A guid uniquely identifying the service's current session
		/// </summary>
		public Guid		SessionID	
		{
			get
			{
				return _SessionID ;
			}
			set
			{
				_SessionID = value;
			}
		}

		/// <summary>
		/// The logical name of the service
		/// </summary>
		public string	ServiceName	
		{
			get
			{
				return _ServiceName ;
			}
			set
			{
				_ServiceName = value;
			}
		}

		/// <summary>
		/// The description of the service
		/// </summary>
		public string	Description	
		{
			get
			{
				return _Description ;
			}
			set
			{
				_Description = value;
			}
		}

		/// <summary>
		/// The network name of the machine that host the service
		/// </summary>
		public string	NetworkHost	
		{
			get
			{
				return _NetworkHost ;
			}
			set
			{
				_NetworkHost = value;
			}
		}

		/// <summary>
		/// The installation identifier of the service. Use this if the service is installed more than once on a machine
		/// </summary>
		public string	InstanceID	
		{
			get
			{
				return _InstanceID ;
			}
			set
			{
				_InstanceID = value;
			}
		}
		#endregion

		#region Constructors 
		/// <summary>
		/// Creates a new service identifier. A new SessionID will be generated. 
		/// The NetworkHost property will be set to the machine name. The InstanceId will be null
		/// The logical name of the service will be set to ServiceName
		/// </summary>
		/// <param name="ServiceName">The logical name of the service</param>
		public ServiceIdentifier(string ServiceName):this()
		{
			_ServiceName = ServiceName;
		}


		/// <summary>
		/// Creates a new service identifier. A new SessionID will be generated. 
		/// The NetworkHost property will be set to the machine name. The InstanceId will be null
		/// </summary>
		public ServiceIdentifier()
		{
			_SessionID = Guid.NewGuid();
			_NetworkHost = System.Environment.MachineName;

		}

		#endregion

		#region public Methods 
		/// <summary>
		/// Converts this instance to a string, in uri format: service://NetworkHost/InstanceID/ServiceName/SessionID
		/// </summary>
		/// <returns></returns>
		public new string ToString()
		{
			return GetIdentityStream(false).ToString();	
		}
        /// <summary>
        /// Converts this instance to a string, in uri format: service://NetworkHost/InstanceID/ServiceName/SessionID
        /// </summary>
        /// <returns></returns>
        public System.Text.StringBuilder GetIdentityStream(bool WordCharactersOnly)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(100);

            sb.Append(SCHEMA_URI)
                .Append(_NetworkHost)
                .Append('/')
                .Append(_InstanceID)
                .Append(_ServiceName)
                .Append(_SessionID);

            bool Ok = false;

            if (WordCharactersOnly)
            {
                for (int x = 0; x < sb.Length; x++)
                {
                    /*
                        48-57:0-9 
                        65-90:A-Z
                        97-122:a-z
                     */
                    Ok = false;

                    if (sb[x] > 47 && sb[x] < 58)
                        Ok = true;

                    if (sb[x] > 64 && sb[x] < 91)
                        Ok = true;

                    if (sb[x] > 96 && sb[x] < 123)
                        Ok = true;

                    if (!Ok)
                        sb[x] = '_';
                }
            }

            return sb;

        }
		#endregion

        public bool Equals(ServiceIdentifier si)
        {
            return (this == si);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ServiceIdentifier);
        }

        public override int GetHashCode()
        {
            return this.GetIdentityStream(false).ToString().GetHashCode();
        }

        public static bool operator == (ServiceIdentifier si1, ServiceIdentifier si2)
        {
            if (object.ReferenceEquals(si2, si1))
                return true;

            if (object.Equals(si1, null))
                return false;

            if (object.Equals(si2, null))
                return false;


            StringBuilder sb1 = si1.GetIdentityStream(false);
            StringBuilder sb2 = si2.GetIdentityStream(false);

            if (sb1.Length != sb2.Length)
                return false;

            for (int x = 0; x < sb1.Length; x++)
            {
                if (sb1[x] != sb2[x])
                    return false;
            }

            return true;
        }

        public static bool operator !=(ServiceIdentifier si1, ServiceIdentifier si2)
        {
            return ( ! ( si1 == si2 ) );
        }
	}
}
