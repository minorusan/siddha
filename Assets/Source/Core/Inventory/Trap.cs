using UnityEngine;
using System.Collections;


namespace Core.Inventory
{
	[RequireComponent (typeof(BoxCollider2D))]
	public class Trap : MonoBehaviour
	{
		private TrapItemBase _selfTrap;
		public string TrapId;
		private AudioClip _pickup;
		private AudioClip _blod;
		private AudioClip pain;

		private void Start ()
		{
			_selfTrap = ItemsData.GetTrapById (TrapId);
			_pickup = Resources.Load <AudioClip> ("Sounds/trap");
			_blod = Resources.Load <AudioClip> ("Sounds/blood");
			pain = Resources.Load <AudioClip> ("Sounds/pain");
		}

		private void OnTriggerEnter2D (Collider2D col)
		{
			if (col.tag != "Player")
			{
				AudioSource.PlayClipAtPoint (_pickup, transform.position);
				AudioSource.PlayClipAtPoint (_blod, transform.position);
				AudioSource.PlayClipAtPoint (pain, transform.position);
				_selfTrap.TrapAction (col.gameObject);
				gameObject.SetActive (false);
			}
		}
	}

}
