using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;

public class Noise : MonoBehaviour
{
    private float t;
    private Texture2D noiseTex;
    private Color[] pix;
    private Color[] pixRequired;
    private Renderer rend;

    public int pixWidth;
    public int pixHeight;

    public float scale = 1.0F;
    public float LerpSpeed;

    void Start()
    {
        rend = GetComponent<Renderer>();
        
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        pixRequired = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex; 
        GenerateNewTexture();   
    }

    private void Update()
    {
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = Color.Lerp(pix[i], pixRequired[i], t);
        }
        t = Mathf.MoveTowards(t, 1f, LerpSpeed);
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        if (t >= 0.8f)
        {
            GenerateNewTexture();
        }
    }

    private void GenerateNewTexture()
    {
            var xOrg = Random.Range(0, 3f);
            var yOrg = Random.Range(0, 3f);
            float y = 0.0F;
            while (y < noiseTex.height)
            {
                float x = 0.0F;
                while (x < noiseTex.width)
                {
                    float xCoord = xOrg + x / noiseTex.width * scale;
                    float yCoord = yOrg + y / noiseTex.height * scale;
                    float sample = Mathf.PerlinNoise(xCoord, yCoord);
                    pixRequired[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample, sample);
                    x++;
                }
                y++;
            }
            t = 0.0f;
    }
}
