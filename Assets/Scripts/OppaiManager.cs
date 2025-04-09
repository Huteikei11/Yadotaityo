using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OppaiManager : MonoBehaviour
{
    public bool isTouch; //�����ς���G�鋖��
    private float pressTime = 0f;
    public bool isHolding = false; //�����ς���������Ă��邩
    public float holdThreshold = 0.5f; // �����������臒l�i�b�j
    public int animePattern;//
    public int whoChara; //�L�����N�^�[���N�Ȃ̂�
    [SerializeField] private SleepManager sleepManager;
    [SerializeField] private EcstasyManager ecstasyManager;
    public Animator anim;
    [SerializeField] private Image whiteScreen;

    private void Start()
    {
        whoChara = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        anim.SetInteger("difficult", whoChara);
    }
    void Update()
    {
        if (isTouch)
        {
            if (Input.GetMouseButtonDown(0))//���N���b�N
            {
                pressTime = Time.time; // �������u�Ԃ̎��Ԃ��L�^
            }

            if (Input.GetMouseButtonDown(1))//�E�N���b�N
            {
                StartCoroutine(RightClick());
            }

            if (Input.GetMouseButton(0))
            {
                float duration = Time.time - pressTime;
                if (duration >= holdThreshold)
                {
                    if (!isHolding)
                    {
                        isHolding = true;
                        OnHoldStart(); // �������J�n���̏���
                    }
                    OnHolding(); // ���������̏���
                }
                else
                {
                    NotHoding();
                }
            }
            else
            {
                NotHoding();//�����ς��G���ĂȂ��Ƃ��̏���
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isHolding)
                {
                    OnHoldEnd(); // �������I�����̏���
                    isHolding = false;
                }
                else
                {
                    OnClick(); // �ʏ�N���b�N����
                }
            }
        }
    }

    void OnClick()
    {
        Debug.Log("�ʏ�N���b�N");
    }

    void OnHoldStart()
    {
        anim.SetTrigger("Start");
        Debug.Log("�������J�n");
    }

    void OnHolding()
    {
        Debug.Log("��������...");
        sleepManager.CalSleepDeepOppai(animePattern, whoChara);
        ecstasyManager.CalEcstasy(animePattern, whoChara);
    }

    public void OnHoldEnd()
    {
        anim.SetTrigger("Close");
        Debug.Log("�������I��");
    }

    void NotHoding()
    {
        sleepManager.PlusSleepDeepNotHolding();
        ecstasyManager.AddEcstacy(-0.001f);//�ː��x������
    }

    public void StartOppai()//���������
    {
        isTouch = true;
    }
    public void StopOppai() //����������Ȃ�
    {
        isTouch = false;
    }

    private int ChangeAnimation(int pattern)
    {
        int nextpattern = (pattern + 1) % 4;
        Debug.Log($"�A�j���p�^�[��{nextpattern}");
        return nextpattern;
    }
    public int GetChara()
    {
        return whoChara;
    }

    private IEnumerator RightClick()
    {

        if (isHolding)
        {
            // �z���C�g�A�E�g�J�n
            whiteScreen.gameObject.SetActive(true);
            for (float t = 0; t < 1f; t += Time.deltaTime * 4)
            {
                whiteScreen.color = new Color(0.3098039f, 0.1882353f, 0.2980392f, t);
                yield return null;
            }
            animePattern = ChangeAnimation(animePattern);//�A�j����pattern��ς��鏈��
            anim.SetInteger("animePattern", animePattern);
            // �z���C�g�C���J�n�i��ʂ𔒂���t�F�[�h�A�E�g�j
            for (float t = 1f; t > 0; t -= Time.deltaTime * 4)
            {
                whiteScreen.color = new Color(0.3098039f, 0.1882353f, 0.2980392f, t);
                yield return null;
            }
        }

    }
}
