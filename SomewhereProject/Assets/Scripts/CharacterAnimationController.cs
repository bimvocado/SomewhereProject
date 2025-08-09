using UnityEngine;
using DG.Tweening;
using Live2D.Cubism.Rendering;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Renderer[] characterRenderers;
    [SerializeField] private CubismRenderer[] cubismRenderers;

    private Animator animator;

    [Header("DOTween Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Ease fadeEaseType = Ease.OutSine;
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField] private Ease moveEaseType = Ease.OutQuad;

    [Header("Movement Offsets (World Space)")]
    [SerializeField] private float offscreenLeftX = -5f;
    [SerializeField] private float offscreenRightX = 5f;

    private Vector3 originalPosition;

    private Vector3 originalScale;
    private Vector3 targetScale;
    public float scalePercent = 0.95f;
    public float duration = 5f;

    private CubismRenderController renderController;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (characterRenderers == null || characterRenderers.Length == 0)
        {
            characterRenderers = GetComponentsInChildren<Renderer>(true);
            if (characterRenderers.Length == 0)
            {
                Debug.LogWarning($"{gameObject.name}: Renderer 컴포넌트 없음");
            }
        }

        if (cubismRenderers == null || cubismRenderers.Length == 0)
        {
            cubismRenderers = GetComponentsInChildren<CubismRenderer>(true);
            if (cubismRenderers.Length == 0 && characterRenderers.Length == 0)
            {
                Debug.LogWarning($"{gameObject.name}: CubismRenderer 없음");
            }
        }

        originalPosition = transform.position;

        renderController = GetComponent<CubismRenderController>();
    }

    private void Start()
    {
        SetAlpha(0f);
        gameObject.SetActive(false);

        originalScale = transform.localScale;
        targetScale = originalScale;
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * duration);
    }

    private void SetAlpha(float alpha)
    {
        foreach (Renderer rend in characterRenderers)
        {
            if (rend != null && rend.material.HasProperty("_Color"))
            {
                Color color = rend.material.color;
                color.a = alpha;
                rend.material.color = color;
            }
        }
        foreach (CubismRenderer cubismRend in cubismRenderers)
        {
            if (cubismRend != null)
            {
                Color color = cubismRend.Color;
                color.a = alpha;
                cubismRend.Color = color;
            }
        }
    }


    private Sequence FadeRenderers(float targetAlpha, float duration, Ease easeType)
    {
        Sequence fadeSequence = DOTween.Sequence();
        foreach (Renderer rend in characterRenderers)
        {
            if (rend != null && rend.material.HasProperty("_Color"))
            {
                fadeSequence.Join(rend.material.DOFade(targetAlpha, duration).SetEase(easeType));
            }
        }
        foreach (CubismRenderer cubismRend in cubismRenderers)
        {
            if (cubismRend != null)
            {
                Color targetColor = cubismRend.Color;
                targetColor.a = targetAlpha;
                fadeSequence.Join(DOVirtual.Color(cubismRend.Color, targetColor, duration, (color) => {
                    if (cubismRend != null) cubismRend.Color = color;
                }).SetEase(easeType));
            }
        }
        return fadeSequence;
    }

    public void SetAnimationTrigger(string triggerName)
    {
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.SetTrigger(triggerName);
        }
    }

    public void Appear()
    {
        gameObject.SetActive(true);

        Sequence appearSequence = DOTween.Sequence();

        appearSequence.Append(FadeRenderers(1f, fadeDuration, fadeEaseType));


        appearSequence.OnComplete(() => {
            Debug.Log($"{gameObject.name} 등장");
        });

        appearSequence.Play();
    }

    public void Disappear()
    {
        Sequence disappearSequence = DOTween.Sequence();
        disappearSequence.Append(FadeRenderers(0f, fadeDuration, fadeEaseType));

        disappearSequence.OnComplete(() => {
            gameObject.SetActive(false);
            Debug.Log($"{gameObject.name} 사라짐");
        });
        disappearSequence.Play();
    }

    public void MoveLeft()
    {
        transform.DOMoveX(offscreenLeftX, moveDuration).SetEase(moveEaseType).OnComplete(() => {
            Debug.Log($"{gameObject.name} 왼쪽으로 이동");
        });
    }

    public void MoveRight()
    {
        transform.DOMoveX(offscreenRightX, moveDuration).SetEase(moveEaseType).OnComplete(() => {
            Debug.Log($"{gameObject.name} 오른쪽으로 이동");
        });
    }

    public void MoveToCenter()
    {
        transform.DOMoveX(originalPosition.x, moveDuration).SetEase(moveEaseType).OnComplete(() => {
            Debug.Log($"{gameObject.name} 중앙으로 이동");
        });
    }

    public void NotTalking()
    {
        targetScale = originalScale * scalePercent;

        var multiplyColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);

        foreach (var renderer in renderController.Renderers)
        {
            renderer.MultiplyColor = multiplyColor;
        }
    }

    public void Talking()
    {
        targetScale = originalScale;

        var multiplyColor = new Color(1f, 1f, 1f, 1f); // same as Color.white;

        foreach (var renderer in renderController.Renderers)
        {
            renderer.MultiplyColor = multiplyColor;
        }
    }
}