using System;
using System.Collections.Generic;
using Core.Characters.Player;
using Core.Inventory;
using UnityEngine;

namespace Core.Gameplay.Interactivity
{
    public static class DialogueActionsStorage
    {
        private static Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        static DialogueActionsStorage()
        {
            InitDialogueActions();
        }

        private static void InitDialogueActions()
        {
            
            _actions.Add("dialogue.id.childgroup_one", () =>
            {
                GameObject.Find("ChildrenPolilogue").GetComponentInChildren<DialogTrigger>().AutoStart = false;
                QuestController.StartQuest("quest.id.gettovault");
                SwapNPCPosition("ChildrenPolilogue", "ChildrenPolilogueFinished");
            });

            _actions.Add("dialogue.id.genny", () =>
            {
                SwapNPCPosition("GennyTalk", "GennyTrade");
            });

            _actions.Add("dialogue.id.kidstart", () =>
            {
                QuestController.StartQuest("quest.id.getbear");
            });

            _actions.Add("dialogue.id.kidend", () =>
            {
                SwapNPCPosition("ScholarMad", "Scholar");
            });

            _actions.Add("dialogue.id.scholar", () =>
            {
                QuestController.StartQuest("quest.id.getnails");
            });

            _actions.Add("dialogue.id.lockpickteach", () =>
            {
                SwapNPCPosition("GennyStart", "GennyTalk");
                QuestController.StartQuest("quest.id.getlock");
            });

            _actions.Add("dialogue.id.scholarend", () =>
            {
                PlayerInventory.Instance.TryAddItemToInventory(ItemsData.GetItemById("genericitem.id.lockpick"));
                //var girlposition = GameObject.Find("GirlPosition");
                //var girl = GameObject.Find("LostGirl");

                SwapNPCPosition("Picker", "PickerTeacher");

                //girl.transform.position = girlposition.transform.position;
            });

            _actions.Add("dialogue.id.madscholar", () =>
            {
                SwapNPCPosition("KidBusy", "Kid");
            });

            _actions.Add("dialogue.id.lockpicktought", () =>
            {
                PlayerQuirks.ModifySkill(EPlayerSkills.Lockpicking, 20);
                
            });
        }

        public static Action GetDialogueCompletion(string dialogueId)
        {
            Action ret;
            _actions.TryGetValue(dialogueId, out ret);
            return ret;
        }

        private static void SwapNPCPosition(string npc, string npcToSwap)
        {
            var npc1 = GameObject.Find(npc);
            var npc2 = GameObject.Find(npcToSwap);
            npc2.transform.position = npc1.transform.position;
            npc1.SetActive(false);
        }
    }
}