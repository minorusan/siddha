using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Core.Characters.AI;
using Core.Characters.Player;
using DynamicLight2D;
using Debug = UnityEngine.Debug;
using Core.Interactivity.Combat;

namespace Core.Interactivity.AI
{
    public class GuardBrains : ArtificialIntelligence
    {
        public static event Action PlayerCaught;

        private Vector3 _startPosition;
        private float _searchTime;
        private float _health = 1f;
        private bool _inLineOfSight = false;
        private DynamicLight light2d;
        public event Action Spotted;
        public event Action RanAway;

        public float SearchDistance = 6f;
        public float ActiveDistance = 25f;
        public float AlertTime = 5f;
        public Image SuspentionBar;
        public Transform WanderingPointsRoot;
        public AudioClip AngerSound;

        [Header("Dialogue strings")]
        public string[] WanderingStrings;
        public string[] AlertStrings;
        public string[] AttackStrings;

        private SpriteRenderer _renderer;

        public SpriteRenderer Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = GetComponentInChildren<SpriteRenderer>();
                }
                return _renderer;
            }
        }

        #region ArtificialIntelligence

        protected override void InitStates()
        {
            light2d = GetComponentInChildren<DynamicLight>();
            _startPosition = transform.position;

            _availiableStates.Add(EAIState.Wandering, new AIStateWandering(this, SearchDistance, WanderingPointsRoot, SuspentionBar));
            _availiableStates.Add(EAIState.Alert, new AIStateAlert(this, SearchDistance * 2, AlertTime));
            _availiableStates.Add(EAIState.Attack, new AIStateAttack(this));
            BaseState = EAIState.Wandering;
        }

        protected override void Start()
        {
            base.Start();
            PlayerCaught += OnCaught;
            _renderer = GetComponent<SpriteRenderer>();
            if (AngerSound == null)
            {
                AngerSound = Resources.Load<AudioClip>("Sounds/moan");
            }
        }

        protected override void Update()
        {
            base.Update();
            var touch = UnityEngine.Input.GetMouseButtonDown(0);
           
#if !UNITY_EDITOR && UNITY_ANDROID
            touch = UnityEngine.Input.touches.Length >= 1;
#endif
            if (touch)
            {
                var position = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

#if !UNITY_EDITOR && UNITY_ANDROID
              position = Camera.main.ScreenToWorldPoint(UnityEngine.Input.touches[UnityEngine.Input.touches.Length - 1].position);
#endif

                if (GetComponent<Collider2D>().bounds.Contains((Vector2)position))
                {
                    ThrowController.Instance.ThrowTarget = gameObject;
                }
            }

            HandleSearching();
        }

        private void HandleSearching()
        {
            if (_searchTime > 0f)
            {
                _searchTime -= Time.deltaTime;
                if (_searchTime <= 0f)
                {
                    ChangeState();
                }
            }
        }

        #endregion

        private void OnCaught()
        {
            transform.position = _startPosition;
        }

        public void OnSpotted(GameObject go)
        {
            if (go.tag == "Player" && !PlayerQuirks.Shadowed)
            {
                _inLineOfSight = true;

                _searchTime = -1f;
                Spotted();
            }
        }

        public void OnExit(GameObject go)
        {
            if (go.tag == "Player")
            {
                _inLineOfSight = false;
                _searchTime = AlertTime;
            }
        }

        private void ChangeState()
        {
            if (!_inLineOfSight)
            {
                RanAway();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player" && PlayerQuirks.Attacked)
            {
                OnCaught();
                PlayerBehaviour.CurrentPlayer.Kill();
            }
        }
    }
}