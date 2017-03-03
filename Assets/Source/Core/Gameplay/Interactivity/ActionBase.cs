using UnityEngine;
using System.Collections;
using System;


namespace Core.Gameplay.Interactivity
{
	public delegate bool ActionRequirement (GameObject owner);
	public delegate void InteractiveAction (GameObject obj);
	public class ActionBase
	{
		#region Private

		private const string kActionSpritesPath = "Sprites/Actions/";
		private readonly string _id;
		private readonly ActionRequirement _requirement;
		private readonly InteractiveAction _action;
		private readonly Sprite _actionImage;

		#endregion

		public Sprite ActionImage
		{
			get
			{
				return _actionImage;
			}
		}

		public string ActionID
		{
			get
			{
				return _id;
			}
		}

		public ActionBase (string actionID, ActionRequirement requirement, InteractiveAction action)
		{
			_id = actionID;
			_requirement = requirement;
			_action = action;
			_actionImage = Resources.Load <Sprite> (kActionSpritesPath + actionID);
		}

		public void PerformAction (GameObject obj)
		{
			_action (obj);
		}

		public bool IsRequirementSatisfied (GameObject owner)
		{
			return _requirement (owner);
		}
	}
}

