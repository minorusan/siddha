using UnityEngine;
using Core.Characters.Player;


namespace Core.Inventory.Display
{
	[RequireComponent (typeof(SpriteRenderer))]
	public class GenericItemDisplay : MonoBehaviour
	{
		private SpriteRenderer _selfRenderer;
		private AudioClip _pickup;

		public string ItemID;

		void Start ()
		{
			_pickup = Resources.Load <AudioClip> ("Sounds/pickup");
			_selfRenderer = GetComponent <SpriteRenderer> ();
			_selfRenderer.sprite = InventoryImagesLoader.GetImageForItem (EItemType.Generic, ItemID);
		}

		private void OnTriggerEnter2D (Collider2D col)
		{
			AudioSource.PlayClipAtPoint (_pickup, Camera.main.transform.position, 1f);
			PlayerInventory.Instance.TryAddItemToInventory (ItemsData.GetItemById (ItemID));

			gameObject.SetActive (false);
		}

		private void OnDrawGizmos ()
		{
			_selfRenderer = GetComponent <SpriteRenderer> ();
			_selfRenderer.sprite = InventoryImagesLoader.GetImageForItem (EItemType.Generic, ItemID);
		}
	}
}
