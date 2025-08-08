using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Renderer quadRenderer;
    public Sprite initialSprite;

    void Awake()
    {
        quadRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        if (BackgroundManager.Instance != null)
        {
            BackgroundManager.Instance.RegisterController(this);
        }

        if (initialSprite != null)
        {
            ChangeBackground(initialSprite);
        }
    }

    public void ChangeBackground(Sprite newSprite)
    {
        if (newSprite != null && quadRenderer != null)
        {
            quadRenderer.material.mainTexture = newSprite.texture;
        }
    }
}