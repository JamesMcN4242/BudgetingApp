////////////////////////////////////////////////////////////
/////   UISettings.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using TMPro;

public class UISettings : UIStateBase
{
    private TMP_Dropdown m_languageDropDown = null;

    void Start()
    {
        m_languageDropDown = gameObject.GetComponentFromChild<TMP_Dropdown>("LanguageDrop");
    }

    public void SetDropdownValues(List<string> options, int currentLangIndex)
    {
        m_languageDropDown.ClearOptions();
        m_languageDropDown.AddOptions(options);
        m_languageDropDown.SetValueWithoutNotify(currentLangIndex);
    }
}
