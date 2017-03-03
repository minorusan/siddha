using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities.UI;
using Core.Gameplay.Interactivity;


namespace Core.Interactivity
{
	public enum ETutorialId
	{
		HungerItemPicked,
		StressItemPicked,
		GenericItemPicked,
		CraftItemPicked,
		LockedDoor,
		CanHide
	}

	public static class Tutorial
	{
		private static Dictionary<ETutorialId, string> _tutorials = new Dictionary<ETutorialId, string>();

		static Tutorial()
		{
			

			_tutorials.Add(ETutorialId.CanHide, "dialogue.id.tutorial.canhide");
			_tutorials.Add(ETutorialId.CraftItemPicked, "dialogue.id.tutorial.craft");
			_tutorials.Add(ETutorialId.LockedDoor, "dialogue.id.tutorial.nippers");
			_tutorials.Add(ETutorialId.HungerItemPicked, "dialogue.id.tutorial.hunger");
			_tutorials.Add(ETutorialId.StressItemPicked, "dialogue.id.tutorial.stress");
			_tutorials.Add(ETutorialId.GenericItemPicked, "dialogue.id.tutorial.generic");
		}

		public static void ShowForIDIfNeeded(ETutorialId id)
		{
			if(PlayerPrefs.GetInt(id.ToString()) <= 0)
			{
				DialogueDisplayer.ShowDialogue(DialogueStorage.GetDialogueByID(_tutorials[id]));
				PlayerPrefs.SetInt(id.ToString(), 2);
			}
		}
	}
}

