using UnityEngine;

using System.Linq;
using System.Collections;
using Core.Map;
using Core.Map.Pathfinding;


namespace Core.Map
{

    public class MovableObject : MapDependentObject
    {
        #region PRIVATE

        private Path _currentPath = new Path();
        private EMovableObjectState _currentState = EMovableObjectState.Standing;
        private AudioSource _myScource;
        private AnimateSpriteSheet _animator;

        #endregion

        public Color DebugColor;
        public float MovementSpeed;
        public AudioClip StepSound;

        #region Properties

        public bool ReachedDestination
        {
            get
            {
                return _currentPath.Empty;
            }
        }

        public Path CurrentPath
        {
            get
            {
                return _currentPath;
            }

        }

        public int CurrentPathLength
        {
            get
            {
                return _currentPath.Nodes.Count;
            }

        }

        public AnimateSpriteSheet SelfAnimator
        {
            get
            {
                return _animator;
            }
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _animator = GetComponentInChildren<AnimateSpriteSheet>();
            _myScource = gameObject.AddComponent<AudioSource>();
        }

        private void LateUpdate()
        {
            if (!_currentPath.Empty)
            {
                if (_currentPath.Nodes[0].CurrentCellType == ECellType.Busy)
                {
                    BeginMovementByPath(Pathfinder.FindPathToDestination(_map, CurrentNode.Position, _currentPath.Nodes.Last().Position));
                    if (!_currentPath.Empty)
                    {
                        MoveToTarget(_currentPath.Nodes[0].Position);
                    }
                    else
                    {
                        ToggleAnimationState(EMovableObjectState.Standing);
                    }
                }
                else
                {
                    MoveToTarget(_currentPath.Nodes[0].Position);
                }

                DrawDebugPath();
            }
        }

        private void OnDisable()
        {
            _currentPath.Nodes.Clear();
            _animator.enabled = false;
        }

        private void OnEnable()
        {
            _animator.enabled = true;
            StartCoroutine(Stepping());
        }

        #endregion

        public void BeginMovementByPath(Path path)
        {
            _currentPath.Nodes.Clear();

            _currentPath = path;
            ToggleAnimationState(EMovableObjectState.Walking);
        }

        private IEnumerator Stepping()
        {
            while (true)
            {
                _myScource.PlayOneShot(StepSound, 0.05f);
                var speed = (1f - MovementSpeed * 10);
                yield return new WaitForSeconds(speed);
            }
        }

        public void Step()
        {
            //_myScource.PlayOneShot(StepSound, 0.6f);
        }

        #region Internal

        private void MoveToTarget(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, MovementSpeed);
            CheckIfDestinationIsReached();
        }

        private void CheckIfDestinationIsReached()
        {
            if (Vector3.Distance(_currentPath.Nodes[0].Position, this.transform.position) < 0.1f)
            {
                _myPosition = _currentPath.Nodes[0];
                _currentPath.Nodes.Remove(_currentPath.Nodes[0]);
            }

            if (_currentPath.Empty)
            {
                ToggleAnimationState(EMovableObjectState.Standing);
            }
        }

        private void DrawDebugPath()
        {
            if (_currentPath.Empty)
            {
                return;
            }

            var startDraw = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            var endFirst = new Vector3(_currentPath.Nodes[0].Position.x, _currentPath.Nodes[0].Position.y, _currentPath.Nodes[0].Position.z - 1);
            Debug.DrawLine(startDraw, endFirst, DebugColor);
            for (int i = 0; i < _currentPath.Nodes.Count - 1; i++)
            {
                var start = new Vector3(_currentPath.Nodes[i].Position.x, _currentPath.Nodes[i].Position.y, _currentPath.Nodes[i].Position.z - 1);
                var end = new Vector3(_currentPath.Nodes[i + 1].Position.x, _currentPath.Nodes[i + 1].Position.y, _currentPath.Nodes[i + 1].Position.z - 1);

                Debug.DrawLine(start, end, DebugColor);
            }
        }

        protected virtual void ToggleAnimationState(EMovableObjectState state)
        {
            switch (state)
            {
                case EMovableObjectState.Standing:
                    {
                        SelfAnimator.enabled = false;
                        _currentState = EMovableObjectState.Standing;
                        break;
                    }
                case EMovableObjectState.Walking:
                    {
                        SelfAnimator.enabled = true;
                        _currentState = EMovableObjectState.Walking;
                        break;
                    }
                default:
                    break;
            }
        }

        #endregion
    }
}