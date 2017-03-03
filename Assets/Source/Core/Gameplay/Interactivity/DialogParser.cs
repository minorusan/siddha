using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utils.SimpleJSON;

namespace Core.Gameplay.Interactivity
{
    public static class DialogParser
    {
        private static int kLevelsCount = 1;
        private static string kDialoguesPath = "Data/dialogues_level_{0}";
        private static readonly List<Dialogue> _dialogues;
        

        static DialogParser()
        {
            for (int i = 0; i < kLevelsCount; i++)
            {
                var dialoguesFile = Resources.Load<TextAsset>(string.Format(kDialoguesPath, i));
                var parsedJson = JSON.Parse(dialoguesFile.text);

                _dialogues = GetAllDialoguesFromJSON(parsedJson["dialogues"]);
            }
           
            Debug.Log("DialogueParser::Got " + _dialogues.Count + " dialogues");
        }

        public static List<Dialogue> Dialogues
        {
            get
            {
                return _dialogues;
            }
        }

        private static List<Dialogue> GetAllDialoguesFromJSON(JSONNode parsedJson)
        {
            var parsedDialogues = new List<Dialogue>();
            foreach (JSONNode dialogue in parsedJson.AsArray)
            {
                var statements = GetDialogueStatements(dialogue["statements"]);
                var newDialogue = new Dialogue(dialogue["id"].Value,statements, DialogueActionsStorage.GetDialogueCompletion(dialogue["id"].Value));
                parsedDialogues.Add(newDialogue);
            }

            return parsedDialogues;
        }

        private static DialogueStatement[] GetDialogueStatements(JSONNode dialogue)
        {
            var parsedStatements = new List<DialogueStatement>();

            foreach (JSONNode statement in dialogue.AsArray)
            {
                var newStatement = new DialogueStatement();
                newStatement.SpeakerGOName = statement["speakerName"].Value;
                newStatement.Thought = statement["isthought"].AsBool;
                newStatement.Sentence = statement["sentence"].Value;

                parsedStatements.Add(newStatement);
            }

            return parsedStatements.ToArray();
        }
    }
}

