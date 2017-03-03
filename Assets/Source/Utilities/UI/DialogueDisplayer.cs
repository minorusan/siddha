using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Gameplay.Interactivity;
using System;

namespace Core.Utilities.UI
{
    public class DialogueDisplayer : MonoBehaviour
    {
        private enum EDialogueDisplayerState
        {
            Idle,
            Showing
        }

        public static event Action<string> StartedDialogue;

        private static DialogueDisplayer _current;
        private Dialogue _dialogue;
        private int _currentMessage;
        private EDialogueDisplayerState _state;

        public DialogueBubble Bubble;

        private void Start()
        {
            _current = FindObjectOfType<DialogueDisplayer>();
            Bubble.BubbleCompleted += EndDialog;
        }

        private void Update()
        {
            if (_state == EDialogueDisplayerState.Showing)
            {
                if (Bubble.Ready)
                {
                    Bubble.ShowMessage(_dialogue.Statements[_currentMessage].Sentence,
                                        _dialogue.Statements[_currentMessage].SpeakerGOName,
                                        _dialogue.Statements[_currentMessage].Thought);
                    _currentMessage++;
                }
            }
        }

        private void EndDialog()
        {
            if ( _dialogue != null && _currentMessage >= _dialogue.Statements.Length)
            {
              
                _state = EDialogueDisplayerState.Idle;
                _dialogue = null;
            }
        }

        public static void ForceCancel()
        {
            if (_current._dialogue != null)
            {
                QuitDialog();
                const string forceenddialogues = "dialogue.id.forceend{0}";

               // ShowDialogue(DialogueStorage.GetDialogueByID(string.Format(forceenddialogues, UnityEngine.Random.Range(0, 3))));
            }
        }

        private static void QuitDialog()
        {
            _current.Bubble.ForceQuit();
            _current._currentMessage = 0;
            _current._dialogue = null;
            _current.Bubble.StopAllCoroutines();
            _current.Bubble.gameObject.SetActive(false);
            _current._state = EDialogueDisplayerState.Idle;
        }

        public static void ShowDialogue(Dialogue dialogue)
        {
            if (dialogue != _current._dialogue)
            {
                _current._currentMessage = 0;
                _current._dialogue = dialogue;
                _current._state = EDialogueDisplayerState.Showing;

                if (dialogue.Completion != null)
                {
                    dialogue.Completion();
                }

                StartedDialogue(dialogue.Id);
            }
        }
    }
}

