using UnityEngine;
using UnityEngine.SceneManagement;
using System;


namespace Core.Map
{
	[RequireComponent(typeof(Collider2D))]
	public class NonWalkable : MonoBehaviour
	{
		public bool Active = true;

		public event Action Disabled;

		public Bounds Bounds
		{
			get
			{
				return GetComponent<Collider2D>().bounds;
			}
		}

		public void ClearObject()
		{
			gameObject.SetActive(false);
			if(Disabled != null)
			{
				Disabled();
			}
		}
	}
}

