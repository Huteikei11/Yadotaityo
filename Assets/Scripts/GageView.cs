using TMPro;
using UnityEngine;

public class GageView : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // �ړ�������I�u�W�F�N�g
    [SerializeField] private float minY = 0f; // �ŏ�Y���W
    [SerializeField] private float maxY = 10f; // �ő�Y���W
    [SerializeField, Range(0f, 100f)] private float value = 0f; // 0~100�̒l
    [SerializeField] private EcstasyManager ecstasy;
    [SerializeField] private TextMeshProUGUI textMeshPro; // �\���p��TextMeshPro

    private void Update()
    {
        if (targetObject != null)
        {
            value = ecstasy.ecstacyGage;
            float newY = Mathf.Lerp(minY, maxY, value / 100f); // 0~100�̒l��Y���W�ɕϊ�
            targetObject.position = new Vector3(targetObject.position.x, newY, targetObject.position.z);
        }
        if (textMeshPro != null)
        {
            int intValue = Mathf.FloorToInt(value); // �����_�ȉ���؂�̂�
            textMeshPro.text = intValue.ToString(); // ������ɕϊ����ĕ\��
        }
    }
}
