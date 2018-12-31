//------------------------------------------------------------------------------
//  <copyright from='2005' to='2006' company='WoWSharp.NET'>
//    Copyright (c) WoWSharp.NET. All Rights Reserved.
//
//    Please look in the accompanying license.htm file for the license that 
//    applies to this source code.(a copy can also be found at: 
//    http://www.wowsharp.net/license.htm)
//  </copyright>
//-------------------------------------------------------------------------------
using System;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace DAoC_Bot
{
	/// <summary>
	/// Contains all the Localization settings
	/// </summary>
	public class Localization
	{
		private Hashtable _strings;
        private string _languageid;

		public Localization()
		{
			_strings = new Hashtable();
            _languageid = "en";

			SetLanguage("en");
		}

        public Localization(string languageid)
        {
            _strings = new Hashtable();

            SetLanguage(languageid);
        }

        public void AppendFile(string fn)
        {
            string directory = Path.GetDirectoryName(Application.ExecutablePath);
            if (!directory.EndsWith("\\"))
                directory += "\\";

			if( !File.Exists( directory + fn))
				return;
			//	throw new Exception( "Unknown localization file! " + fn);
			
			XmlDocument doc = new XmlDocument();
            doc.Load(directory + fn);

			foreach (XmlNode node in doc.SelectNodes("//string[@lang='" + _languageid + "']"))
			{
				
				_strings[node.SelectSingleNode("@id").Value.ToLower()] =  node.InnerText;
			}
        }

		public void ClearFile(string fn)
		{
			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if (!directory.EndsWith("\\"))
				directory += "\\";

			XmlDocument doc = new XmlDocument();
			doc.Load(directory + fn);

			foreach (XmlNode node in doc.SelectNodes("//string[@lang='" + _languageid + "']"))
			{
				_strings.Remove(node.SelectSingleNode("@id").Value.ToLower());
			}
		}

        public void LoadFile(string fn)
        {
            _strings.Clear();

            AppendFile(fn);
        }

		public void SetLanguage( string languageid)
		{
            this._languageid = languageid;
            this.LoadFile("localization.xml");
		}

		public string GetString( string stringid)
		{
			if( !_strings.ContainsKey( stringid.ToLower()))
				throw new Exception( string.Format( "Invalid (or not translated) string id found: {0}", stringid));

			return (string) _strings[ stringid.ToLower()];
		}
	}
}
