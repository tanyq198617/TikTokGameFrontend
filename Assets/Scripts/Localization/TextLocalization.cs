using UnityEngine;
using TMPro;

public class TextLocalization : MonoBehaviour
{
    public string stringKey;
    private TextMeshProUGUI mText;

    void Start()
    {
        mText = gameObject.GetComponent<TextMeshProUGUI>();
        if (Application.isPlaying && stringKey.Length > 0)
        {
            LocalizationMgr.Instance.AddText(this);
            SetText();
        }
    }

    public void SetText()
    {
        string str = LocalizationMgr.Instance.GetString(stringKey);
        if (string.IsNullOrEmpty(str))
            return;
        mText.text = str;
    }
}
