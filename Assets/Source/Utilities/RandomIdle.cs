using Core.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    [RequireComponent(typeof(Animator))]
    public class RandomIdle : MonoBehaviour
    {
        private Animator _selfAnimator;
        private PlayerBehaviour _player;
        private bool _canInvoke = true;

        public string[] Clips;
        public Vector2 InvacationRatio;
        
        // Use this for initialization
        void Start()
        {
            _selfAnimator = GetComponent<Animator>();
            _player = GetComponentInParent<PlayerBehaviour>();
        }
        
        void Update()
        {
            if (!_player.Moves && _canInvoke)
            {
                Invoke("PlayRandomClip", Random.Range(InvacationRatio.x, InvacationRatio.y));
                _canInvoke = false;
            }

            if (_player.Moves)
            {
                _canInvoke = true;
            }
        }

        private void PlayRandomClip()
        {
            if (!_player.Moves)
            {
                _selfAnimator.Play(Clips[Random.Range(0, Clips.Length)]);
                _canInvoke = true;
            }
        }
    }
}

