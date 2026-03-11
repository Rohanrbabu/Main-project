using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] Text legacyText;
    [SerializeField] string prefix = "";

    Component tmpTextComponent;
    PropertyInfo tmpTextProperty;

    void Awake()
    {
        if (legacyText == null)
        {
            legacyText = GetComponent<Text>();
        }

        if (tmpTextComponent == null)
        {
            var tmpTextType = System.Type.GetType("TMPro.TMP_Text, Unity.TextMeshPro");
            if (tmpTextType != null)
            {
                tmpTextComponent = GetComponent(tmpTextType);
                if (tmpTextComponent != null)
                {
                    tmpTextProperty = tmpTextType.GetProperty("text", BindingFlags.Public | BindingFlags.Instance);
                }
            }
        }
    }

    void Update()
    {
        if (ScoreManager.Instance == null)
        {
            return;
        }

        string value = prefix + ScoreManager.Instance.Score.ToString();

        if (legacyText != null)
        {
            legacyText.text = value;
        }

        if (tmpTextComponent != null && tmpTextProperty != null)
        {
            tmpTextProperty.SetValue(tmpTextComponent, value);
        }
    }
}
