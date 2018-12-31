//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Windows.Forms;
using AutoKillerScript;
using System.IO;
using System.Xml;
using System.Globalization;

namespace DAoC_Bot
{
	public enum PatrolType
	{
		PatrolArea,
		TravelPath
	}
	public enum MovementDirection
	{
		Forward,
		Back
	}
	#region PatrolAreas class
	/// <summary>
	/// Summary description for PatrolArea.
	///		Contains all hunting locations and their names, and target data for that location
	/// </summary>
	
	public class PatrolAreas
	{
		private AutoKillerScript.clsAutoKillerScript _ak;

		private Hashtable _mPatrolAreas;

		/// <summary>
		/// Create a new PatrolAreas class that can contain PatrolArea objects
		/// </summary>
		/// <param name="wow">The WoW!Sharp object</param>
		/// <param name="wow">PatrolType indicating what type of list it is</param>
		internal PatrolAreas( AutoKillerScript.clsAutoKillerScript ak)
		{
			_ak = ak;
			_mPatrolAreas = Hashtable.Synchronized(new Hashtable());

			LoadPatrolAreas();
		}

		/// <summary>
		/// Add a new patrol area without specifying target names
		/// </summary>
		/// <param name="name">The name of the new patrol area</param>
		/// <param name="type">A PatrolType indicating what type of list this is</param>
		public void AddPatrolArea(string name, PatrolType type)
		{
			if( name == "")
				throw new Exception( "You must specify a name for a new PatrolArea!");

			lock(_mPatrolAreas.SyncRoot)
			{
				if( _mPatrolAreas.ContainsKey( name))
					throw new Exception( "Patrol Area " + name + " already exists!");

				_mPatrolAreas.Add(name, new PatrolArea(_ak, type, name));
			}
		}

		/// <summary>
		/// Add a new patrol area and specify target names
		/// </summary>
		/// <param name="name">The name of the new patrol area</param>
		/// <param name="targetNames">An ArrayList containing the names of targets for this new patrol area</param>
		/// <param name="type">A PatrolType indicating what type of list this is</param>
		public void AddPatrolArea(string name, ArrayList targetNames, PatrolType type)
		{
			if( name == "")
				throw new Exception( "You must specify a name for a new PatrolArea!");

			lock(_mPatrolAreas.SyncRoot)
			{
				if( _mPatrolAreas.ContainsKey( name))
					throw new Exception( "Patrol Area " + name + " already exists!");

				_mPatrolAreas.Add(name, new PatrolArea(_ak, targetNames, type, name));
			}
		}

		/// <summary>
		/// Get a PatrolArea object by name
		/// </summary>
		/// <param name="name">The name of the patrol area to get</param>
		internal PatrolArea GetPatrolArea(string name)
		{
			lock(_mPatrolAreas.SyncRoot)
			{
				if( ! _mPatrolAreas.ContainsKey( name))
					throw new Exception( "Patrol Area " + name + " does not exist!");

				return (PatrolArea)_mPatrolAreas[name];
			}
		}

		/// <summary>
		/// Get an ICollection of all PatrolArea names
		/// </summary>
		internal ICollection GetAllPatrolAreas()
		{
			lock(_mPatrolAreas.SyncRoot)
			{
				string[] patrolAreaNames = new string[_mPatrolAreas.Keys.Count];

				// Make a copy of patrol area keys in order to avoid sync issues if keys are modified
				// during enumeration.
				_mPatrolAreas.Keys.CopyTo(patrolAreaNames, 0);

				return patrolAreaNames;
			}
		}

		/// <summary>
		/// Remove a PatrolArea object by name
		/// </summary>
		/// <param name="name">The name of the patrol area to remove</param>
		public void RemovePatrolArea(string name)
		{
			lock(_mPatrolAreas.SyncRoot)
			{
				if( ! _mPatrolAreas.ContainsKey( name))
					return;

				_mPatrolAreas.Remove(name);
			}

			//now we need to remove the saved file also
			//get path
			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "PatrolAreas";
			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			String fileName = directory + "\\" + name + ".xml";

			File.Delete(fileName);

		}

