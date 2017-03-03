using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Core.Utilities.UI;
using Core.Inventory;


namespace Core.Gameplay.Interactivity
{
	public delegate void QuestCompletedHandler (Quest sender);

	public class Quest
	{
		private string _questId;
		private bool _completed;
		private string _description;
		private readonly ActionRequirement _requirement;
		private readonly InteractiveAction _completion;

		public event QuestCompletedHandler QuestCompleted;

		public bool Completed
		{
			get
			{
				return _completed;
			}

			set
			{
				_completed = value;
				if (_completed && QuestCompleted != null)
				{
					QuestCompleted (this);
				}
			}
		}

		public string ID
		{
			get
			{
				return _questId;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
		}

		public void PerformAction (GameObject obj)
		{
			_completion (obj);
		}

		public bool IsRequirementSatisfied (GameObject owner)
		{
			return _requirement (owner) && QuestController.TrackedQuests.Contains(this);
		}

		public Quest (string questId, string description, ActionRequirement requirement, InteractiveAction completion)
		{
			_questId = questId;
			_description = description;
			_requirement = requirement;
			_completion = completion;
		}
	}

	public static class QuestStorage
	{
		private static Dictionary<string, Quest> _quests = new Dictionary<string, Quest> ();

		static QuestStorage ()
		{
			InitQuests ();
		}

		private static void InitQuests ()
		{
			var gettovault = new Quest ("quest.id.gettovault", "Find your way to vault", (GameObject owner) => { return true; }, (GameObject owner) => { DialogueDisplayer.ShowDialogue(DialogueStorage.GetDialogueByID("dialogue.id.vault")); });

            var getbear = new Quest("quest.id.getbear", "Find some toy for a kid", (GameObject owner) =>
            {
                return PlayerInventory.Instance.GetItems().Count(i => i.ItemID == "genericitem.id.toybear") > 0;
            },
            (GameObject obj) =>
            {
                PlayerInventory.Instance.RemoveItemFromInventory("genericitem.id.toybear");
                DialogueDisplayer.ShowDialogue(DialogueStorage.GetDialogueByID("dialogue.id.kidend"));
            });

            var getnails = new Quest("quest.id.getnails", "Find nails", (GameObject owner) =>
            {
                return PlayerInventory.Instance.GetItems().Count(i => i.ItemID == "genericitem.id.nails") > 0;
            },
            (GameObject obj) =>
            {
                PlayerInventory.Instance.RemoveItemFromInventory("genericitem.id.nails");
               
                DialogueDisplayer.ShowDialogue(DialogueStorage.GetDialogueByID("dialogue.id.scholarend"));
            });

            var getlock = new Quest("quest.id.getlock", "Get something with lock", (GameObject owner) =>
            {
                return PlayerInventory.Instance.GetItems().Count(i => i.ItemID == "genericitem.id.chain") > 0;
            },
          (GameObject obj) =>
          {
              PlayerInventory.Instance.RemoveItemFromInventory("genericitem.id.chain");
 
              DialogueDisplayer.ShowDialogue(DialogueStorage.GetDialogueByID("dialogue.id.lockpicktought"));
          });

            _quests.Add (gettovault.ID, gettovault);
            _quests.Add(getlock.ID, getlock);
            _quests.Add(getbear.ID, getbear);
            _quests.Add(getnails.ID, getnails);
        }

		public static Quest GetQuestById (string questID)
		{
			return _quests [questID];
		}

		public static List<Quest> GetQuests ()
		{
			return _quests.Values.ToList ();
		}
	}
}

