using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Core.Characters.Player.Demand
{
	[RequireComponent (typeof(HungerAffector))]
	[RequireComponent (typeof(PlayerBehaviour))]
	public class StressAffector : DemandAffector
	{
		#region PRIVATE

		private PlayerBehaviour _player;
		private HungerAffector _hungerAffector;

		private float kMinimumTickSpeed = 0.2f;
		private float kDecreaseCoef = 0.1f;

		#endregion

		private void Start ()
		{
			_player = GetComponent<PlayerBehaviour> ();
			_hungerAffector = GetComponent<HungerAffector> ();
			StartCoroutine (StressTick ());
		}

		private IEnumerator StressTick ()
		{
			while (true)
			{
				if (DemandState < 100)
				{
					DemandState += 1;

					if (_hungerAffector.DemandTickTime > kMinimumTickSpeed)
					{
						_hungerAffector.DemandTickTime -= kDecreaseCoef;
					}
				}
				yield return new WaitForSeconds (DemandTickTime);
			}
		}
	}
}

