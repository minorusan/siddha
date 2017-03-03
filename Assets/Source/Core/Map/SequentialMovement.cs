using UnityEngine;
using Core.Map.Pathfinding;


namespace Core.Map
{
	public class SequentialMovement
	{
		#region Private

		private Transform[] _path;
		private int _currentIndex;
		private MovableObject _client;
		private bool _looped;

		#endregion

		public SequentialMovement(Transform[] path, MovableObject client, bool looped = false)
		{
			_path = path;
			_client = client;
			_looped = looped;
		}

		public void UpdateMovement()
		{
			if(_client.CurrentPath.Empty)
			{
				_currentIndex++;
				if(_currentIndex > _path.Length - 1 && _looped)
				{
					_currentIndex = 0;
				}
				else
				if(_currentIndex > _path.Length && !_looped)
				{
					return;
				}

				BeginMovement();
			}
		}

		private void BeginMovement()
		{
			_client.BeginMovementByPath(
				Pathfinder.FindPathToDestination(_client.Map,
				                                 _client.transform.position,
				                                 _path[_currentIndex].position));
		}
	}
}


