using Core.ObjectPooling;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Characters.AI
{
    public class NPCHealthHandler : MonoBehaviour
    {
        private float _startHealth;

        public float Health;
        public Image HealthBar;
        public AudioClip[] PainSounds;
        public GameObject CombatText;

        private void Awake()
        {
            _startHealth = Health;
            PoolManager.Instance.CreatePool(CombatText, 20);
        }

        private void OnEnable()
        {
            Health = _startHealth;
            HealthBar.fillAmount = 1f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                var damage = collision.gameObject.GetComponent<ProjectileBase>().Damage;
                Health -= damage.Damage;
                if (Health <= 0f)
                {
                    gameObject.SetActive(false);
                }

                AudioSource.PlayClipAtPoint(PainSounds[Random.Range(0, PainSounds.Length - 1)], transform.position);
                PoolManager.Instance.ReuseObject(CombatText,
                    new Vector3(collision.transform.position.x, collision.transform.position.y + 1f, collision.transform.position.z)
                    , Quaternion.identity, damage);
                HealthBar.fillAmount = Health / _startHealth;
            }
        }
    }

}