		/// <summary>
		/// Load all PatrolAreas
		/// </summary>
		/// <remarks>
		/// PatrolAreas are stored at:
		/// [ExecutablePath]\PatrolAreas\[PatrolArea].xml
		/// </remarks>
		public void LoadPatrolAreas( )
		{
			// create NumberFormatInfo object for converting string to float
			NumberFormatInfo nfi = new NumberFormatInfo();
			nfi.NumberGroupSeparator = ",";
			nfi.NumberDecimalSeparator = ".";

			//get path
			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "PatrolAreas";
			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			//get all files
			string[ ] patrolareaFiles = Directory.GetFiles(directory,"*.xml");

			lock(_mPatrolAreas.SyncRoot)
			{
				//get rid of any old entries
				_mPatrolAreas.Clear();

				//process each file
				foreach (string filename in patrolareaFiles )
				{
					XmlDocument doc = new XmlDocument();
					doc.Load( filename);

					XmlNode root = doc.DocumentElement;

					IEnumerator eachPatrolArea = root.GetEnumerator();
					XmlNode patrolarea;
					while (eachPatrolArea.MoveNext()) 
					{     
						patrolarea = (XmlNode) eachPatrolArea.Current;
						//create a new PatrolArea object using the name from the root
						string paName = patrolarea.Attributes.GetNamedItem("Name").Value; 
						if(paName == "")
							continue;
						PatrolType type = (PatrolType)Enum.Parse(typeof(PatrolType), patrolarea.Attributes.GetNamedItem("Type").Value);
						AddPatrolArea(paName, type);

						IEnumerator eachPatrolAreaElement = patrolarea.GetEnumerator();
						XmlNode patrolareaElement;
						while (eachPatrolAreaElement.MoveNext()) 
						{     
							patrolareaElement = (XmlNode) eachPatrolAreaElement.Current;
							if(patrolareaElement.Name == "Target")
								GetPatrolArea(paName).AddTarget(patrolareaElement.Attributes.GetNamedItem("Name").Value);
							if(patrolareaElement.Name == "Waypoint")
								GetPatrolArea(paName).AddWaypoint(float.Parse( patrolareaElement.Attributes.GetNamedItem("X").Value, nfi),
									float.Parse( patrolareaElement.Attributes.GetNamedItem("Y").Value, nfi),
									float.Parse( patrolareaElement.Attributes.GetNamedItem("Z").Value, nfi),
									patrolareaElement.Attributes.GetNamedItem("PointName").Value);

						}					
					}
				}
			}
		}
		/// <summary>
		/// Saves all PatrolAreas
		/// </summary>
		/// <remarks>
		/// PatrolAreas are stored at:
		/// [ExecutablePath]\PatrolAreas\[PatrolArea].xml
		/// </remarks>
		public void SavePatrolAreas( )
		{
			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "PatrolAreas";

			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			lock(_mPatrolAreas.SyncRoot)
			{
				foreach (string patrolareaname in _mPatrolAreas.Keys )
				{
					String fileName = directory + "\\" + patrolareaname + ".xml";

					XmlDocument doc = new XmlDocument();
					doc.LoadXml( "<PatrolAreas/>");

					// Create the element
					XmlNode patrolareaNode = doc.CreateElement("PatrolArea");

					//add the PatrolArea name
					XmlAttribute attr = doc.CreateAttribute("Name");
					attr.Value = patrolareaname;
					patrolareaNode.Attributes.Append( attr);
					
					// Let the PatrolArea save
					PatrolArea patrolarea = GetPatrolArea(patrolareaname);
					patrolarea.SaveYourself(doc, patrolareaNode);

					//append to the xml doc
					doc.DocumentElement.AppendChild( patrolareaNode);

					// Let each Waypoint save
					ArrayList waypoints = patrolarea.GetAllWaypoints;

					// Remember to lock on the SyncRoot before enumeration.
					lock(waypoints.SyncRoot)
					{
						foreach (Waypoint wpt in waypoints)
						{
							// Create the element
							XmlNode waypointNode = doc.CreateElement("Waypoint");

							// Let the Waypoint save
							wpt.SaveYourself(doc, waypointNode);

							//append to the xml doc
							patrolareaNode.AppendChild( waypointNode);
						}
					}

					//save it
					doc.Save( fileName);
				}
			}
		}
	}


