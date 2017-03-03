using UnityEngine;
using System.Collections;
using System;
using Core.Characters.Player;
using Core.Utilities.UI;
using Core.Interactivity;
using Core.Inventory;


namespace Core.Gameplay.Interactivity
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class LockedDoorway : MonoBehaviour
	{
		public const string kOpenDoorActionId = "action.id.opendoor";
		public string ItemToUnlock;
		private ActionBase _action;

		public ActionBase Action
		{
			get
			{
				return _action;
			}
		}

		// Use this for initialization
		void Start()
		{
			_action = ActionsInitialiser.GetActionByID(kOpenDoorActionId);
		}

		private void OnTriggerEnter2D(Collider2D trigger)
		{
			if(trigger.tag == PlayerBehaviour.kPlayerTag)
			{
				ActionPerformer.Instance.SetAction(_action, gameObject, InventoryImagesLoader.GetImageForItem(ItemsData.GetItemById(ItemToUnlock).EItemType, ItemToUnlock));
			}
		}

		private void OnDisable()
		{
			var performer = ActionPerformer.Instance;
			if(performer != null)//To avoid error on level init or chunk regeneration
			{
				ActionPerformer.Instance.SetAction(null);
			}
		}

		private void OnTriggerExit2D(Collider2D trigger)
		{
			if(trigger.tag == PlayerBehaviour.kPlayerTag)
			{
				ActionPerformer.Instance.SetAction(null);
			}
		}
	}
}

