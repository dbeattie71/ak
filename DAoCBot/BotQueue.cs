//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Collections;

namespace DAoC_Bot
{
	/// <summary>
	/// This implements a queue for bot actions, unlike the Queue object that's
	/// already present in .NET, this will allow you to add actions at the start
	/// of the queue. Its also strong-typed.
	/// </summary>
	public class BotQueue
	{
		private ArrayList _actions;

		public BotQueue()
		{
			_actions = new ArrayList();
		}

		public void Enqueue( BotAction action)
		{
			_actions.Add( action);
		}

		public BotAction Dequeue()
		{
			if( _actions.Count == 0)
				throw new Exception( "Can't dequeue botaction because the queue is empty.");

			BotAction action = (BotAction) _actions[0];
			_actions.RemoveAt(0);

			return action;
		}

		public BotAction Peek(int index)
		{
			if( index > _actions.Count - 1)
				throw new Exception( "Can't Peek because the index is greater than the number of actions in the queue.");

			BotAction action = (BotAction) _actions[index];

			return action;
		}

		public void Insert( int index, BotAction action)
		{
			_actions.Insert( index, action);
		}

		public void Clear()
		{
			_actions.Clear();
		}

		public int Count
		{
			get
			{
				return _actions.Count;
			}
		}
	}
}
