using UnityEngine;
using System.Collections;
using System;


namespace Core.Inventory
{
	public delegate void TrapAction(GameObject obj);
	public class TrapItemBase : AItemBase
	{
		private const string kTrapsPath = "Prefabs/Traps/";
		private TrapAction _trapAction;
		private float _requiredTime;
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


