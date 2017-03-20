using Core.Characters.Player;
using Core.ObjectPooling;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Interactivity.Combat
{
    public class ThrowController : MonoBehaviour
    {
        private static ThrowController _instance;
        private float _coolDown;
        private ProjectileBase _projectile;
        public GameObject ThrowTarget { get; set; }
        private float _coolDownLeft;
        private Quaternion _rotationVector;

        [Header("View set up")]
        public Image CircleView;
        public ProjectileBase[] Projectiles;
        public GameObject Target;

        public static ThrowController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ThrowController>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            _rotationVector = Quaternion.Euler(new Vector3(0f, 0f, -1f));
            for (int i = 0; i < Projectiles.Length; i++)
            {
                PoolManager.Instance.CreatePool(Projectiles[i].gameObject, 20);
            }
            SetProjectile(EProjecileID.ProjectileRock);
        }

        public void SetProjectile(EProjecileID id)
        {
            _projectile = Projectiles.FirstOrDefault(p=>p.ID == id);
            _coolDown = _projectile.Cooldown;
        }

        public void Update()
        {
            HandleTargetUpdate();
        }

        private void HandleTargetUpdate()
        {
            if (ThrowTarget != null && ThrowTarget.activeInHierarchy)
            {
                if (!Target.activeInHierarchy)
                {
                    Target.SetActive(true);
                }
                Target.transform.position = new Vector3(ThrowTarget.transform.position.x,
                    ThrowTarget.transform.position.y + 0.3f, -0.5f);

                Target.transform.rotation = Quaternion.RotateTowards(Target.transform.rotation, Target.transform.rotation * _rotationVector, 2f);
            }
            else
            {
                Target.SetActive(false);
                ThrowTarget = null;
            }
        }

        public void Throw()
        {
            if (ThrowTarget != null && _coolDownLeft <= 0f)
            {
                _coolDownLeft = _coolDown;
                StartCoroutine(HandleCooldown());
                PoolManager.Instance.ReuseObject(_projectile.gameObject, new Vector3(PlayerBehaviour.CurrentPlayer.transform.position.x,
                    PlayerBehaviour.CurrentPlayer.transform.position.y + 0.5f, -1.2f), Quaternion.identity);
            }
        }

        private IEnumerator HandleCooldown()
        {
            while (_coolDownLeft >= 0f)
            {
                _coolDownLeft -= Time.smoothDeltaTime;
                CircleView.fillAmount = 1f - _coolDownLeft / _coolDown;
                yield return new WaitForEndOfFrame();
            }

        }
    }

}