	#endregion

	#region PatrolArea class

	/// <summary>
	/// Summary description for PatrolArea.
	///		Contains a list of target names and a list of waypoints for a hunting location
	///		Can also be used just a list of waypoints for travel purposes
	/// </summary>
	internal class PatrolArea
	{
		private ArrayList _mWaypoints;
		private int _mWayIterator = -1;
		private MovementDirection _bCycleForward = MovementDirection.Forward ;
		private ArrayList _mTargetNames;
		private AutoKillerScript.clsAutoKillerScript _ak;
		private PatrolType _type;
		private string _name;

		/// <summary>
		/// Create a new waypoint list
		/// </summary>
		internal PatrolArea(AutoKillerScript.clsAutoKillerScript ak, PatrolType type, string name)
		{
			this._ak = ak;
			_type = type;
			_name = name;

			_mWaypoints = ArrayList.Synchronized(new ArrayList());
			_mTargetNames = ArrayList.Synchronized(new ArrayList(6));
		}

		/// <summary>
		/// Create a new waypoint list with target names
		/// </summary>
		/// <param name="ak">A AutoKillerScript.clsAutoKillerScript ak object used for calculating distances etc.</param>
		/// <param name="targetNames">An ArrayList of target names to associate with this PatrolArea</param>
		internal PatrolArea(AutoKillerScript.clsAutoKillerScript ak, ArrayList targetNames, PatrolType type, string name)
		{
			this._ak = ak;
			_type = type;
			_name = name;

			_mWaypoints = ArrayList.Synchronized(new ArrayList());
			_mTargetNames = ArrayList.Synchronized(new ArrayList(targetNames));
		}

		/// <summary>
		/// Save PatrolArea data to xml
		/// </summary>
		/// <param name="doc">The XmlDocument to which we are saving</param>
		/// <param name="patrolareaNode">An XmlNode to which we add attributes for all data</param>
		internal void SaveYourself(XmlDocument doc, XmlNode patrolareaNode)
		{
			//add the PatrolArea type to the Node we were passed
			XmlAttribute attr = doc.CreateAttribute("Type");
			attr.Value = _type.ToString();
			patrolareaNode.Attributes.Append( attr);

			lock(_mTargetNames.SyncRoot)
			{
				foreach (string target in _mTargetNames)
				{
					XmlNode targetNode = doc.CreateElement("Target");

					attr = doc.CreateAttribute("Name");
					attr.Value = target;
					targetNode.Attributes.Append( attr);
					
					patrolareaNode.AppendChild(targetNode);
				}
			}
		}

		/// <summary>
		/// Add a new waypoint to the end of the waypoint list
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <param name="z">z</param>
		public int AddWaypoint(float x, float y, float z, string name)
		{
			lock(_mWaypoints.SyncRoot)
			{
				return _mWaypoints.Add(new Waypoint(_ak, x, y, z, name, 1));
			}
		}

		/// <summary>
		/// Get the next Waypoint at which to hunt
		/// </summary>
		/// <remarks>
		/// This automatically advances to the next or previous waypoint,
		/// reversing the direction of iteration at each end of the list
		/// </remarks>
		public Waypoint CycleWaypoints()
		{
			lock(_mWaypoints.SyncRoot)
			{
				//first lets check for the turn around at each end of the list
				if(_bCycleForward == MovementDirection.Forward  && _mWayIterator == _mWaypoints.Count-1)
				{
					Waypoint first = (Waypoint)_mWaypoints[0]; 
					Waypoint last = (Waypoint)_mWaypoints[_mWaypoints.Count - 1]; 

					float dist = GetDistance(first, last.X, last.Y, last.Z); 
					if(dist < 30) 
					{ 
						_mWayIterator = -1; 
					} 
					else 
					{
						_bCycleForward = MovementDirection.Back ;
					}			
				}
				if(_bCycleForward == MovementDirection.Back  && _mWayIterator == 0)
				{
					_bCycleForward = MovementDirection.Forward ;
				}

				//now get the "next" waypoint in whichever direction
				if(_bCycleForward == MovementDirection.Forward )
					return GetForwardWaypoint();
				else
					return GetPreviousWaypoint();
			}
		}

