//using System;
//using System.Runtime.Serialization.Formatters.Binary;

//namespace TECHIS.Core.Serialization
//{
//	/// <summary>
//	/// Summary description for BinarySerializer.
//	/// </summary>
//	public class BinarySerializer
//	{
//		public BinarySerializer()
//		{
//			//
//			// TODO: Add constructor logic here
//			//
//		}
//		public static byte[] Serialize(object data)
//		{
//			BinaryFormatter bf = new BinaryFormatter();
//			System.IO.MemoryStream ms = new System.IO.MemoryStream();
//			bf.Serialize(ms,data);

//			return ms.ToArray();
//		}
//		public static object Deserialize(byte[] data)
//		{
//			BinaryFormatter bf = new BinaryFormatter();
//			System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
//			return bf.Deserialize(ms);
//		}
//	}
//}
