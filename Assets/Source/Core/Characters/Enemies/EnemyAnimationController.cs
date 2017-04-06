using System;
using UnityEngine;

namespace Core.Characters.Enemies
{
    public enum EAnimationState
    {
        EAnimationStateWalking,
        EAnimationStateSuspicious,
        EAnimationStateAlert,
        EAnimationStateAttack
    }

    public class EnemyAnimationController : MonoBehaviour
    {
        private EAnimationState _currentState;
        public event Action<EAnimationState> AnimationFinished;

        public AnimateSpriteSheet[] StateGfx;

        public EAnimationState CurrentState
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

        private void SwitchAnimationState(EAnimationState state)
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

