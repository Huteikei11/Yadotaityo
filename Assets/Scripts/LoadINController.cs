using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadINController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("LoadIN"); // ���̎��_�ŃA�j���[�V�����Đ��𐧌�ł���
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
