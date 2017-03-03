using System;
using UnityEngine;
using UnityEngine.UI;


namespace Core.Map
{
    public enum EDayTime
    {
        Morning,
        Day,
        Evening,
        Night,
    }

    [ExecuteInEditMode]
    public class DayNight : MonoBehaviour
    {
        public static event Action<EDayTime> DayStateChanged;
        public static float StaticTimeLeft;
        private SpriteRenderer[] _renderers;
        private bool _blocked;

        private float _time;
        private float _currentDuration;

        public EDayTime State;
        public float TimeLeft;
        
        [Header("Time periods")]
        public float DayLenght;
        public float MorningLength;
        public float EveningLength;
        public float NightLength;
        public float TimeRate = 0.001f;

        [Header("Symbols")]
        public Image SunImage;
        public Image MoonImage;
       

        // Use this for initialization
        void Start()
        {
            _renderers = FindObjectsOfType<SpriteRenderer>();
        }

        private void OnValidate()
        {
            if (DayStateChanged != null)
            {
                DayStateChanged(State);
            }
        }

        private void Update()
        {
            if (!_blocked)
            {
                _time -= TimeRate;

                if (_time <= 0f)
                {
                    TimeLeft = 1f;
                    StaticTimeLeft = 1f;
                    ChangeDayState();
                }
                else
                {
                    TimeLeft = Mathf.Clamp(_time / _currentDuration, 0f, 1f);
                    StaticTimeLeft = TimeLeft;
                }
            }
        }

        public void Block(bool val)
        {
            if (_blocked != val && val)
            {
                State = EDayTime.Evening;
                StaticTimeLeft = 0.1f;
                TimeLeft = 0.1f;
                ChangeDayState();
            }
            _blocked = val;
        }

        private void ChangeDayState()
        {
            switch (State)
            {
                case EDayTime.Morning:
                    {
                        _time = DayLenght;                        
                        State = EDayTime.Day;
                        break;
                   }
                case EDayTime.Day:
                    {
                        _time = EveningLength;
                        State = EDayTime.Evening;
                        break;
                    }
                case EDayTime.Evening:
                    {
                        _time = NightLength;
                        State = EDayTime.Night;
                        break;
                    }
                case EDayTime.Night:
                    {
                        _time = MorningLength;
                        State = EDayTime.Morning;
                        break;
                    }
            }
            _currentDuration = _time;
            if (DayStateChanged != null)
            {
                DayStateChanged(State);
            }
        }
    }
}

