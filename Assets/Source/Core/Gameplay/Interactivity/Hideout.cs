using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Characters.Player;
using Core.Interactivity;


namespace Core.Gameplay.Interactivity
{
	[RequireComponent(typeof(Collider2D))]
	public class Hideout : MonoBehaviour
	{
		public const string kHideAction = "action.id.hide";
		private ActionBase _action;

		public ActionBase Action
		{
			get
			{
				return _action;
			}
		}

		void Start()
		{
			_action = ActionsInitialiser.GetActionByID(kHideAction);
		}

		private void OnTriggerEnter2D(Collider2D trigger)
		{
			if(trigger.tag == PlayerBehaviour.kPlayerTag)
			{
				ActionPerformer.Instance.SetAction(_action, gameObject);
			}
		}

		private void OnTriggerExit2D(Collider2D trigger)
		{
			if(trigger.tag == PlayerBehaviour.kPlayerTag)
			{
				ActionPerformer.Instance.SetAction(null);
				PlayerBehaviour.CurrentPlayer.Renderer.enabled = true;
				PlayerQuirks.Hidden = false;
			}
		}
	}
}

