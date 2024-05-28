using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
[RequireComponent(typeof(AsToggle))]
public class ToggleEx : MonoBehaviour
{
    public GameObject backGround;
    public GameObject checkMark;
    public TextMeshProUGUI text;
    public Color normal = Color.gray;
    public Color highLight = Color.white;

    private AsToggle toggle;

    void Awake()
    {
        toggle = transform.GetComponent<AsToggle>();
        if (toggle == null)
            return;
        OnValueChanged(toggle.isOn);
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool isOn)
    {
        if (backGround != null)
        {
            backGround.SetActive(!isOn);
        }

        if (checkMark != null)
        {
            checkMark.SetActive(isOn);
        }

        if (text != null)
        {
            text.color = isOn ? highLight : normal;
        }
    }
}
