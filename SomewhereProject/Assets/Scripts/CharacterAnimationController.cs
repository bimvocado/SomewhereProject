using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationTrigger(string triggerName)
    {
        if (string.IsNullOrEmpty(triggerName)) return;

        animator.SetTrigger(triggerName);
    }
}