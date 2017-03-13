using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Core.Characters.AI;
using Core.Characters.Player;
using DynamicLight2D;
using Debug = UnityEngine.Debug;


namespace Core.Interactivity.AI
{
	public class GuardBrains:ArtificialIntelligence
	{
        public static event Action PlayerCaught;
        
        private Vector3 _startPosition;
	    private float _searchTime;
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
				if(_renderer == null)
				{
					_renderer = GetComponentInChildren<SpriteRenderer>();
				}
				return _renderer;
			}
		}

		private void OnDrawGizmos()
		{
			//var color = Color.white;
			//var radius = SearchDistance;
			//#if !UNITY_EDITOR
   //         if (_currentState != null && _currentState.State == EAIState.Wandering)
   //         {
   //         color = Color.green;
   //         }
   //         else
   //         {
   //         color = Color.yellow;
   //         radius = SearchDistance * 2f;
   //         }
			//#endif
            
			//Gizmos.color = color;
			//Gizmos.DrawWireSphere(transform.position, radius);
   //         Gizmos.color = Color.red;
   //         Gizmos.DrawWireSphere(transform.position, ActiveDistance);
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
			_renderer = GetComponent <SpriteRenderer>();
			if(AngerSound == null)
			{
				AngerSound = Resources.Load<AudioClip>("Sounds/moan");
			}
		}

        private void OnCaught()
        {
            transform.position = _startPosition;
        }

        protected override void Update()
		{
			base.Update();
		    if (_searchTime > 0f)
		    {
		        _searchTime -= Time.deltaTime;
		        if (_searchTime <= 0f)
		        {
		            ChangeState();
		        }
		    }
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

        #endregion
    }
}