		/// <summary>
		/// Get an ArrayList of all Waypoint objects. (Don't forget to lock on the SyncRoot property
		/// of the ArrayList if you are going to enumerate the list.)
		/// </summary>
		public ArrayList GetAllWaypoints
		{
			get
			{
				return _mWaypoints;
			}
		}

		public int NumWaypoints
		{
			get
			{
				lock(_mWaypoints.SyncRoot)
				{
					return _mWaypoints.Count;
				}
			}
		}

		/// <summary>
		/// Clear all Waypoint objects
		/// </summary>
		public void ClearAllWaypoints()
		{
			lock(_mWaypoints.SyncRoot)
			{
				_mWaypoints.Clear();
			}
		}

		/// <summary>
		/// Get the most recently added Waypoint
		/// </summary>
		/// <remarks>
		/// Useful for adding a waypoint, and immediately getting the new waypoint object
		/// for use in a listview.Tag
		/// </remarks>
		public Waypoint GetNewestWaypoint()
		{
			lock(_mWaypoints.SyncRoot)
			{
				if(_mWaypoints.Count > 0)
					return (Waypoint)_mWaypoints[_mWaypoints.Count - 1];
				else
					return null;
			}
		}

		/// <summary>
		/// Find which direction a named waypoint is from players coordinates
		/// </summary>
		/// <param name="waypointname">The name of the waypoint for which we are finding the direction</param>
		public MovementDirection GetDirectionTo(string waypointname)
		{
			//lets look forward to the end of the list to see if the name is there somewhere
			Waypoint wpt = FindClosest(MovementDirection.Forward );
			while(wpt != null)
			{
				//have we gotten to the selected destination going forward?
				if(wpt.Name == waypointname)
					return MovementDirection.Forward ;

				wpt = GetForwardWaypoint();
			}
			//if we didnt find it going forward, we have to assume the named point is backward
			return MovementDirection.Back ;
		}

		/// <summary>
		/// Find which direction a waypoint is from players coordinates
		/// </summary>
		/// <param name="targetwpt">The waypoint for which we are finding the direction</param>
		public MovementDirection GetDirectionToWaypoint(Waypoint targetwpt)
		{
			//lets look forward to the end of the list to see if the name is there somewhere
			Waypoint wpt = FindClosest(MovementDirection.Forward );
			while(wpt != null)
			{
				//have we gotten to the selected destination going forward?
				if(wpt.X == targetwpt.X && wpt.Y == targetwpt.Y )
					return MovementDirection.Forward ;

				wpt = GetForwardWaypoint();
			}
			//if we didnt find it going forward, we have to assume the named point is backward
			return MovementDirection.Back ;
		}

		/// <summary>
		/// Get a Waypoint object for the current position in the PatrolArea
		/// </summary>
		public Waypoint GetCurrentWaypoint()
		{
			lock(_mWaypoints.SyncRoot)
			{
				if(_mWayIterator != -1)
					return (Waypoint)_mWaypoints[_mWayIterator];
				else
					return null;
			}
		}

		/// <summary>
		/// Get the next forward Waypoint object for this PatrolArea
		/// Sets direction flag to Forward and iterator to the returned waypoing
		/// </summary>
		public Waypoint GetForwardWaypoint()
		{
			lock(_mWaypoints.SyncRoot)
			{
				_bCycleForward = MovementDirection.Forward ;
				if(_mWayIterator < _mWaypoints.Count-1)
					return (Waypoint)_mWaypoints[++_mWayIterator];
				else
					return null;
			}
		}

		/// <summary>
		/// Get the previous Waypoint object for this PatrolArea
		/// Sets direction flag to Back and iterator to the returned waypoing
		/// </summary>
		public Waypoint GetPreviousWaypoint()
		{
			lock(_mWaypoints.SyncRoot)
			{
				_bCycleForward = MovementDirection.Back ;
				if(_mWayIterator > 0)
					return (Waypoint)_mWaypoints[--_mWayIterator];
				else
					return null;
			}
		}

