using UnityEngine;
using System.Collections;
using Core.Map;


namespace Utils
{
	public class ChunkReset : MonoBehaviour
	{
		private void OnTriggerEnter2D (Collider2D col)
		{
			var t = FindObjectOfType <ChunkCreator> ().gameObject;
			t.SetActive (false);
			t.SetActive (true);
		}
	}
}

