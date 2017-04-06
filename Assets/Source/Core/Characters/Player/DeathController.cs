using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Characters.Player
{
    public class DeathController : MonoBehaviour
    {
        private Transform _latestCheckpoint;
        [Range(0, 100)]
        public int DeathPossibility = 20;
        public static event Action Death; 

        public void Kill()
        {
            
            SceneManager.LoadScene(0);
            return;

            var minDeathChance = PlayerQuirks.GetCharactheristic(EPlayerCharachteristic.Empathy) +
                PlayerQuirks.GetCharactheristic(EPlayerCharachteristic.Prowlness) +
                PlayerQuirks.GetCharactheristic(EPlayerCharachteristic.Reflection);

            if (UnityEngine.Random.Range(Mathf.Clamp(minDeathChance,0, 100), 100) > DeathPossibility)
            {
                // StartCoroutine(DeathEffect());
                PlayerBehaviour.CurrentPlayer.transform.position = _latestCheckpoint.position;

            }
            else
            {
                SceneManager.LoadScene(0);
            }
            Death();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "CheckPoint")
            {
                _latestCheckpoint = collision.transform;
            }
        }

        private IEnumerator DeathEffect()
        {
            for (float i = 1; i >= 0; i-= 0.01f)
            {
                Time.timeScale = i;
                Camera.main.orthographicSize -= 0.01f;
                yield return new WaitForEndOfFrame();
            }
            // StartCoroutine(DeathEffectContinue());
            Death();
            PlayerBehaviour.CurrentPlayer.transform.position = _latestCheckpoint.position;
        }

        private IEnumerator DeathEffectContinue()
        {
            for (float i = 0; i <= 1; i += 0.01f)
            {
                Time.timeScale = i;
                Camera.main.orthographicSize += 0.01f;
                yield return new WaitForEndOfFrame();
            }
            PlayerBehaviour.CurrentPlayer.transform.position = _latestCheckpoint.position;
           
        }
    }

}