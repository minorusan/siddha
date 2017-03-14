using System;
using UnityEngine;
using UnityEngine.UI;

using Core.Map;
using Core.Characters.Player;
using Core.Interactivity.AI;
using DynamicLight2D;
using Utils;
using Random = UnityEngine.Random;


namespace Core.Characters.AI
{
	public class AIStateWandering:AIStateBase
	{
	    private GuardBrains _guardBrains;
		private float _previousMoveSpeed;
		private float _searchDistance;
		private float _suspention;
		private Image _suspentionBar;
		private Core.Characters.Player.PlayerBehaviour _player;
		private NoiseEffect _effect;
		private AudioClip _whispering;
		private AudioClip _bellCreepy;
		private SequentialMovement _movementController;
	  


		public AIStateWandering(ArtificialIntelligence brains, float searchDistance, Transform pathRoot, Image suspentionBar) : base(brains)
		{
			_searchDistance = searchDistance;
			State = EAIState.Wandering;

			_movementController = new SequentialMovement(pathRoot.GetComponentsInChildren <Transform>(),
			                                             _masterBrain.MovableObject,
			                                             true);

			_bellCreepy = Resources.Load <AudioClip>("Sounds/bellCreepy");
			_effect = GameObject.FindObjectOfType<NoiseEffect>();
			_player = GameObject.FindObjectOfType<PlayerBehaviour>();
			_suspentionBar = suspentionBar;

		    _guardBrains = (GuardBrains) brains;
            _guardBrains.Spotted += GuardBrainsOnSpotted;
		}

	    private void GuardBrainsOnSpotted()
	    {
            _currentCondition = AIStateCondition.Done;
            _pendingState = EAIState.Attack;
        }

	    public override void OnEnter()
		{
			base.OnEnter();
           
            if (_player != null && _player.isActiveAndEnabled)
			{
				var playerNode = MapController.GetNodeByPosition(_player.transform.position);
				_suspention = _suspention > 0f && playerNode != null ? 0.9f : 0f;

			   
				_previousMoveSpeed = _masterBrain.MovableObject.MovementSpeed;
				_masterBrain.MovableObject.MovementSpeed *= 0.4f;

				_masterBrain.MovableObject.DebugColor = Color.green;
			}
		}

		public override void OnLeave()
		{
			_masterBrain.MovableObject.MovementSpeed = _previousMoveSpeed;
		}

		public override void UpdateState()
		{
			base.UpdateState();
			CheckLeaveStateConditions();
            if (Random.Range(0, 500) == 1)
            {
                _masterBrain.StatusText.text = _guardBrains.WanderingStrings[Random.Range(0, _guardBrains.WanderingStrings.Length)];
            }
			_movementController.UpdateMovement();
			_suspention -= _suspention > 0f ? 0.01f : 0f;
		}

		private void CheckNoise()
		{
			var noise = _player.Noise;

			_suspention += noise;
			_suspentionBar.fillAmount = _suspention;
		}

		private void CheckLeaveStateConditions()
		{
			var playerNode = MapController.GetNodeByPosition(_player.transform.position);
            var close = Vector3.Distance(_masterBrain.transform.position, _player.transform.position) < _searchDistance;

            if (_suspention > 1f || (close && !PlayerQuirks.StoppedBreathing))
			{
				_currentCondition = AIStateCondition.Done;
				_pendingState = EAIState.Alert;
			}

			if(playerNode != null)
			{
				CheckNoise();
			}
		}
	}
}

