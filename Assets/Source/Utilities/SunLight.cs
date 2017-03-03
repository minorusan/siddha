using System;
using System.Collections;
using System.Collections.Generic;
using Core.Map;
using UnityEngine;

namespace Utilities
{
    [ExecuteInEditMode]
    public class SunLight : MonoBehaviour
    {
        private Light _selfLight;

        private float _prevValue;
        private Color _prevColor;

        private float _valueToLerp;
        private Color _colourToLerp;
        
        [Header("Day")]
        public Color DayColor;
        public float DayIntensity;

        [Header("Evening")]
        public Color EveningColor;
        public float EveningIntensity;

        [Header("Night")]
        public Color NightColor;
        public float NightIntensity;

        [Header("Morning")]
        public Color MorningColor;
        public float MorningIntensity;

        private void Start()
        {
            _selfLight = GetComponent<Light>();
           
            DayNight.DayStateChanged += DayNight_DayStateChanged;
            DayNight_DayStateChanged(FindObjectOfType<DayNight>().State);
        }

        private void OnDestroy()
        {
            DayNight.DayStateChanged -= DayNight_DayStateChanged;
        }
        private void DayNight_DayStateChanged(EDayTime obj)
        {
            _prevValue = _selfLight.intensity;
            _prevColor = _selfLight.color;
            switch (obj)
            {
                case EDayTime.Morning:
                    _colourToLerp = DayColor;
                    _valueToLerp = DayIntensity;
                    break;
                case EDayTime.Day:
                    _colourToLerp = EveningColor;
                    _valueToLerp = EveningIntensity;
                    break;
                case EDayTime.Evening:
                    _colourToLerp = NightColor;
                    _valueToLerp = NightIntensity;
                    break;
                case EDayTime.Night:
                    _colourToLerp = MorningColor;
                    _valueToLerp = MorningIntensity;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("obj", obj, null);
            }
        }

        private void Update()
        {
            _selfLight.intensity = Mathf.Lerp(_prevValue, _valueToLerp, Mathf.Clamp(1f - DayNight.StaticTimeLeft, 0f, 1f));
            _selfLight.color = Color.Lerp(_prevColor, _colourToLerp, Mathf.Clamp(1f - DayNight.StaticTimeLeft, 0f, 1f));
        }
    }
}

