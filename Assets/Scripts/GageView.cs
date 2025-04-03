using TMPro;
using UnityEngine;

public class GageView : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // 移動させるオブジェクト
    [SerializeField] private float minY = 0f; // 最小Y座標
    [SerializeField] private float maxY = 10f; // 最大Y座標
    [SerializeField, Range(0f, 100f)] private float value = 0f; // 0~100の値
    [SerializeField] private EcstasyManager ecstasy;
    [SerializeField] private TextMeshProUGUI textMeshPro; // 表示用のTextMeshPro

    private void Update()
    {
        if (targetObject != null)
        {
            value = ecstasy.ecstacyGage;
            float newY = Mathf.Lerp(minY, maxY, value / 100f); // 0~100の値をY座標に変換
            targetObject.position = new Vector3(targetObject.position.x, newY, targetObject.position.z);
        }
        if (textMeshPro != null)
        {
            int intValue = Mathf.FloorToInt(value); // 小数点以下を切り捨て
            textMeshPro.text = intValue.ToString(); // 文字列に変換して表示
        }
    }
}
