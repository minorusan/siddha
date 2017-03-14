using System.Linq;
using UnityEngine;

using Core.Map.Pathfinding;
using Core.Characters.Player;
using Core.Interactivity.AI;
using Core.Map;
using Random = UnityEngine.Random;


namespace Core.Characters.AI
{
    public class AIStateAlert : AIStateBase
    {
        private float _searchDistance;
        private float _previousMoveSpeed;
        private float _alertTime;
        private GuardBrains _guardBrains;
        private float _timeInState;
        private Core.Characters.Player.PlayerBehaviour _player;

        public AIStateAlert(ArtificialIntelligence brains, float searchDistance, float alertTime) : base(brains)
        {
            _searchDistance = searchDistance;
            _alertTime = alertTime;
            State = EAIState.Alert;
            _timeInState = alertTime;
            _guardBrains = (GuardBrains) brains;
            _guardBrains.Spotted += GuardBrainsOnSpotted;
            _player = GameObject.FindObjectOfType<Core.Characters.Player.PlayerBehaviour>();
        }

        private void GuardBrainsOnSpotted()
        {
            _pendingState = EAIState.Attack;
            _currentCondition = AIStateCondition.Done;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _previousMoveSpeed = _masterBrain.MovableObject.MovementSpeed;
            _masterBrain.MovableObject.MovementSpeed *= 0.1f;
            _timeInState = _alertTime;
            var _guardBrains = (GuardBrains)_masterBrain;
            _masterBrain.StatusText.text = _guardBrains.AlertStrings[Random.Range(0, _guardBrains.AlertStrings.Length)];
        }

        public override void OnLeave()
        {
            _masterBrain.MovableObject.MovementSpeed = _previousMoveSpeed;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            var playerNode = MapController.GetNodeByPosition(_player.transform.position);

            if (_timeInState <= 0)
            {
                _currentCondition = AIStateCondition.Done;
                _pendingState = EAIState.Wandering;
                _masterBrain.StatusText.text = "Whatever..";
            }

            var path = FindNewpath();
            if (path.Nodes.Count <= 1)
            {
                PlayerBehaviour.CurrentPlayer.Kill();
            }

            if (_masterBrain.MovableObject.ReachedDestination)
            {
                _masterBrain.MovableObject.BeginMovementByPath(path);
            }
            _timeInState -= Time.deltaTime;
        }

        private Path FindNewpath()
        {
            var suitableAttackPosition = MapController.GetNodeByPosition(_player.transform.position);
            if (suitableAttackPosition == null)
            {
                return null;
            }
            if (suitableAttackPosition.CurrentCellType == ECellType.Blocked)
            {
                suitableAttackPosition =
                    MapController.GetNeighbours(suitableAttackPosition).First(i => i.CurrentCellType == ECellType.Walkable);
            }
            return Pathfinder.FindPathToDestination(
                _map,
                _masterBrain.MovableObject.CurrentNode.Position,
                suitableAttackPosition.Position);
        }
    }
}