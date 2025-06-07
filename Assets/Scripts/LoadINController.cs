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
        yield return null; // 1フレーム待機
        yield return null; // 1フレーム待機
        yield return null; // 1フレーム待機
        Animator animator = GetComponent<Animator>();
        animator.Play("LoadIN");
    }

    void Update()
    {

    }
}