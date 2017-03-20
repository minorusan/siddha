using UnityEngine;

namespace Core.Inventory
{
	public class StackableItemBase : AItemBase
	{
		private const string kTrapsPath = "Prefabs/Projectiles/";

		private GameObject _projectilePrefab;
        private int _maxInStack;

		public GameObject ProjectilePrefab
		{
			get
			{
				return _projectilePrefab;
			}
		}

        public int MaxInStack
        {
            get
            {
                return _maxInStack;
            }
        }

        public StackableItemBase(string itemId, string name, int maxInStack) : base(itemId, name, EItemType.Stackable)
		{
			_projectilePrefab = Resources.Load <GameObject>(kTrapsPath + itemId);
            _maxInStack = maxInStack;
		}
	}
}


