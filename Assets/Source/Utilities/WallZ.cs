using Core.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallZ : MonoBehaviour {

	void Update ()
    {
        var playerTransform = PlayerBehaviour.CurrentPlayer.transform;
        if (Vector3.Distance(transform.position, playerTransform.position) < 30f)
        {
            if (playerTransform.position.y < transform.position.y)
            {
                transform.position = new Vector3(transform.position.x,
                    transform.position.y,
                    playerTransform.position.z + 1f);
            }else
            {
                transform.position = new Vector3(transform.position.x,
                    transform.position.y,
                    playerTransform.position.z - 1f);
            }
        }
	}
}
