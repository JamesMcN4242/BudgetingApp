////////////////////////////////////////////////////////////
/////   UIAddOrEditElement.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using TMPro;

public class UIAddOrEditElement : UIStateBase
{
    private TextMeshProUGUI m_titleText = null;
    private TMP_InputField m_nameInputField = null;
    private TMP_InputField m_valueInputField = null;

    void Start()
    {
        m_titleText = gameObject.GetComponentFromChild<TextMeshProUGUI>("Title");
        m_nameInputField = gameObject.GetComponentFromChild<TMP_InputField>("NameField");
        m_valueInputField = gameObject.GetComponentFromChild<TMP_InputField>("CostField");
    }

    public void SetTitle(string titleText)
    {
        m_titleText.text = titleText;
    }

    public void SetStartingInputs(string name, string value)
    {
        m_nameInputField.text = name;
        m_valueInputField.text = value;
    }

    public (string name, string value) GetInputFieldContent()
    {
        return (m_nameInputField.text, m_valueInputField.text);
    }
}
