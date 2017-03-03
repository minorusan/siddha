using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Characters.Player;
using UnityEngine.UI;


namespace Core.Gameplay.Interactivity
{
	public class QuestNPC : MonoBehaviour
	{
		private Quest _quest;
		public string QuestId = "quest.id.";

		private void Start ()
		{
			_quest = QuestStorage.GetQuestById (QuestId);
		}

		private void OnTriggerEnter2D (Collider2D trigger)
		{
			if (trigger.tag == PlayerBehaviour.kPlayerTag && trigger.isTrigger)
			{
				var satisfied = _quest.IsRequirementSatisfied (gameObject);
				if (!_quest.Completed && satisfied && QuestController.TrackedQuests.Contains(_quest))
				{
                    var dialog = GetComponent<DialogTrigger>();
                    if (dialog != null)
                    {
                        Destroy(dialog);
                    }
                   
					_quest.PerformAction (gameObject);
					_quest.Completed = true;
				}
			}
		}
	}

}
