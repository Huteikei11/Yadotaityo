using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadINController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("LoadIN"); // この時点でアニメーション再生を制御できる
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
