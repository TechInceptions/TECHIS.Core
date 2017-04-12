using System;

namespace TECHIS.Core
{
	/// <summary>
	/// Summary description for IBinarySafe.
	/// </summary>
	public interface IBinarySafe
	{
		 void		Add		(Guid key, byte[] data);
		 byte[]	Remove	(Guid key);
		 byte[]	Get		(Guid key);
	}
}
