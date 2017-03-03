using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Gameplay.Interactivity;
using UnityEngine.UI;


namespace Utilities.UI
{
	public class QuestLog : MonoBehaviour
	{
		private List<Quest> _quests;
		public GameObject _questDescription;
		// Use this for initialization
		private void OnEnable ()
		{
			for (int i = 1; i < transform.childCount; i++)
			{
				Destroy (transform.GetChild (i).gameObject);
			}

			_quests = QuestController.TrackedQuests;
			foreach (var quest in _quests)
			{
				var newDescription = Instantiate (_questDescription.gameObject, transform);
				newDescription.GetComponent <Text> ().text = quest.Description;
				newDescription.SetActive (true);
			}
		}

	}
}
