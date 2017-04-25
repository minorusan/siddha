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
            _map = MapController.GetMap(transform.position);
		}
	}

}
