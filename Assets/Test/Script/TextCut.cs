using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextCut : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Animator anim;
    public Sprite Start;
    public Sprite Failed;
    public Sprite Success;

    // Update is called once per frame
    public void CutScene(Sprite cutText,bool miss)
    {
        anim.SetTrigger("Open");
        image.sprite = cutText;
        anim.SetBool("FullScreen",miss);
        image.gameObject.GetComponent<Animator>().SetTrigger("textTrigger");
    }

}
