using UnityEngine;

using Core.Characters.Player;

namespace Core.Gameplay.Interactivity
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class VendorTrigger : MonoBehaviour
    {
        private ActionBase _vendorAction;
        public string VendorID;

        private void Start()
        {
            _vendorAction = ActionsInitialiser.GetActionByID("action.id.trade");
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag && trigger.isTrigger)
            {
                ActionPerformer.Instance.SetAction(_vendorAction, gameObject);
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

