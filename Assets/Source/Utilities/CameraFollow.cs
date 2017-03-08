using UnityEngine;

using Core.Characters.Player;


namespace Utils
{
    public class CameraFollow : MonoBehaviour
    {
        #region PRIVATE
        private Vector3 _destination = new Vector3(0f, 0f, -10f);
        private PlayerBehaviour _player;

        #endregion

        public float TransitionSpeed;
        public bool Instant = true;

        private void OnEnable()
        {
            _player = FindObjectOfType<PlayerBehaviour>();
        }
        
        private void FixedUpdate()
        {
            _destination = new Vector3(_player.transform.position.x,
                                        _player.transform.position.y,
                                        transform.position.z);

            
            transform.position = Instant ? _destination : Vector3.MoveTowards(transform.position, _destination, TransitionSpeed * Time.deltaTime);
        }
    }
}


