using Core.Characters.Player;
using UnityEngine;

namespace Utils
{ 
    public class Shadow : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag)
            {
                var color = PlayerBehaviour.CurrentPlayer.Renderer.color;
                PlayerBehaviour.CurrentPlayer.Renderer.color = new Color(color.r, color.g, color.b, 0.3f);
                PlayerQuirks.Shadowed = !PlayerQuirks.Attacked; ;
            }
        }

        private void OnTriggerStay2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag)
            {
                PlayerQuirks.Shadowed = !PlayerBehaviour.CurrentPlayer.Moves && !PlayerQuirks.Attacked;
                var color = PlayerBehaviour.CurrentPlayer.Renderer.color;
                PlayerBehaviour.CurrentPlayer.Renderer.color = new Color(color.r, color.g, color.b, 0.3f);
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag)
            {
                var color = PlayerBehaviour.CurrentPlayer.Renderer.color;
                PlayerBehaviour.CurrentPlayer.Renderer.color = new Color(color.r, color.g, color.b, 1f);
                PlayerQuirks.Shadowed = false;
            }
        }
    }
}

