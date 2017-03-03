using UnityEngine;
using System.Linq;
using System.Collections.Generic;


namespace Core.Inventory.Display
{
	public abstract class ConsumableDisplay : MonoBehaviour
	{
		#region Protected

		protected AConsumableBase _selectedConsumable;

		#endregion

		public bool RequiresInitialisation = true;

		public AConsumableBase.EDemand AffectsDemand;
		public HungerDecreasers HungerDecreaserType;
		public StressDecreasers StressDecreaserType;
		protected AudioClip _pickup;

		private void Start ()
		{
			InitConsumable ();
			_pickup = Resources.Load <AudioClip> ("Sounds/pickup");
		}

		public void InitConsumable ()
		{
			if (!RequiresInitialisation)
			{
				return;
			}

			string selectedItemId = "";
			List<AConsumableBase> consumables = new List<AConsumableBase> ();
			switch (AffectsDemand)
			{
			case AConsumableBase.EDemand.Hunger:
				{
					selectedItemId = ItemIDStorage.GetHungerDecreaserID (HungerDecreaserType);
					consumables = ItemsData.GetConsumablesOfType<HungerDecreaser> ();
					break;
				}
			case AConsumableBase.EDemand.Stress:
				{
					selectedItemId = ItemIDStorage.GetStressDecreaserID (StressDecreaserType);
					consumables = ItemsData.GetConsumablesOfType<StressDecreaser> ();
					break;
				}
			}
			if (consumables.Count > 0)
			{
				_selectedConsumable = consumables.First (c => c.ItemID == selectedItemId);
				ApplyImage ();
			}
		}

		public abstract void ApplyImage ();
	}
}

