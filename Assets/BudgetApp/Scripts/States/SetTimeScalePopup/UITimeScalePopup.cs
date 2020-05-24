////////////////////////////////////////////////////////////
/////   UITimeScalePopup.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
using System.Collections.Generic;
using TMPro;

public class UITimeScalePopup : UIStateBase
{
    private TMP_Dropdown m_fromDate = null;
    private TMP_Dropdown m_toDate = null;

    void Start()
    {
        m_fromDate = gameObject.GetComponentFromChild<TMP_Dropdown>("FromDrop");
        m_toDate = gameObject.GetComponentFromChild<TMP_Dropdown>("ToDrop");
    }

    public void SetDropdownValues(string[] options, int fromIndex, int toIndex)
    {
        m_fromDate.ClearOptions();
        m_toDate.ClearOptions();

        m_fromDate.AddOptions(new List<string>(options));
        List<string> toDates = new List<string>(options.Length - fromIndex);
        for(int i = 0; i < toDates.Capacity; i++)
        {
            toDates.Add(options[i + fromIndex]);
        }
        m_toDate.AddOptions(toDates);

        m_fromDate.SetValueWithoutNotify(fromIndex);
        m_toDate.SetValueWithoutNotify(toIndex - fromIndex);
    }

    public (string from, string to) GetDropdownValues()
    {
        return (m_fromDate.options[m_fromDate.value].text, m_toDate.options[m_toDate.value].text);
    }
}
