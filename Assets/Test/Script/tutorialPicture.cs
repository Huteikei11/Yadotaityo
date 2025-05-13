using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class tutorialPicture : MonoBehaviour
{
    public Sprite[] page; //ページ数
    SpriteRenderer sprite;
    int pageNow = 0;

    // Start is called before the first frame update

    public bool pageTurn()
    {
        if (Input.GetMouseButtonDown(0))//左クリック
        {
            pageNow++;
            if (pageNow >= page.Length)
            {
                return false;
            }
        }else if(Input.GetMouseButtonDown(1))//右クリック
        {
            pageNow--;
            if (pageNow < 0)
            {
                return false;
            }
        }
        pageDisplay();
        return true;
    }
    public void pageDisplay()
    {
        if (sprite.sprite != page[pageNow])
        {
            sprite.sprite = page[pageNow];
        }
    }

    public void pageReset()
    {
        sprite = GetComponent<SpriteRenderer>();
        pageNow = 0;
        pageDisplay();
    }
}
