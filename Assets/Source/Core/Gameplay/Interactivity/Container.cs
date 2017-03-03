using Core.Characters.Player;
using Core.Gameplay.Interactivity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay.Interactivity
{
    [RequireComponent(typeof(Collider2D))]
    public class Container : MonoBehaviour
    {
        public string[] Items;

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag && trigger.isTrigger)
            {
                ActionPerformer.Instance.SetAction(ActionsInitialiser.GetActionByID("action.id.container"), gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag)
            {
                ActionPerformer.Instance.SetAction(null);
            }
        }
    }
}

