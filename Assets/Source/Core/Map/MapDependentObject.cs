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

		public Node CurrentNode
		{
			get
			{
				return _myPosition;
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
			var maps = MapController.GetMapsOnScene ();
			for (int i = 0; i < maps.Length; i++)
			{
				var playerNode = MapController.GetNodeByPosition (transform.position);
				if (playerNode != null && (_map == null || _map != maps [i]))
				{
					_map = maps [i];
					_myPosition = MapController.GetNodeByPosition (transform.position);
					return;
				}
			}
		}
	}

}
