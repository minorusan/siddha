using UnityEngine;
using Core.Characters.Player;
using Core.Utilities.UI;


namespace Core.Gameplay.Interactivity
{
	[RequireComponent(typeof(Collider2D))]
	public class DialogTrigger : MonoBehaviour
	{
		private ActionBase _dialogueAction;
        private bool _showsDialogue;

		public string DialogueID = "dialogue.id.";
		public bool AutoStart = false;

		void Start()
		{
			_dialogueAction = ActionsInitialiser.GetActionByID("action.id.dialogue");
            DialogueDisplayer.StartedDialogue += OnDialogueStarted;
		}

        private void OnDialogueStarted(string obj)
        {
            if (obj==DialogueID)
            {
                _showsDialogue = true;
            }
        }

        private void Update()
        {
            if (_showsDialogue && Vector3.Distance(PlayerBehaviour.CurrentPlayer.transform.position, transform.position) > 10)
            {
                _showsDialogue = false;
                DialogueDisplayer.ForceCancel();
            }
        }

        private void OnTriggerEnter2D(Collider2D trigger)
		{
			if(AutoStart)
			{
			    AutoStart = false;
				DialogueDisplayer.ShowDialogue(DialogueStorage.GetDialogueByID(DialogueID));
				return;
			}

			if(trigger.tag == PlayerBehaviour.kPlayerTag && trigger.isTrigger)
			{
				ActionPerformer.Instance.SetAction(_dialogueAction, gameObject);
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

