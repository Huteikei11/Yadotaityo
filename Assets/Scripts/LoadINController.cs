using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadINController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayAnimationDelayed());
    }

    private IEnumerator PlayAnimationDelayed()
    {
        yield return null; // 1�t���[���ҋ@
        yield return null; // 1�t���[���ҋ@
        yield return null; // 1�t���[���ҋ@
        Animator animator = GetComponent<Animator>();
        animator.Play("LoadIN");
    }

    void Update()
    {

    }
}