
using UnityEngine;

namespace Utils
{
	[RequireComponent (typeof(Collider2D))]
	public class NoiseEffect : MonoBehaviour
	{

	    private Renderer _renderer;

	    private void Start()
	    {
	        _renderer = GetComponent<Renderer>();
	       
	    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                _renderer.enabled = false;

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                _renderer.enabled = true;
               
            }
        }
    }
}

