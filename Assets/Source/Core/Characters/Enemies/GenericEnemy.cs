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

     
    }
}

