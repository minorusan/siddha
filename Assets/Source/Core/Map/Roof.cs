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
            _enter = Resources.Load<AudioClip>("Sounds/swoosh");
            _dayNight = FindObjectOfType<DayNight>();
            _selfRenderer = GetComponent<Renderer>();
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                AudioSource.PlayClipAtPoint(_enter, Camera.main.transform.position);

                _selfRenderer.enabled = false;

                for (int i = 0; i < transform.parent.parent.childCount; i++)
                {
                    if (transform.parent.parent.GetChild(i).gameObject.name == "NPC")
                    {
                        transform.parent.parent.GetChild(i).gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                for (int i = 0; i < transform.parent.parent.childCount; i++)
                {
                    if (transform.parent.parent.GetChild(i).gameObject.name == "NPC")
                    {
                        transform.parent.parent.GetChild(i).gameObject.SetActive(false);
                    }
                }

                _selfRenderer.enabled = true;
            }
          }

        #endregion
    }
}

