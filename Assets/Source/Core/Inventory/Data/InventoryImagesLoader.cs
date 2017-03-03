using UnityEngine;
using System.Collections.Generic;


namespace Core.Inventory
{
	public enum EItemType
	{
		Consumable,
		Receipt,
		Generic,
		Trap
	}

	public static class InventoryImagesLoader
	{
		#region Private

		private const string kConsumablesPath = "Sprites/Items/Consumables/";
		private const string kGenericPath = "Sprites/Items/Generic/";
		private static Dictionary<string, Sprite> _cachedImages = new Dictionary<string, Sprite> ();

		#endregion

		public static Sprite GetImageForItem (EItemType itemType, string spriteId)
		{
			Sprite cashedSprite;
			_cachedImages.TryGetValue (spriteId, out cashedSprite);
			var rescourcePath = "";

			switch (itemType)
			{
			case EItemType.Consumable:
				{
					rescourcePath = kConsumablesPath;
					break;
				}
			case EItemType.Trap:
			case EItemType.Receipt:
			case EItemType.Generic:
				{
					rescourcePath = kGenericPath;
					break;
				}
			}

			if (cashedSprite == null)
			{
				var sprite = Resources.Load<Sprite> (rescourcePath + spriteId);
				_cachedImages.Add (spriteId, sprite);
				return sprite;
			
			}
			return cashedSprite;
		}
	}
}

