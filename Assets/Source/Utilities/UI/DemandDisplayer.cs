using UnityEngine;
using System.Collections;
using Core.Characters.Player.Demand;
using UnityEngine.UI;


namespace Utilities.UI
{
	public enum EDemandType
	{
		None,
		Hunger,
		Stress
	}

	[RequireComponent (typeof(Image))]
	public class DemandDisplayer : MonoBehaviour
	{
		#region PRIVATE

		private Image _demandBar;
		private DemandAffector _affector;

		#endregion

		public  EDemandType DemandType = EDemandType.None;

		#region Monobehaviour

		private void Start ()
		{
			_demandBar = GetComponent <Image> ();
			InitAffector ();
		}

		private void Update ()
		{
			_demandBar.fillAmount = (float)_affector.DemandState / 100;
		}

		private void InitAffector ()
		{
			switch (DemandType)
			{
			case EDemandType.Hunger:
				{
					_affector = FindObjectOfType<HungerAffector> ();
					break;
				}
			case EDemandType.Stress:
				{
					_affector = FindObjectOfType<StressAffector> ();
					break;
				}
			default :
				{
					Debug.LogError (this.name + ":: affector type not selected.");
					break;
				}
			}
		}

		#endregion
	}
}

