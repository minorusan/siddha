using System;
using UnityEngine;

namespace Core.Characters.Player.Utils
{
    public enum EPlayerAnimationState
    {
        EAnimationStateWalking,
        EAnimationStateSuspicious,
        EAnimationStateAlert,
        EAnimationStateAttack
    }

    public class PlayerAnimationController : MonoBehaviour
    {
        private EPlayerAnimationState _currentState;
        public event Action<EPlayerAnimationState> AnimationFinished;

        public AnimateSpriteSheet[] StateGfx;

        public EPlayerAnimationState CurrentState
        {
            get
            {
                return _currentState;
            }

            set
            {
                _currentState = value;
                SwitchAnimationState(_currentState);
            }
        }

        private void Start()
        {
            for (int i = 0; i < StateGfx.Length; i++)
            {
                StateGfx[i].AnimationFinished += OnAnimationFinished;
            }
        }

        private void OnAnimationFinished()
        {
            if (AnimationFinished != null)
            {
                AnimationFinished(_currentState);
            }
        }

        private void SwitchAnimationState(EPlayerAnimationState state)
        {
            DeactivateAll();
            StateGfx[(int)state].gameObject.SetActive(true);
        }

        private void DeactivateAll()
        {
            for (int i = 0; i < StateGfx.Length; i++)
            {
                StateGfx[i].gameObject.SetActive(false);
            }
        }
    }
}

