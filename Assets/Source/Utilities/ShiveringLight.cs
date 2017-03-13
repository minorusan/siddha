using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShiveringLight : MonoBehaviour {

    private Light _light;
    private float _initialIntensity;
    public float Chance;
	void Start ()
    {
        _light = GetComponent<Light>();
        _initialIntensity = _light.intensity;
	}
	
	void Update ()
    {
        float randValue = Random.value;
        if (randValue > Chance) 
        {
            _light.intensity = _initialIntensity *= Random.Range(0.9f, 1.1f);
        }
    }
}
