using Core.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : PoolObject
{
    private float _iterator;
    private Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        _iterator -= 0.01f;
        if (_iterator <= 0f)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.up, 2f * Time.deltaTime);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _iterator);
    }

    public override void OnObjectReuse(object parameters = null)
    {
        base.OnObjectReuse(parameters);
        var damageInfo = parameters as DamageInfo;
        if(damageInfo != null)
        {
            _iterator = damageInfo.Crit ? 1f : 0.5f;
            transform.localScale = damageInfo.Crit ? Vector3.one * 2f : Vector3.one;
            _text.color = damageInfo.Crit ? Color.red : Color.white;
            _text.text = damageInfo.Damage.ToString("0.00");
        }
    }
}

public class DamageInfo
{
    public bool Crit { get; set; }
    public float Damage { get; set; }
}
