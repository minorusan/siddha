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

        public void SubstractHealth(float damage, DamageInfo damageInfo = null)
        {
            Health -= damage;
            HealthBar.fillAmount = Health / _startHealth; 
            if (Health <= 0f)
            {
                gameObject.SetActive(false);
            }

            AudioSource.PlayClipAtPoint(PainSounds[Random.Range(0, PainSounds.Length - 1)], transform.position);
            PoolManager.Instance.ReuseObject(CombatText,
                new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z)
                , Quaternion.identity, damageInfo);
            HealthBar.fillAmount = Health / _startHealth;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                var damage = collision.gameObject.GetComponent<ProjectileBase>().Damage;
                SubstractHealth(damage.Damage, damage);            }
        }
    }

}
