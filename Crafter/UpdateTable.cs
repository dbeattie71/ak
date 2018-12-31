using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace Crafter
{
	[Serializable]
	public class UpdateTable
	{
		public ArrayList Hash;

		public UpdateTable()
		{
			this.Hash = new ArrayList();
		}

		public ArrayList Deserialize(string filename)
		{
			if (!(new FileInfo(filename)).get_Exists())
			{
				return new ArrayList();
			}
			FileStream fileStream = new FileStream(filename, 3);
			ArrayList arrayList = this.Deserialize(fileStream);
			fileStream.Close();
			return arrayList;
		}

		public ArrayList Deserialize(Stream stream)
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			soapFormatter.set_AssemblyFormat(0);
			return (ArrayList)soapFormatter.Deserialize(stream);
		}

		public void Serialize(string filename)
		{
			string str = string.Concat(filename, ".tmp");
			FileInfo fileInfo = new FileInfo(str);
			if (fileInfo.get_Exists())
			{
				fileInfo.Delete();
			}
			FileStream fileStream = new FileStream(str, 2);
			this.Serialize(fileStream);
			fileStream.Close();
			fileInfo.CopyTo(filename, true);
			fileInfo.Delete();
		}

		public void Serialize(Stream stream)
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			soapFormatter.set_AssemblyFormat(0);
			soapFormatter.Serialize(stream, this.Hash);
		}
	}
}