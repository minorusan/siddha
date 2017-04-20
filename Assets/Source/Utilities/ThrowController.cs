﻿using Core.Characters.Player;
using Core.Inventory;
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
        private TargetHandler _targetHandler;
        private ProjectilesInfo _currentProjectileInfo;
        private float _coolDown;
        private ProjectileBase _projectile;
        private GameObject _throwTarget;
        
        private float _coolDownLeft;
        private Quaternion _rotationVector;

        [Header("View set up")]
        public Image CircleView;
        public Image ProjectileView;
        public Text ProjectilesCount;
        public ProjectileBase[] Projectiles;
        public GameObject Target;
        public GameObject Animation;

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

        public GameObject ThrowTarget
        {
            get
            {
                return _throwTarget;
            }

            set
            {
                Target.SetActive(false);
                _throwTarget = value;
                Target.SetActive(true);
            }
        }

        private void Awake()
        {
            for (int i = 0; i < Projectiles.Length; i++)
            {
                PoolManager.Instance.CreatePool(Projectiles[i].gameObject, 10);
            }
          
            _targetHandler = new TargetHandler(this);
        }

        private void Update()
        {
            _targetHandler.UpdateTarget();
        }

        public void SetProjectile(ProjectilesInfo projectileInfo)
        {
            var projectilesInInventory = PlayerInventory.Instance.GetProjectiles();

            _currentProjectileInfo = projectileInfo;
            _projectile = Projectiles.FirstOrDefault(p => p.ID == projectileInfo.Projectile.ID);
            _coolDown = _projectile.Cooldown;

            ProjectileView.sprite = _currentProjectileInfo.Image;
            ProjectilesCount.text = _currentProjectileInfo.CurrentCount.ToString();
            Animation.SetActive(true);
        }

        public void Throw()
        {
            bool canShoot = ThrowTarget != null && _coolDownLeft <= 0f && _currentProjectileInfo != null &&
                _currentProjectileInfo.CurrentCount > 0;
            if (canShoot)
            {
                _coolDownLeft = _coolDown;
                StartCoroutine(HandleCooldown());
                PoolManager.Instance.ReuseObject(_projectile.gameObject, new Vector3(PlayerBehaviour.CurrentPlayer.transform.position.x,
                    PlayerBehaviour.CurrentPlayer.transform.position.y + 0.5f, -1.2f), Quaternion.identity);
                _currentProjectileInfo.CurrentCount--;
                UpdateInfo();
                if (_currentProjectileInfo.CurrentCount <= 0)
                {
                    PlayerInventory.Instance.RemoveItemFromInventory(_currentProjectileInfo.ItemReference.ItemID);
                }
            }
        }

        public void UpdateInfo()
        {
            ProjectilesCount.text = _currentProjectileInfo.CurrentCount.ToString();
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

    public class TargetHandler
    {
        private ThrowController _controller;
        private Quaternion _rotationVector;

        public TargetHandler(ThrowController controller)
        {
            _controller = controller;
            _rotationVector = Quaternion.Euler(new Vector3(0f, 0f, -1f));
        }

        public void UpdateTarget()
        {
            if (_controller.ThrowTarget != null && _controller.ThrowTarget.activeInHierarchy)
            {
                if (!_controller.Target.activeInHierarchy)
                {
                    _controller.Target.SetActive(true);
                }
                _controller.Target.transform.position = new Vector3(_controller.ThrowTarget.transform.position.x,
                    _controller.ThrowTarget.transform.position.y + 0.3f, -0.5f);

                _controller.Target.transform.rotation = Quaternion.RotateTowards(_controller.Target.transform.rotation,
                    _controller.Target.transform.rotation * _rotationVector, 2f);
            }
            else
            {
                _controller.Target.SetActive(false);
                _controller.ThrowTarget = null;
            }
        }
    }
}
