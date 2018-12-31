//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Xml;
using System.Globalization;

namespace DAoC_Bot
{
	/// <summary>
	/// Used to update variables in the profile
	/// </summary>
	public delegate void UpdateVariableDelegate( string variableName, object newValue);
	
	/// <summary>
	/// Used to save or load the current profile
	/// </summary>
	public delegate void CurrentProfileDelegate();

	/// <summary>
	/// The interface that should be used by all forms with profile information
	/// </summary>
	public interface IProfileForm
	{
		/// <summary>
		/// Called when initializing the profile
		/// </summary>
		/// <param name="profile">Profile to initialize</param>
		void DefineVariables( Profile profile);

		/// <summary>
		/// Called when the profile changed, e.g. other profile got loaded
		/// </summary>
		/// <param name="profile"></param>
		void OnProfileChange( Profile profile);

		/// <summary>
		/// Event which the profile hooks on to update its variables
		/// </summary>
		event UpdateVariableDelegate UpdateVariable;

		/// <summary>
		/// Event which the pofile hooks on to save its current settings
		/// </summary>
		event CurrentProfileDelegate SaveCurrentProfile;

		/// <summary>
		/// Event which the pofile hooks on to reload its current settings
		/// </summary>
		event CurrentProfileDelegate LoadCurrentProfile;
	}

	/// <summary>
	/// Summary description for Profile.
	/// </summary>
	public class Profile
	{
		/// <summary>
		/// Class used to store variables internally
		/// </summary>
		private class Variable
		{
			internal string Name;
			internal System.Type Type;
			internal object Value;
			internal object DefaultValue;
		}

		/// <summary>
		/// List of all variables
		/// </summary>
		private Hashtable _variables;

		/// <summary>
		/// List of all profile forms
		/// </summary>
		private ArrayList _profileforms;

		/// <summary>
		/// [private] Profile name
		/// </summary>
		private string _profilename;

		/// <summary>
		/// Profile name
		/// </summary>
		public string ProfileName
		{
			get
			{
				return _profilename;
			}
			set
			{
				_profilename = value;
			}
		}

		/// <summary>
		/// Creates the profile object
		/// </summary>
		public Profile()
		{
			_variables = new Hashtable();
			_profileforms = new ArrayList();
		}

		/// <summary>
		/// Adds a profile form, these are used to initialize all the possible 
		/// variables
		/// </summary>
		/// <param name="frm">IProfileForm</param>
		public void AddProfileForm( IProfileForm frm)
		{
			// Load its variables
			frm.DefineVariables( this);

			// Hook the update variable event
			frm.UpdateVariable += new UpdateVariableDelegate(SetValue);

			// Hoko the save / load events
			frm.SaveCurrentProfile += new CurrentProfileDelegate(SaveProfile);
			frm.LoadCurrentProfile += new CurrentProfileDelegate(LoadProfile);

			// Reset the form to default values
			frm.OnProfileChange( this);

			// Add it to the notify list
            _profileforms.Add( frm);
		}
        
		/// <summary>
		/// Define a variable
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <param name="defaultValue">Default value</param>
		/// <remarks>
		/// The default value's type is used as the type for the variable,
		/// checks are done when setting / getting the variable to make sure 
		/// the type stays the same
		/// </remarks>
		public void DefineVariable( string variableName, object defaultValue)
		{
			if( _variables.ContainsKey( variableName))
				throw new Exception( string.Format( "Variable {0} already exists!", variableName));

			Variable variable = new Variable();
			variable.Name = variableName;
			variable.Type = defaultValue.GetType();
			variable.Value = defaultValue;
			variable.DefaultValue = defaultValue;
			_variables.Add( variableName, variable);
		}

		/// <summary>
		/// Resets a variable back to its default value
		/// </summary>
		/// <param name="variableName">Variable name</param>
		public void ResetValue( string variableName)
		{
			if( !_variables.ContainsKey( variableName))
				throw new Exception( string.Format( "Variable {0} doesnt exist!", variableName));

			Variable variable = (Variable)_variables[variableName];
			variable.Value = variable.DefaultValue;

			// Should we do the notify change in this reset too?
		}

		/// <summary>
		/// Resets all variables back to their default value
		/// </summary>
		public void ResetAll()
		{
			foreach( Variable variable in _variables.Values)
				variable.Value = variable.DefaultValue;

			// Notify all forms about the reset
			foreach( IProfileForm frm in _profileforms)
				frm.OnProfileChange(this);
		}

		/// <summary>
		/// Sets a variable value
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <param name="newValue">The new value</param>
		/// <remarks>
		/// Variable type is checked
		/// </remarks>
		public void SetValue( string variableName, object newValue)
		{
			if( !_variables.ContainsKey( variableName))
				throw new Exception( string.Format( "Variable {0} doesnt exist!", variableName));

			Variable variable = (Variable)_variables[variableName];

			if( variable.Type != newValue.GetType())
				throw new Exception( "Wrong variable type!");

			variable.Value = newValue;

			// Should we do the notify change in this setvalue?
		}

		/// <summary>
		/// Gets a variable
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <param name="type">Type of the variable</param>
		/// <returns>Value</returns>
		public object GetValue( string variableName, System.Type type)
		{
			if( !_variables.ContainsKey( variableName))
				throw new Exception( string.Format( "Variable {0} doesnt exist!", variableName));

			Variable variable = (Variable)_variables[variableName];

			if( variable.Type != type)
				throw new Exception( "Wrong variable type!");