		/// <summary>
		/// Get the Waypoint object for this PatrolArea based on the current direction setting
		/// </summary>
		public Waypoint GetNextWaypoint()
		{
			if(_bCycleForward == MovementDirection.Back)
				return GetPreviousWaypoint();

			if(_bCycleForward == MovementDirection.Forward)
				return GetForwardWaypoint();

			return null;
		}


		/// <summary>
		/// Find the waypoint closest to specified coordinates, and sets the current position in the list to that waypoint
		/// </summary>
		/// <remarks>
		/// This also sets the internal iterator so that the next call to 
		/// GetForwardWaypoint (if the direction set here is forward)
		///  or GetPreviousWaypoint (if the direction set here is back)
		///  will retrieve the same waypoint that this function returns
		/// </remarks>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="z">The z coordinate</param>
		/// <param name="bForward">True = find the next closest waypoint going forward, false = going backward</param>
		public Waypoint FindClosest(float x, float y, float z, MovementDirection direction)
		{
			float closest = 99999;
			int closestIndex = -1;
			float second = 99999;
			int secondIndex = 0;

			int index = 0;
			lock(_mWaypoints.SyncRoot)
			{
				foreach (Waypoint wpt in _mWaypoints)
				{
					float dist = GetDistance(wpt, x, y, z);
					if(dist < closest)
					{
						second = closest;
						secondIndex = closestIndex;
						closest = dist;
						closestIndex = index;
					}
					index++;
				}
			
				//we now know the two points closest to us, and we know which comes first in the waypoint list
				if(direction == MovementDirection.Forward ) //we want the point with a higher index which should be forward in the list
				{
					if(closestIndex > secondIndex)
					{
						SetNextWaypoint(closestIndex);
						return (Waypoint)_mWaypoints[closestIndex];
					}
					else
					{
						SetNextWaypoint(secondIndex);
						return (Waypoint)_mWaypoints[secondIndex];
					}
				}
				else //if we are heading backward through the list, we want the lower index which should be backward in the list
				{
					if(closestIndex < secondIndex || secondIndex == -1)
					{
						SetNextWaypoint(closestIndex);
						return (Waypoint)_mWaypoints[closestIndex];
					}
					else
					{
						SetNextWaypoint(secondIndex);
						return (Waypoint)_mWaypoints[secondIndex];
					}
				}
			}
		}

		/// <summary>
		/// Find the waypoint closest to the player
		/// </summary>
		/// <remarks>
		/// This also sets the internal iterator so that the next call to 
		/// GetForwardWaypoint (if the direction set here is forward)
		///  or GetPreviousWaypoint (if the direction set here is back)
		///  will retrieve the same waypoint that this function returns
		/// </remarks>
		/// <param name="bForward">True = find the next closest waypoint going forward, false = going backward</param>
		public Waypoint FindClosest(MovementDirection direction)
		{
			return FindClosest(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord,  direction );
		}

		/// <summary>
		/// Set what point will next be returned by a call to CycleWaypoints
		/// </summary>
		/// <param name="point">The number of the point in the waypoint list</param>
		public void SetNextWaypoint(int point)
		{
			lock(_mWaypoints.SyncRoot)
			{
				if(_bCycleForward == MovementDirection.Forward )  //need to offset by one depending on direction
				{
					_mWayIterator = point-1;
					if(_mWayIterator > _mWaypoints.Count-2)  // -2 means that next forward call would be last point in array
						_mWayIterator = _mWaypoints.Count-2;	//safety net, dont let anyone set index such that nextpoint call is out of range
					if(_mWayIterator < 0)
						_mWayIterator = 0;					//if we are before the first point, dont set to -1
				}
				else
				{
					_mWayIterator = point+1;
					if(_mWayIterator < 1)	//1 means the next previouspoint call would be first point in array
						_mWayIterator = 1;	//safety net, dont let anyone set index such that previouspoint call is out of range
					if(_mWayIterator >= _mWaypoints.Count )
						_mWayIterator = _mWaypoints.Count - 1;					//if we are after the last point, dont set to beyond the end of the list
				}
			}
		}

		/// <summary>
		/// Get PatrolType for this PatrolArea or TargetPath list
		/// </summary>
		public PatrolType Type
		{
			get
			{
				return _type;
			}
		}

