using UnityEngine;

namespace Core.Inventory
{
	public delegate void TrapAction(GameObject obj);
	public class TrapItemBase : AItemBase
	{
		private const string kTrapsPath = "Prefabs/Traps/";
        private float _requiredTime;

        private TrapAction _trapAction;
		private GameObject _trapPrefab;

		public GameObject TrapPrefab
		{
			get
			{
				return _trapPrefab;
			}
		}

		public float RequiredTime
		{
			get
			{
				return _requiredTime;
			}
		}

		public TrapAction TrapAction
		{
			get
			{
				return _trapAction;
			}
		}

		public TrapItemBase(string itemId, string name, float requiredTime, TrapAction trapAction) : base(itemId, name, EItemType.Trap)
		{
			_requiredTime = requiredTime;
			_trapAction = trapAction;
			_trapPrefab = Resources.Load <GameObject>(kTrapsPath + itemId);
		}
	}
}