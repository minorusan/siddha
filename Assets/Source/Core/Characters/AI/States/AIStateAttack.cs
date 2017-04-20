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
            ObjectPooling.PoolManager.Instance.CreatePool(_guardBrains.Target, 5);
            PlayerQuirks.Attacked = true;
           
            _startPosition = _guardBrains.transform.position;
            _masterBrain.StatusText.text = _guardBrains.AttackStrings[Random.Range(0, _guardBrains.AttackStrings.Length)];
            _masterBrain.AnimationController.CurrentState = Enemies.EAnimationState.EAnimationStateAttack;
            _player = PlayerBehaviour.CurrentPlayer;
		    _masterBrain.MovableObject.MovementSpeed *= 2f;

            _masterBrain.AnimationController.CurrentState = Enemies.EAnimationState.EAnimationStateAttack;
            AudioSource.PlayClipAtPoint(_guardBrains.AngerSound, _masterBrain.transform.position, 1f);
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
		      var  suitableAttackPositions =
		            MapController.GetNeighbours(suitableAttackPosition).Where(ni => ni.CurrentCellType == ECellType.Walkable).ToArray();

            var path = new Path();
            int i = 0;
            while (i < suitableAttackPositions.Length - 1 && path.Empty)
            {
                path = Pathfinder.FindPathToDestination(
                _map,
                _masterBrain.MovableObject.CurrentNode.Position,
                suitableAttackPositions[i].Position);
                i++;
                ObjectPooling.PoolManager.Instance.ReuseObject(_guardBrains.Target, suitableAttackPositions[i].Position, Quaternion.identity);
            }

           
            _masterBrain.MovableObject.BeginMovementByPath(path);
		}

		private bool IsPlayerReachable()
		{
		    var node = MapController.GetNodeByPosition(_player.transform.position);

		    return node != null && Vector2.Distance(_masterBrain.transform.position, _startPosition) < _guardBrains.ActiveDistance;
		} 

		#endregion
	}
}