using UnityEngine;


namespace Core.Characters.Player.Demand
{
	public class DemandAffector : MonoBehaviour
	{
		public static float DefaultTickTime = 2f;

		public int DemandState;
		public float DemandTickTime = 2f;
	    public Animator Animator;
	    public AudioClip Clip;

		private void Awake ()
		{
			DefaultTickTime = DemandTickTime;
		}

	    public void AnimateEffect()
	    {
	        Animator.SetTrigger("Effect");
            AudioSource.PlayClipAtPoint(Clip, Camera.main.transform.position);
	    }
	}
}