		/// <summary>
		/// Get the Name for this PatrolArea or TargetPath list
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// Get the number of the current waypoint 
		/// </summary>
		public int iCurrentWaypoint
		{
			get
			{
				return _mWayIterator;
			}
		}

		/// <summary>
		/// Get MovementDirection setting 
		/// </summary>
		public MovementDirection Direction
		{
			get
			{
				return _bCycleForward;
			}
			set
			{
				_bCycleForward = value;
			}
		}

		/// <summary>
		/// Get or Set all the targets for this PatrolArea list
		/// </summary>
		public ArrayList Targets
		{
			get
			{
				return _mTargetNames;
			}
			set
			{
				_mTargetNames = value;
			}
		}

		/// <summary>
		/// Get number of targets
		/// </summary>
		public int NumTargets
		{
			get
			{
				lock(_mTargetNames.SyncRoot)
				{
					return _mTargetNames.Count;
				}
			}
		}

		/// <summary>
		/// Get the specified target for this PatrolArea list
		/// </summary>
		/// <param name="index">The zero based index of the Target name to get</param>
		public string GetTargetAt(int index)
		{
			lock(_mTargetNames.SyncRoot)
			{
				if( index > _mTargetNames.Count - 1 || index < 0)
					throw new Exception( "Invalid index for Target list!");
				
				return (string)_mTargetNames[index];
			}
		}

		/// <summary>
		/// Set the specified target for this PatrolArea list
		/// </summary>
		/// <param name="index">The zero based index of the Target name to set</param>
		public void SetTargetAt(int index, string target)
		{
			lock(_mTargetNames.SyncRoot)
			{
				if( index > _mTargetNames.Count - 1 || index < 0)
					throw new Exception( "Invalid index for Target list!");
				
				_mTargetNames[index] = target;
			}
		}

		/// <summary>
		/// Add a new Target to this PatrolArea
		/// </summary>
		/// <param name="targetName">The name of the target to add</param>
		public void AddTarget(string targetName)
		{
			lock(_mTargetNames.SyncRoot)
			{
				_mTargetNames.Add(targetName);
			}
		}

		/// <summary>
		/// Remove all Targets from this PatrolArea
		/// </summary>
		public void ClearTargets()
		{
			lock(_mTargetNames.SyncRoot)
			{
				_mTargetNames.Clear();
			}
		}

		/// <summary>
		/// Helper function for finding closest waypoint
		/// </summary>
		internal float GetDistance(Waypoint wpt, float x, float y, float z)
		{
			float dX = wpt.X - x;
			float dY = wpt.Y - y;
			float dZ = (z != 0 ? wpt.Z - z : 0);

			return (float)Math.Sqrt(dX*dX + dY*dY + dZ*dZ);
		}


	}
	#endregion

	#region Waypoint class
	
