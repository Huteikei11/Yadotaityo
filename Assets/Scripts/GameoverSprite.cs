using UnityEngine;
using DG.Tweening;

public class GameoverSprite : MonoBehaviour
{
    [Header("������Sprite��Transform")]
    public Transform sprite1;
    public Transform sprite2;

    [Header("�J�n�ʒu")]
    public Vector3 startPos1;
    public Vector3 startPos2;

    [Header("�I���ʒu�i��~�ʒu�j")]
    public Vector3 endPos1;
    public Vector3 endPos2;

    [Header("�ړ�����")]
    public float moveDuration = 1.5f;

    void Start()
    {
        // �J�n�ʒu�ɐݒ�
        sprite1.position = startPos1;
        sprite2.position = startPos2;
    }
    public void gameover()
    {
        // DOTween�ŖړI�n�Ɉړ�
        sprite1.DOMove(endPos1, moveDuration);
        sprite2.DOMove(endPos2, moveDuration);
    }
}
