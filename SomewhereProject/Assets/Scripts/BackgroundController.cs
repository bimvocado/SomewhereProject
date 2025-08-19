using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Renderer quadRenderer;
    public Sprite initialSprite;

    public string currentBackgroundName {  get; private set; }
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
            this.currentBackgroundName = newSprite.texture.name;
        }
    }

    private void OnEnable()
    {
        if (BackgroundManager.Instance != null)
        {
            BackgroundManager.Instance.RegisterController(this);
        }
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.RegisterBackground(this);
        }

        if (initialSprite != null)
        {
            ChangeBackground(initialSprite);
        }
    }

    private void OnDisable()
    {
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.UnregisterBackground(this);
        }
    }
}