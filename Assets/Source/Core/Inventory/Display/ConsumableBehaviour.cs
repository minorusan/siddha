using UnityEngine;
using System;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;

#endif


namespace Core.Inventory.Display
{
	[RequireComponent (typeof(SpriteRenderer))]
	public class ConsumableBehaviour : ConsumableDisplay
	{
		#region Private

		private SpriteRenderer _renderer;

		#endregion

		private void OnTriggerEnter2D (Collider2D col)
		{
			
			PlayerInventory.Instance.TryAddItemToInventory (_selectedConsumable);
			gameObject.SetActive (false);
		}

		public override void ApplyImage ()
		{
			_renderer = GetComponent <SpriteRenderer> ();
			_renderer.sprite = InventoryImagesLoader.GetImageForItem (EItemType.Consumable, _selectedConsumable.ItemID);
		}
	}

	#if UNITY_EDITOR

	[CustomEditor (typeof(ConsumableBehaviour))]
	public class ConsumableBehaviourEditor:Editor
	{
		public override void OnInspectorGUI ()
		{
			var consumableBehaviour = (ConsumableBehaviour)target;

			consumableBehaviour.RequiresInitialisation = EditorGUILayout.ToggleLeft ("Automatic", consumableBehaviour.RequiresInitialisation);
			consumableBehaviour.AffectsDemand = (AConsumableBase.EDemand)EditorGUILayout.EnumPopup ("Demand to affect", consumableBehaviour.AffectsDemand);
			var consumableTypeLabelText = "Consumable type";
			switch (consumableBehaviour.AffectsDemand)
			{
			case AConsumableBase.EDemand.Hunger:
				{
					consumableBehaviour.HungerDecreaserType = (HungerDecreasers)EditorGUILayout.EnumPopup (consumableTypeLabelText,
					                                                                                       consumableBehaviour.HungerDecreaserType);
					break;
				}
			case AConsumableBase.EDemand.Stress:
				{
					consumableBehaviour.StressDecreaserType = (StressDecreasers)EditorGUILayout.EnumPopup (consumableTypeLabelText,
					                                                                                       consumableBehaviour.StressDecreaserType);
					break;
				}
			default:
				break;
			}

			consumableBehaviour.InitConsumable ();
		}
	}

	[CustomEditor (typeof(ConsumableUI))]
	public class ConsumableUIEditor:Editor
	{
		public override void OnInspectorGUI ()
		{
			var consumableBehaviour = (ConsumableUI)target;
			consumableBehaviour.RequiresInitialisation = EditorGUILayout.ToggleLeft ("Automatic", consumableBehaviour.RequiresInitialisation);
		}
	}
	#endif
}

