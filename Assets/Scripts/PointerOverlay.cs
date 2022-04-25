using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PointerOverlay : MonoBehaviour
{
    [Header("Controlling")]
    public ConfigurableControl configurableControl;
    [HideInInspector, SerializeField] private Material effectMaterial = null;
    private Shader effectShader = null;

    [Header("Pointer")]
    [ShowAssetPreview]
    public Texture2D pointerTexture;
    public Vector2 center = new Vector2(0.5f, 0.5f);
    public Vector2 scale = Vector2.one;

    private void OnValidate()
    {
        effectShader = Shader.Find("Hidden/PointerOverlay");
        if (effectShader is not null && effectShader.isSupported)
        {
            effectMaterial = new Material(effectShader);
            if(!effectMaterial.HasTexture("_PointerTexture") || !effectMaterial.HasVector("_Center") || !effectMaterial.HasVector("_Scale"))
            {
                effectMaterial = null;
            }
        }
    }

    private void Start()
    {
        OnValidate();
    }

    private void Update()
    {
        if (effectMaterial is not null)
        {
            effectMaterial.SetTexture("_PointerTexture", pointerTexture);
            effectMaterial.SetVector("_Center", center);
            effectMaterial.SetVector("_Scale", new Vector2(1.0f / scale.x, 1.0f / scale.y));
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(effectMaterial is not null)
        {
            Graphics.Blit(source, destination, effectMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
