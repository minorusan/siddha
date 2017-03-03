using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.UI;

public class Reveal : MonoBehaviour {

    public void OnReveal(GameObject obj)
    {
        if (obj.tag == "NPC")
        {
            var npc = obj.GetComponent<NPC>();
            npc.enabled = true;
            npc.DuplicateSprite.gameObject.SetActive(false);
        }
    }

    public void OnHide(GameObject obj)
    {
        if (obj.tag == "NPC")
        {
            var npc = obj.GetComponent<NPC>();
            npc.enabled = false;
            npc.DuplicateSprite.gameObject.SetActive(true);
        }
    }
}
