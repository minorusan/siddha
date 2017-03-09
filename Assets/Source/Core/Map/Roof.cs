using Core.Characters.Player;
using DynamicLight2D;
using UnityEngine;


namespace Core.Map
{
    [RequireComponent(typeof(Collider2D))]
    public class Roof : MonoBehaviour
    {
        private Renderer[] _renderers;
        private AudioClip _enter;
        private DynamicLight[] _lights;
        private Renderer _selfRenderer;
        private Canvas[] _canvases;
        private MovableObject[] _movableObjects;
        private DayNight _dayNight;

        public bool DisableOnAwake = false;

        #region Monobehaviour

        private void Start()
        {
            _enter = Resources.Load<AudioClip>("Sounds/windowShowFX");
            _dayNight = FindObjectOfType<DayNight>();
            _selfRenderer = GetComponent<Renderer>();
            
            _lights = transform.parent.parent.gameObject.GetComponentsInChildren<DynamicLight>();
            if (DisableOnAwake)
            {
                foreach (var dynamicLight in _lights)
                {
                    dynamicLight.enabled = false;
                }

                _selfRenderer.enabled = true;
            }
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                AudioSource.PlayClipAtPoint(_enter, collision.transform.position);
                foreach (var dynamicLight in _lights)
                {
                    dynamicLight.enabled = true;
                }

                _selfRenderer.enabled = false;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
               // PlayerBehaviour.CurrentPlayer.TurnOnLight(true);
                //_dayNight.Block(true);
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
               // _dayNight.Block(false);

               // PlayerBehaviour.CurrentPlayer.TurnOnLight(false);
                foreach (var dynamicLight in _lights)
                {
                    dynamicLight.enabled = false;
                }
             
                _selfRenderer.enabled = true;
            }
          }

        #endregion
    }
}

