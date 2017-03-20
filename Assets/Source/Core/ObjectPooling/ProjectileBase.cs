using Core.Characters.Player;
using Core.Interactivity.Combat;
using UnityEngine;

namespace Core.ObjectPooling
{
    public enum EProjecileID
    {
        ProjectileRock,
        ProjectileBottle,
        ProjectileShard
    }
    public class ProjectileBase : PoolObject
    {
        private Vector3 _target;
        private bool _active;

        private DamageInfo _info = new DamageInfo();
        private Renderer _renderer;
        private TrailRenderer _trailRenderer;
        public Material[] Materials;
        public Material[] TrailMaterials;
        public AudioClip[] ThrowSounds;
        public AudioClip[] HitSounds;

        public EProjecileID ID;
        public float Speed;
        public float Cooldown;
        public float BaseDamage;

        public DamageInfo Damage
        {
            get
            {
                return _info;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _trailRenderer = GetComponent<TrailRenderer>();
            InitDamage();
        }

        private void Update()
        {
            if (!_active)
            {
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * Speed);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _active = false;
            AudioSource.PlayClipAtPoint(HitSounds[Random.Range(0, HitSounds.Length - 1)], transform.position);
            GetComponent<Collider2D>().enabled = false;
        }

        public override void OnObjectReuse(object parameters = null)
        {
            base.OnObjectReuse(parameters);
            InitDamage();
            _active = true;

            GetComponent<Collider2D>().enabled = true;
            _target = new Vector3(ThrowController.Instance.ThrowTarget.transform.position.x,
                ThrowController.Instance.ThrowTarget.transform.position.y + 0.5f,
                -0.5f);
            AudioSource.PlayClipAtPoint(ThrowSounds[Random.Range(0, ThrowSounds.Length - 1)], transform.position);
        }

        private void InitDamage()
        {
            _info.Crit = Random.value < PlayerQuirks.GetSkill(EPlayerSkills.Throwing) + 0.1f;
            _info.Damage = _info.Crit ? BaseDamage * Random.Range(1.8f, 2.3f) : BaseDamage * Random.Range(0.7f, 1.3f);

            _renderer.material = !_info.Crit ? Materials[0] : Materials[1];
            _trailRenderer.material = !_info.Crit ? TrailMaterials[0] : TrailMaterials[1];
        }
    }
}
