
using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text infoText;

    /// <summary>
    /// 텍스트를 설정하는 함수입니다.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="info"></param>
    public void SetText(string title, string info)
    {
        titleText.text = title;
        infoText.text = info;
    }

    public void SetText(InfoData data)
    {
        titleText.text = data.title;
        infoText.text = data.info;
    }
}
