using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationAndDisappear : MonoBehaviour
{
    private Animator animator;
    public string animationName;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(animationName);
        StartCoroutine(DisappearAfterAnimation());
    }

    private IEnumerator DisappearAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
