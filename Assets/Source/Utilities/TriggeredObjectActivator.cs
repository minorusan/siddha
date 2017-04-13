using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggeredObjectActivator : MonoBehaviour
{
    public GameObject[] ObjectsToActivate;
    public bool Activate = true;
    public string[] activatorNames;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activatorNames.Any(c=>c== collision.gameObject.name))
        {
            for (int i = 0; i < ObjectsToActivate.Length; i++)
            {
                ObjectsToActivate[i].SetActive(Activate);
            }
        }
    }
}
