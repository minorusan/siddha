using Core.Characters.Player;
using UnityEngine;

namespace Core.Gameplay.Interactivity
{
    public class RopeDragController : MonoBehaviour
    {
        private static RopeDragController _instance;
        private Rigidbody2D _lastJoint;
        private DraggableObject _current;
        private HingeJoint2D _firstJoint2D;

        private void Awake()
        {
            _instance = this;
            _firstJoint2D = transform.GetChild(0).GetComponent<HingeJoint2D>();
            _lastJoint = transform.GetChild(transform.childCount-1).GetComponent<Rigidbody2D>();
            gameObject.SetActive(false);
        }

        public static void Bind(GameObject obj)
        {
            var draggableObject = obj.GetComponent<DraggableObject>();
            _instance._current = draggableObject;
            draggableObject.State = EDragState.Dragging;

            PlayerBehaviour.CurrentPlayer.GetComponent<Rigidbody2D>().isKinematic = true;
            _instance.transform.position = PlayerBehaviour.CurrentPlayer.transform.position;
            _instance.gameObject.SetActive(true);
            _instance._firstJoint2D.connectedAnchor = Vector2.zero;

            draggableObject.Joint.connectedBody = _instance._lastJoint;
            draggableObject.Joint.connectedAnchor = Vector2.zero;
          _instance.Invoke("UpdateBodies", 2f);
            
        }

        private void UpdateBodies()
        {
            PlayerQuirks.Drags = true;
            PlayerBehaviour.CurrentPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
            _current.SelfRigidbody2D.isKinematic = false;
        }

        public static void Unbind(GameObject obj)
        {
            obj.GetComponent<DraggableObject>().State = EDragState.Idle;
            obj.GetComponent<Rigidbody2D>().isKinematic = true;
            _instance.gameObject.SetActive(false);
            obj.GetComponent<HingeJoint2D>().connectedBody = null;
        }
    }
}

