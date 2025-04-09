using UnityEngine;
using DG.Tweening;

public class GameoverSprite : MonoBehaviour
{
    [Header("動かすSpriteのTransform")]
    public Transform sprite1;
    public Transform sprite2;

    [Header("開始位置")]
    public Vector3 startPos1;
    public Vector3 startPos2;

    [Header("終了位置（停止位置）")]
    public Vector3 endPos1;
    public Vector3 endPos2;

    [Header("移動時間")]
    public float moveDuration = 1.5f;

    void Start()
    {
        // 開始位置に設定
        sprite1.position = startPos1;
        sprite2.position = startPos2;
    }
    public void gameover()
    {
        // DOTweenで目的地に移動
        sprite1.DOMove(endPos1, moveDuration);
        sprite2.DOMove(endPos2, moveDuration);
    }
}
