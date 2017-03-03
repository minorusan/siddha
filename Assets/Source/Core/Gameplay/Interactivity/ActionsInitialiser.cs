using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Core.Inventory;
using Core.Utilities;
using Core.Characters.Player;
using Core.Utilities.UI;
using System;
using UI;
using Core.Map;
using Utils;

namespace Core.Gameplay.Interactivity
{
	public static class ActionsInitialiser
	{
		private static Dictionary<string, ActionBase> _actions = new Dictionary<string, ActionBase>();

		public static ActionBase GetActionByID(string actionID)
		{
			return _actions[actionID];
		}

		#region Configuration

		static ActionsInitialiser()
		{
			InitialiseActions();
		}

		private static void InitialiseActions()
		{
			InitOpenDoorAction();
			InitHideAction();
			InitDialogueAction();
            InitVendorAction();
            InitContainerAction();
            InitDragAction();
		}

	    private static void InitDragAction()
	    {           
            var undrag = new ActionBase("action.id.undrag", (GameObject obj) => { return true; },
                (GameObject obj) => { RopeDragController.Unbind(obj);
                    PlayerQuirks.Drags = false;
                });

            ActionRequirement openActionRequirement = (GameObject owner) =>
            {
                return PlayerInventory.Instance.GetItems().Any(i => i.ItemID == "genericitem.id.rope") && !PlayerQuirks.Drags;
            };
            InteractiveAction action = (GameObject obj) =>
            {
                ProcessBarController.StartProcessWithCompletion(3f * PlayerQuirks.GetSkill(EPlayerSkills.Hiding), Resources.Load<Sprite>("Sprites/Actions/action.id.drag"), () =>
                {

                   RopeDragController.Bind(obj);
                }, Color.yellow);
            };

            var drag = new ActionBase("action.id.drag", openActionRequirement, action);
            _actions.Add(drag.ActionID, drag);
            _actions.Add(undrag.ActionID, undrag);
        }

        private static void InitVendorAction()
        {

            InteractiveAction action = (GameObject obj) =>
            {
                var trigger = obj.GetComponent<VendorTrigger>();

                var vendor = VendorsStorage.GetVendor(trigger.VendorID);
                TradeController.ShowTradeForVendor(vendor.Vendorid);
            };
            

            var tradeAction = new ActionBase("action.id.trade", (GameObject owner)=> { return true; }, action);
            _actions.Add("action.id.trade", tradeAction);
        }

        private static void InitContainerAction()
        {

            InteractiveAction action = (GameObject obj) =>
            {
                ProcessBarController.StartProcessWithCompletion(3f * PlayerQuirks.GetSkill(EPlayerSkills.Scavanging),
                    Resources.Load<Sprite>("Sprites/Actions/action.id.container"), () =>
                                                                 {
                                                                     var container = obj.GetComponent<Container>();
                                                                     ContainerUI.ShowForContainer(container);
                                                                 }, Color.green);
                
            };

            var containerAction = new ActionBase("action.id.container", (GameObject owner) => { return true; }, action);
            _actions.Add("action.id.container", containerAction);
        }

        private static void InitDialogueAction()
		{
			InteractiveAction action = (GameObject obj) =>
			{
				var trigger = obj.GetComponent <DialogTrigger>();

				var dialogue = DialogueStorage.GetDialogueByID(trigger.DialogueID);
				DialogueDisplayer.ShowDialogue(dialogue);
			};

			var dialogueAction = new ActionBase("action.id.dialogue", (GameObject owner) => { return true; }, action);
			_actions.Add("action.id.dialogue", dialogueAction);
		}

		private static void InitHideAction()
		{
			ActionRequirement openActionRequirement = (GameObject owner) =>
			{
				return !PlayerQuirks.Attacked && PlayerQuirks.GetSkill(EPlayerSkills.Hiding) > 0;
			};

			InteractiveAction action = (GameObject obj) =>
			{
				var doorway = obj.GetComponent <Hideout>();

				ProcessBarController.StartProcessWithCompletion(3f * PlayerQuirks.GetSkill(EPlayerSkills.Hiding), doorway.Action.ActionImage, () =>
				{
                    
					PlayerBehaviour.CurrentPlayer.Renderer.enabled = false;
					PlayerQuirks.Hidden = true;
				}, Color.grey);
			};

			var openDoorAction = new ActionBase(Hideout.kHideAction, openActionRequirement, action);
			_actions.Add(Hideout.kHideAction, openDoorAction);
		}

		private static void InitOpenDoorAction()
		{
			ActionRequirement openActionRequirement = (GameObject owner) =>
			{
				var doorway = owner.GetComponent <LockedDoorway>();

				if(PlayerInventory.Instance.GetItems().Any(item => item.ItemID == doorway.ItemToUnlock))
				{
					return true;
				}
				return false;
			};

			InteractiveAction action = (GameObject obj) =>
			{
				var doorway = obj.GetComponent <LockedDoorway>();
				PlayerInventory.Instance.RemoveItemFromInventory(doorway.ItemToUnlock);
				ProcessBarController.StartProcessWithCompletion(3f, doorway.Action.ActionImage, () =>
				{
				    doorway.gameObject.SetActive(false);
				},Color.yellow);
			};

			var openDoorAction = new ActionBase(LockedDoorway.kOpenDoorActionId, openActionRequirement, action);
			_actions.Add(LockedDoorway.kOpenDoorActionId, openDoorAction);
		}

		#endregion
	}
}

