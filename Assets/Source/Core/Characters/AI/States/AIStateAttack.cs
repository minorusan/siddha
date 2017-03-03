using System;
using System.Linq;
using UnityEngine;

using Core.Map;
using Core.Characters.Player;
using Core.Map.Pathfinding;
using Core.Characters.Player.Demand;
using Core.Interactivity.AI;
using Random = UnityEngine.Random;


namespace Core.Characters.AI
{
	public class AIStateAttack:AIStateBase
	{
		private Node _currentDestination;
		private Node _previousDestination;
        private Vector3 _startPosition;
		private Core.Characters.Player.PlayerBehaviour _player;
	    private GuardBrains _guardBrains;
		private bool _attacks;
		private AudioClip _sound;


		public AIStateAttack(ArtificialIntelligence brains) : base(brains)
		{
			State = EAIState.Attack;
           
		    _guardBrains = (GuardBrains) brains;
            _guardBrains.RanAway += GuardBrainsOnRanAway;
            DeathController.Death += OnDeath;
		}

        private void OnDeath()
        {
            _pendingState = EAIState.Wandering;
            _currentCondition = AIStateCondition.Done;
        }

        private void GuardBrainsOnRanAway()
	    {
	        _pendingState = EAIState.Alert;
	        _currentCondition = AIStateCondition.Done;
	    }

	    public override void OnLeave()
		{
            _masterBrain.MovableObject.MovementSpeed *= 0.5f;
            _masterBrain.StatusText.text = "Whatever..";
            if (PlayerBehaviour.CurrentPlayer != null)
			{
				PlayerQuirks.Attacked = false;
			}
		}

		public override void OnEnter()
		{
			base.OnEnter();

			PlayerQuirks.Attacked = true;
           
            _startPosition = _guardBrains.transform.position;
            _masterBrain.StatusText.text = _guardBrains.AttackStrings[Random.Range(0, _guardBrains.AttackStrings.Length)];
            _sound = ((GuardBrains)_masterBrain).AngerSound;
			
			_player = PlayerBehaviour.CurrentPlayer;
		    _masterBrain.MovableObject.MovementSpeed *= 2f;
			_player.GetComponent<StressAffector>().DemandTickTime *= 0.8f;
            AudioSource.PlayClipAtPoint(_sound, _masterBrain.transform.position, 1f);
        }

		public override void UpdateState()
		{
			base.UpdateState();
			if(IsPlayerReachable())
			{
				MoveToPlayer();
			}
			else
			{
				_currentCondition = AIStateCondition.Done;
				_pendingState = EAIState.Wandering;
			}
		}

		#region PRIVATE

		private void MoveToPlayer()
		{
			var suitableAttackPosition = MapController.GetNodeByPosition(_player.transform.position);
		    if (suitableAttackPosition.CurrentCellType == ECellType.Blocked)
		    {
		        suitableAttackPosition =
		            MapController.GetNeighbours(suitableAttackPosition).First(i => i.CurrentCellType == ECellType.Walkable);
		    } 
			_masterBrain.MovableObject.BeginMovementByPath(Pathfinder.FindPathToDestination(
				_map,
				_masterBrain.MovableObject.CurrentNode.Position,
				suitableAttackPosition.Position));
		}

		private bool IsPlayerReachable()
		{
		    var node = MapController.GetNodeByPosition(_player.transform.position);

		    return node != null && Vector2.Distance(_masterBrain.transform.position, _startPosition) < _guardBrains.ActiveDistance;
		} 

		#endregion
	}
}