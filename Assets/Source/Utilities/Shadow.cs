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
                var material = PlayerBehaviour.CurrentPlayer.Renderer.material;
                material.color = new Color(0.4f, 0.4f, 0.4f, 1f);
                PlayerQuirks.Shadowed = !PlayerQuirks.Attacked; ;
            }
        }

        private void OnTriggerStay2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag)
            {
                PlayerQuirks.Shadowed = !PlayerBehaviour.CurrentPlayer.Moves && !PlayerQuirks.Attacked;
                var material = PlayerBehaviour.CurrentPlayer.Renderer.material;
                material.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }

        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.tag == PlayerBehaviour.kPlayerTag)
            {
                var material = PlayerBehaviour.CurrentPlayer.Renderer.material;
                material.color = new Color(1f, 1f, 1f, 1f);
                PlayerQuirks.Shadowed = false;
            }
        }
    }
}

