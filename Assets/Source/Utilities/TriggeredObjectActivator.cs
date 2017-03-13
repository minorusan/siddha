using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredObjectActivator : MonoBehaviour
{
    public GameObject[] ObjectsToActivate;
    public bool Activate = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < ObjectsToActivate.Length; i++)
        {
            ObjectsToActivate[i].SetActive(Activate);
        }
    }
}
