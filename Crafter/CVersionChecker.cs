using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Crafter
{
	public class CVersionChecker
	{
		public CVersionChecker()
		{
		}

		public void Check(App TheApp)
		{
			string str = this.LoadLocalXML(TheApp);
			string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().get_Location()).get_FileVersion();
			if (StringType.StrCmp(str, FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().get_Location()).get_FileVersion(), false) != 0)
			{
				MessageBox.Show(string.Concat("You are running an old version ", fileVersion, " you need to update.  Current version is ", str), "AutoKiller Version Checker");
			}
		}

		private string LoadLocalXML(App TheApp)
		{
			XmlElement xmlElement;
			string attribute = null;
			XmlDocument xmlDocument = new XmlDocument();
			string str = "http://www.autokiller.com/updates/versions.xml";
			WebClient webClient = new WebClient();
			try
			{
				xmlDocument.Load(webClient.OpenRead(str));
				XmlElement xmlElement1 = (XmlElement)xmlDocument.SelectSingleNode("//section[@name='AutoKiller Updates']");
				switch (TheApp)
				{
					case App.AutoKillerMap:
					{
						xmlElement = (XmlElement)xmlElement1.SelectSingleNode("item[@key='AutoKillerMap']");
						if (xmlElement == null)
						{
							break;
						}
						attribute = xmlElement.GetAttribute("newValue");
						return attribute;
					}
					case App.AutoKillerCrafter:
					{
						xmlElement = (XmlElement)xmlElement1.SelectSingleNode("item[@key='AutoKillerCrafter']");
						if (xmlElement == null)
						{
							break;
						}
						attribute = xmlElement.GetAttribute("newValue");
						return attribute;
					}
					case App.AutoKillerHunter:
					{
						xmlElement = (XmlElement)xmlElement1.SelectSingleNode("item[@key='AutoKillerHunter']");
						if (xmlElement == null)
						{
							break;
						}
						attribute = xmlElement.GetAttribute("newValue");
						return attribute;
					}
					case App.AutoKillerDXMapLoader:
					{
						xmlElement = (XmlElement)xmlElement1.SelectSingleNode("item[@key='AutoKillerDXMapLoader']");
						if (xmlElement == null)
						{
							break;
						}
						attribute = xmlElement.GetAttribute("newValue");
						return attribute;
					}
					case App.AutoKillerWindowDXMap:
					{
						xmlElement = (XmlElement)xmlElement1.SelectSingleNode("item[@key='AutoKillerWindowDXMap']");
						if (xmlElement == null)
						{
							break;
						}
						attribute = xmlElement.GetAttribute("newValue");
						return attribute;
					}
					default:
					{
						return attribute;
					}
				}
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
			return attribute;
		}
	}
}