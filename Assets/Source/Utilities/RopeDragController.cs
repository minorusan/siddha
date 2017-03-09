using Core.Characters.Player;
using System.Collections;
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
            _firstJoint2D = transform.GetChild(transform.childCount - 1).GetComponent<HingeJoint2D>();
            _lastJoint = transform.GetChild(0).GetComponent<Rigidbody2D>();
            gameObject.SetActive(false);
        }

        public static void Bind(GameObject obj)
        {
            var draggableObject = obj.GetComponent<DraggableObject>();
            _instance._current = draggableObject;
            draggableObject.State = EDragState.Dragging;

            //PlayerBehaviour.CurrentPlayer.GetComponent<Rigidbody2D>().isKinematic = true;
            _instance.transform.position = new Vector3(PlayerBehaviour.CurrentPlayer.transform.position.x,
                                                        PlayerBehaviour.CurrentPlayer.transform.position.y,
                                                        -1f);
            
            _instance.gameObject.SetActive(true);
            _instance._firstJoint2D.connectedAnchor = Vector2.zero;

            draggableObject.Joint.connectedBody = _instance._lastJoint;
            draggableObject.Joint.connectedAnchor = Vector2.zero;
            draggableObject.Joint.enabled = false;
            _instance.StartCoroutine(_instance.UpdateBodies(draggableObject));
        }

        private IEnumerator UpdateBodies(DraggableObject draggableObject)
        {
            yield return new WaitForSeconds(0.5f);

            draggableObject.DistanceJoint.connectedBody = _instance._lastJoint;
            draggableObject.DistanceJoint.connectedAnchor = Vector2.zero;
            PlayerQuirks.Drags = true;
            PlayerBehaviour.CurrentPlayer.GetComponent<HingeJoint2D>().enabled = true;
            
            StartCoroutine(TurnOnJoint());
        }

        private IEnumerator TurnOnJoint()
        {
            yield return new WaitForSeconds(2f);
            _current.Joint.enabled = true;
            _current.SelfRigidbody2D.isKinematic = false;
        }

        public static void Unbind(GameObject obj)
        {
            obj.GetComponent<DraggableObject>().State = EDragState.Idle;
            obj.GetComponent<Rigidbody2D>().isKinematic = true;
            PlayerBehaviour.CurrentPlayer.GetComponent<HingeJoint2D>().enabled = false;
            _instance.gameObject.SetActive(false);
            obj.GetComponent<HingeJoint2D>().connectedBody = null;
            
        }
    }
}