	/// <summary>
	/// Summary description for Waypoints.
	///		List of X,Y,Z waypoints for use WaypointList
	/// </summary>
	/// 
	internal class Waypoint
	{
		private AutoKillerScript.clsAutoKillerScript  _ak;
		private float _mX;
		private float _mY;
		private float _mZ;
		private string _pointName;
		private int _mType; //1=keep travelling, 2=try to fight
		/// <summary>
		/// Waypoint constructor
		/// </summary>
		/// <param name="ak">A AutoKillerScript.clsAutoKillerScript object</param>
		/// <param name="x">The X coordinate of the new waypoint</param>
		/// <param name="y">The Y coordinate of the new waypoint</param>
		/// <param name="z">The Z coordinate of the new waypoint</param>
		/// <param name="name">The optional name of the new waypoint</param>
		/// <param name="type">Unused, for future concept of fighting at just some points</param>
		public Waypoint(AutoKillerScript.clsAutoKillerScript ak, float x, float y, float z, string name, int type)
		{
			_ak = ak;
			_mX = x;
			_mY = y;
			_mZ = z;
			_pointName = name;
			_mType = type;
		}
		/// <summary>
		/// Waypoint constructor
		/// </summary>
		/// <param name="ak">A AutoKillerScript.clsAutoKillerScript object</param>
		/// <param name="wpt">The Waypoint object to copy and add</param>
		public Waypoint(AutoKillerScript.clsAutoKillerScript ak, Waypoint wpt)
		{
			_ak = ak;
			_mX = wpt.X ;
			_mY = wpt.Y ;
			_mZ = wpt.Z ;
			_pointName = wpt.Name;
			_mType = wpt.Type ;
		}
		/// <summary>
		/// Save Waypoint data to xml
		/// </summary>
		/// <param name="doc">The XmlDocument to which we are saving</param>
		/// <param name="waypointNode">An XmlNode to which we add attributes for all data</param>
		internal void SaveYourself(XmlDocument doc, XmlNode waypointNode)
		{
			NumberFormatInfo nfi = new NumberFormatInfo();
			nfi.NumberGroupSeparator = ",";
			nfi.NumberDecimalSeparator = ".";

			XmlAttribute attr = doc.CreateAttribute("X");
			attr.Value = _mX.ToString(nfi);
			waypointNode.Attributes.Append( attr);

			attr = doc.CreateAttribute("Y");
			attr.Value = _mY.ToString(nfi);
			waypointNode.Attributes.Append( attr);

			attr = doc.CreateAttribute("Z");
			attr.Value = _mZ.ToString(nfi);
			waypointNode.Attributes.Append( attr);

			attr = doc.CreateAttribute("PointName");
			attr.Value = _pointName;
			waypointNode.Attributes.Append( attr);

			attr = doc.CreateAttribute("Type");
			attr.Value = _mType.ToString();
			waypointNode.Attributes.Append( attr);
		}

		public float X
		{
			get
			{
				return _mX;
			}
			set
			{
				_mX = value;
			}
		}
		public float Y
		{
			get
			{
				return _mY;
			}
			set
			{
				_mY = value;
			}
		}
		public float Z
		{
			get
			{
				return _mZ;
			}
			set
			{
				_mZ = value;
			}
		}
		public int Type
		{
			get
			{
				return _mType;
			}
			set
			{
				_mType = value;
			}
		}

		/// <summary>
		///	Get or Set the Name associated with this Waypoint
		/// </summary>
		public string Name
		{
			get
			{
				return _pointName;
			}
			set
			{
				_pointName = value;
			}
		}

		/// <summary>
		///	Moves to a Waypoint until within distance
		/// </summary>
		/// <param name="distance">stop/return if within distance</param>
		/// <param name="stoprunning">before exiting, stop running</param>
		/// <param name="stopondamage">stop running when the player is damaged</param>
		public void MoveTo(float distance, bool stoprunning, bool stopondamage)
		{
			if(_ak.ZDistance(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord,_mX,_mY,_mZ) > distance)
				_ak.StartRunning();
			int lasthealth = _ak.PlayerHealth ;

			while(_ak.ZDistance(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord,_mX,_mY,_mZ) > distance)
			{
				if(stopondamage && _ak.PlayerHealth < lasthealth)
				{
					_ak.StopRunning();
					return;
				}
				lasthealth = _ak.PlayerHealth ;
				
				//on slow machines (mine!) turnToHeading this often is bad, so lets only turn if we 
				// are really facing the wrong way - more than 15 degrees off of correct heading
				if(Math.Abs(_ak.FindHeading(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_mX,_mY) - _ak.PlayerDir) > 5)
					_ak.TurnToHeading(_ak.FindHeading(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_mX,_mY));

				//Give the game process a chance at the cpu so it can respond
				System.Threading.Thread.Sleep(150);
			}
			
			if(stoprunning)
				_ak.StopRunning();
		}
		/// <summary>
		/// Get the distance from the player to this waypoint
		/// </summary>
		public float GetDistance()
		{
			return (float)_ak.ZDistance(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord,_mX,_mY,_mZ); 
		}

		/// <summary>
		/// Gets the distance from the current waypoint to the x,y,z specified
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <param name="z">z</param>
		/// <returns>Distance</returns>
		public float GetDistance( float x, float y, float z)
		{
			return (float)_ak.ZDistance(x,y,z,_mX,_mY,_mZ);
		}


	}
	#endregion
}
