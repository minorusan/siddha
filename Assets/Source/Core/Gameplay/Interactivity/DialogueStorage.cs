using System.Collections.Generic;

using System;


namespace Core.Gameplay.Interactivity
{
	public struct DialogueStatement
	{
		public string SpeakerGOName;
		public string Sentence;
		public bool Thought;
	}

	public class Dialogue
	{
		private DialogueStatement[] _statements;
		private Action _completion;
        private readonly string _id;

		public Action Completion
		{
			get
			{
				return _completion;
			}
		}

		public DialogueStatement[] Statements
		{
			get
			{
				return (DialogueStatement[])_statements.Clone();
			}
		}

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public Dialogue(string id, DialogueStatement[] statements, Action completion)
		{
			_statements = statements;
			_completion = completion;
            _id = id;
		}
	}

	public class DialogueStorage
	{
		private static Dictionary<string, Dialogue> _dialogs = new Dictionary<string, Dialogue>();

		static DialogueStorage()
		{
            foreach (var dialogue in DialogParser.Dialogues)
            {
                _dialogs.Add(dialogue.Id, dialogue);
            }
		}

		public static Dialogue GetDialogueByID(string dialogueID)
		{
			return _dialogs[dialogueID];
		}
	}
}