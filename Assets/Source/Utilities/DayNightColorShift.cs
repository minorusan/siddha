using System;
using System.Collections;
using System.Collections.Generic;
using Core.Map;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightColorShift : MonoBehaviour
{
    public Material Material;
    private float _prevHUE;
    private float _prevValue;
    private float _prevSat;

    private float _goalHUE;
    private float _goalValue;
    private float _goalSat;

    [Header("Day values")]
    public float[] DayValues;
    [Header("Evening values")]
    public float[] EveningValues;
    [Header("Night values")]
    public float[] NightValues;
    [Header("Morning values")]
    public float[] MorningValues;

    // Use this for initialization
    void Awake()
    {
        DayNight.DayStateChanged += OnDayStateChanged;
        OnDayStateChanged(FindObjectOfType<DayNight>().State);
    }

    private void OnDayStateChanged(EDayTime obj)
    {
        _prevValue = _goalValue;
        _prevHUE = _goalHUE;
        _prevSat = _goalSat;
        switch (obj)
        {
            case EDayTime.Morning:
                _goalHUE = Convert.ToInt32(DayValues[0]);
                _goalSat = DayValues[1];
                _goalValue = DayValues[2];
                break;
            case EDayTime.Day:
                _goalHUE = Convert.ToInt32(EveningValues[0]);
                _goalSat = EveningValues[1];
                _goalValue = EveningValues[2];
                break;
            case EDayTime.Evening:
                _goalHUE = Convert.ToInt32(NightValues[0]);
                _goalSat = NightValues[1];
                _goalValue = NightValues[2];
                break;
            case EDayTime.Night:
                _goalHUE = Convert.ToInt32(MorningValues[0]);
                _goalSat = MorningValues[1];
                _goalValue = MorningValues[2];
                break;
            default:
                throw new ArgumentOutOfRangeException("obj", obj, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Material.SetInt("_HueShift", (int)Mathf.Lerp(_prevHUE, _goalHUE, 1f - DayNight.StaticTimeLeft));
        Material.SetFloat("_Sat", Mathf.Lerp(_prevSat, _goalSat, 1f - DayNight.StaticTimeLeft));
        Material.SetFloat("Val", Mathf.Lerp(_prevValue, _goalValue, 1f - DayNight.StaticTimeLeft));
    }
}