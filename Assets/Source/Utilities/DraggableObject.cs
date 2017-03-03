using UnityEngine;
using Core.Characters.Player;

namespace Core.Gameplay.Interactivity
{
    public enum EDragState
    {
        Dragging,
        Idle
    }

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class DraggableObject : MonoBehaviour
    {
        #region PRIVATE

        private ActionBase _dragActionBase;
        private ActionBase _undragActionBase;
        private Rigidbody2D _selfRigidbody2D;
        private HingeJoint2D _joint;
        public EDragState State = EDragState.Idle;

        #endregion

        public Rigidbody2D SelfRigidbody2D
        {
            get
            {
                return _selfRigidbody2D;
            }
        }

        public HingeJoint2D Joint
        {
            get { return _joint; }
        }

        void Start()
        {
            _joint = GetComponent<HingeJoint2D>();
            _selfRigidbody2D = GetComponent<Rigidbody2D>();
            _selfRigidbody2D.isKinematic = true;
            _dragActionBase = ActionsInitialiser.GetActionByID("action.id.drag");
            _undragActionBase = ActionsInitialiser.GetActionByID("action.id.undrag");
        }

        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag && trigger.isTrigger)
            {
                ActionPerformer.Instance.SetAction(State == EDragState.Idle ? _dragActionBase : _undragActionBase, gameObject);
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

