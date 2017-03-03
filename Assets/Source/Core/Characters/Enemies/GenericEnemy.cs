using UnityEngine;
using System.Collections;
using Core.Map;
using Core.Map.Pathfinding;

namespace Core.Characters.Enemies
{
    [RequireComponent(typeof(MovableObject))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class GenericEnemy : MonoBehaviour
    {
        private MovableObject _movableObject;
        private SpriteRenderer _spriteRenderer;

        public GameObject Target;
        public MapController Map;
        // Use this for initialization
        void Start()
        {
            _movableObject = GetComponent<MovableObject>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            _spriteRenderer.sortingOrder = transform.position.y < Target.transform.position.y ? 1 : -1;
            var node = MapController.GetNodeByPosition(Target.transform.position);
            var playerNode = _movableObject.CurrentNode;
            if (node != null && playerNode != null)
            {
                _movableObject.BeginMovementByPath(Pathfinder.FindPathToDestination(Map, playerNode.Position,
                        node.Position)); 
            }
        }
    }
}