			return variable.Value;
		}

		/// <summary>
		/// Gets variable string value
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <returns>Value</returns>
		/// <remarks>
		/// Variable type is checked
		/// </remarks>
		public string GetString( string variableName)
		{
			return (string) GetValue( variableName, typeof( string));
		}

		/// <summary>
		/// Get a variable int value
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <returns>Value</returns>
		/// <remarks>
		/// Variable type is checked
		/// </remarks>
		public int GetInteger( string variableName)
		{
			return (int) GetValue( variableName, typeof( int));
		}

		/// <summary>
		/// Get a variable long value
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <returns>Value</returns>
		/// <remarks>
		/// Variable type is checked
		/// </remarks>
		public long GetLong( string variableName)
		{
			return (long) GetValue( variableName, typeof( long));
		}

		/// <summary>
		/// Get a variable bool value
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <returns>Value</returns>
		/// <remarks>
		/// Variable type is checked
		/// </remarks>
		public bool GetBool( string variableName)
		{
			return (bool) GetValue( variableName, typeof( bool));
		}

		/// <summary>
		/// Get a variable float/single value
		/// </summary>
		/// <param name="variableName">Variable name</param>
		/// <returns>Value</returns>
		/// <remarks>
		/// Variable type is checked
		/// </remarks>
		public float GetFloat( string variableName)
		{
			return (float) GetValue( variableName, typeof( float));
		}

		/// <summary>
		/// Loads a profile
		/// </summary>
		/// <param name="profileName">Profile to load</param>
		/// <remarks>
		/// Profiles are stored at:
		/// [ExecutablePath]\Profiles\[profileName].xml
		/// </remarks>
		public void LoadProfile( string profileName)
		{
			ProfileName = profileName;

			if(ProfileName == null)
				return;

			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "Profiles";

			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			directory += "\\" + profileName + ".xml";

			if( !File.Exists( directory))
				throw new Exception( "Unknown profile!");

			XmlDocument doc = new XmlDocument();
			doc.Load( directory);

			foreach( Variable variable in _variables.Values)
			{
				// Check if the variable exists
				XmlNode variableNode = doc.SelectSingleNode( "//variable[@name='" + variable.Name + "']/@value");
				if( variableNode == null)
					continue;

				// Get the new value
				string newValue = variableNode.Value;
				try
				{	// Convert it from string to the required variable type
					if( variable.Type == typeof( int))
						variable.Value = int.Parse( newValue);
					else
					if( variable.Type == typeof( long))
						variable.Value = long.Parse( newValue);
					else
					if( variable.Type == typeof( string))
						variable.Value = newValue;
					else
					if( variable.Type == typeof( bool))
						variable.Value = bool.Parse( newValue);
					else
					if( variable.Type == typeof( float))
					{
						NumberFormatInfo nfi = new NumberFormatInfo();
						nfi.NumberGroupSeparator = ",";
						nfi.NumberDecimalSeparator = ".";

						variable.Value = float.Parse( newValue, nfi);
					}
				}
				catch
				{	// Something went wrong with loading the variable, reset it to default
					variable.Value = variable.DefaultValue;
				}
			}

			// Notify all forms about a new profile
			foreach( IProfileForm frm in _profileforms)
				frm.OnProfileChange(this);
		}

		/// <summary>
		/// Reload the current profile
		/// </summary>
		public void LoadProfile()
		{
			if( ProfileName == "")
			{
				ResetAll();
				return;
			}

			LoadProfile( ProfileName);
		}

		/// <summary>
		/// Save the current profile
		/// </summary>
		public void SaveProfile()
		{
			if( ProfileName == "")
				return;

			SaveProfile( ProfileName);
		}

		/// <summary>
		/// Saves a profile
		/// </summary>
		/// <param name="profileName">Profile name</param>
		/// <remarks>
		/// Profiles are stored at:
		/// [ExecutablePath]\Profiles\[profileName].xml
		/// </remarks>
		public void SaveProfile( string profileName)
		{
			ProfileName = profileName;

			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "Profiles";

			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			directory += "\\" + profileName + ".xml";

			XmlDocument doc = new XmlDocument();
			doc.LoadXml( "<variables/>");

			foreach( Variable variable in _variables.Values)
			{
				// Get the new value
				string value = "";
				if( variable.Type == typeof( int))
					value = ((int)variable.Value).ToString();
				else
				if( variable.Type == typeof( long))
					value = ((long)variable.Value).ToString();
				else
				if( variable.Type == typeof( string))
					value = (string)variable.Value;
				else
				if( variable.Type == typeof( bool))
					value = ((bool)variable.Value).ToString();
				else
				if( variable.Type == typeof( float))
				{
					NumberFormatInfo nfi = new NumberFormatInfo();
					nfi.NumberGroupSeparator = ",";
					nfi.NumberDecimalSeparator = ".";

					value = ((float)variable.Value).ToString(nfi);
				}
				else
					throw new Exception(string.Format( "Variable {0} is of an unknown type!", variable.Name));

				// Create the element
				XmlNode variableNode = doc.CreateElement("variable");
				
				XmlAttribute attr = doc.CreateAttribute("name");
				attr.Value = variable.Name;
				variableNode.Attributes.Append( attr);

				attr = doc.CreateAttribute("value");
				attr.Value = value;
				variableNode.Attributes.Append( attr);

				doc.DocumentElement.AppendChild( variableNode);
			}

			doc.Save( directory);
		}
	}
}
