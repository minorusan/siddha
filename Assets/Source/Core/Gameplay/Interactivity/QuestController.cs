using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.UI;


namespace Core.Gameplay.Interactivity
{
	public class QuestController : MonoBehaviour
	{
		private static QuestController _instance;
		private List<Quest> _activeQuests = new List<Quest> ();
		public AudioClip QuestAccepted;

		#region Monobehaviour

		void OnEnable ()
		{
			_instance = this;
			var quests = QuestStorage.GetQuests ();
			foreach (var quest in quests)
			{
				quest.QuestCompleted += QuestCompleted;
			}
		}

		void OnDisable ()
		{
			_instance = null;
		}

		#endregion

		public static List<Quest> TrackedQuests
		{
			get
			{
				return _instance._activeQuests;
			}
		}

		public static void StartQuest (string questID)
		{
			ShowUI (questID);
		}

		private static void ShowUI (string id)
		{
			var quest = QuestStorage.GetQuestById (id);
		    if (!_instance._activeQuests.Contains(quest))
		    {
                _instance._activeQuests.Add(quest);
                FanfareMessage.ShowWithText(string.Format("Quest accepted:{0}", quest.Description));
                AudioSource.PlayClipAtPoint(_instance.QuestAccepted, Camera.main.transform.position, 1f);
            }
		}

		private void QuestCompleted (Quest quest)
		{
			quest.QuestCompleted -= QuestCompleted;
			_instance._activeQuests.Remove (quest);
			FanfareMessage.ShowWithText (string.Format ("Quest completed:{0}", quest.Description));
			AudioSource.PlayClipAtPoint (_instance.QuestAccepted, Camera.main.transform.position, 1f);
		}
	}
}

