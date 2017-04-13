using UnityEngine;
using System;
using System.Collections;

public class AnimateSpriteSheet : MonoBehaviour
{
    public event Action AnimationFinished;

    private Renderer _renderer;
    public int Columns = 5;
    public int Rows = 5;
    public float FramesPerSecond = 10f;
    public bool RunOnce = true;

    public float RunTimeInSeconds
    {
        get
        {
            return ((1f / FramesPerSecond) * (Columns * Rows));
        }
    }

    private Material materialCopy = null;

    void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        // Copy its material to itself in order to create an instance not connected to any other
        materialCopy = new Material(_renderer.sharedMaterial);
        _renderer.sharedMaterial = materialCopy;

        Vector2 size = new Vector2(1f / Columns, 1f / Rows);
        _renderer.sharedMaterial.SetTextureScale("_MainTex", size);
        StartCoroutine(UpdateTiling());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator UpdateTiling()
    {
        float x = 0f;
        float y = 0f;
        Vector2 offset = Vector2.zero;

        while (true)
        {
            for (int i = Rows - 1; i >= 0; i--) // y
            {
                y = (float)i / Rows;

                for (int j = 0; j <= Columns - 1; j++) // x
                {
                    x = (float)j / Columns;

                    offset.Set(x, y);

                    _renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
                    yield return new WaitForSeconds(1f / FramesPerSecond);
                }
            }

            if (RunOnce)
            {
                if (AnimationFinished != null)
                {
                    AnimationFinished();
                }
                yield break;
            }
        }
    }
}