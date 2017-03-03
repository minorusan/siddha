using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Core.Characters.Player.Demand
{
	[RequireComponent(typeof(PlayerBehaviour))]
	[RequireComponent(typeof(StressAffector))]
	public class HungerAffector : DemandAffector
	{
		#region PRIVATE

		private float kMinimumTickSpeed = 0.2f;
		private float kNervesDecreaseCoef = 0.1f;

		private PlayerBehaviour _player;
		private StressAffector _stressAffector;

		#endregion

		private void Start()
		{
			_player = GetComponent<PlayerBehaviour>();
			_stressAffector = GetComponent<StressAffector>();
			StartCoroutine(HungerTick());
		}

		private IEnumerator HungerTick()
		{
			while(true)
			{
				if(DemandState > 1)
				{
					DemandState--;
				
					var newMovementSpeed = PlayerBehaviour.BaseMovementSpeed * (float)DemandState / 100;
					_player.MovementSpeed = newMovementSpeed < PlayerBehaviour.BaseMovementSpeed ? newMovementSpeed : PlayerBehaviour.BaseMovementSpeed;

					if(_stressAffector.DemandTickTime > kMinimumTickSpeed)
					{
						_stressAffector.DemandTickTime -= kNervesDecreaseCoef;
					}
				}

				yield return new WaitForSeconds(DemandTickTime);
			}
		}
	}
}

