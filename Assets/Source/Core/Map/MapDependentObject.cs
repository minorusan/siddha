using UnityEngine;
using System.Collections;


namespace Core.Map
{
	
	public class MapDependentObject : MonoBehaviour
	{
		protected MapController _map;
		protected Node _myPosition;

		public MapController Map
		{
			get
			{
				if (_map == null)
				{
				    GetOwnerMap ();
				}

				return _map;
			}
		}

        public ECellType CurrentCellType;
		public Node CurrentNode
		{
			get
			{
                var currentNode = _map.GetNodeByPosition(transform.position);
                CurrentCellType = currentNode.CurrentCellType;
                return currentNode;
			}
			set
			{
				Debug.Assert (value != null, this.name + " attempted to obtain null position.");
				_myPosition = value;
			}
		}
		// Use this for initialization
		protected virtual void Start ()
		{
			GetOwnerMap ();
		}

		private void GetOwnerMap ()
		{
            _map = MapController.GetMap(transform.position);
            Debug.Assert(_map != null, this.name + " has no map! Help!");
		}
	}

}
