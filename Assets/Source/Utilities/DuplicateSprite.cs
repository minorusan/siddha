using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateSprite : MonoBehaviour
{
    private SpriteRenderer _selfRenderer;
    public SpriteRenderer Parent;

	// Use this for initialization
	void Start ()
	{
	    _selfRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Parent != null)
	    {
            _selfRenderer.sprite = Parent.sprite;

	        //_selfRenderer.sortingOrder = Parent.sortingOrder;
	    }
	    else
	    {
	        Parent = transform.parent.GetComponent<SpriteRenderer>();
	    }
	    
	}
}
