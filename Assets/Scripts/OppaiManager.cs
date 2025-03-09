using UnityEngine;

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
                animePattern = ChangeAnimation(animePattern);//�A�j����pattern��ς��鏈��
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
        Debug.Log("�������J�n");
    }

    void OnHolding()
    {
        Debug.Log("��������...");
        sleepManager.CalSleepDeepOppai(animePattern, whoChara);
        ecstasyManager.CalEcstasy(animePattern,whoChara);
    }

    void OnHoldEnd()
    {
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
        int nextpattern = (pattern + 1) % 5;
        Debug.Log($"�A�j���p�^�[��{nextpattern}");
        return nextpattern;
    }
    public int GetChara()
    {
        return whoChara;
    }
}
