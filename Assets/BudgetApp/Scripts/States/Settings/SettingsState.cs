////////////////////////////////////////////////////////////
/////   SettingsState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using UnityEngine;

using static FlowMessageDefs;
using static PlayerPrefDefs;
using static ResourceDefs;
using static System.Convert;

public class SettingsState : FlowStateBase
{
    private const string k_clearDataMsg = "clearData";
    private const string k_languageDropMsg = "language_";

    private UISettings m_uiSettings = null;

    protected override void StartPresentingState()
    {
        List<string> languages = new List<string>((int)LocalisationService.LocalisableCultures.COUNT);
        for(int i = 0; i < languages.Capacity; i++)
        {
            languages.Add(((LocalisationService.LocalisableCultures)i).ToString());
        }
        m_uiSettings.SetDropdownValues(languages, PlayerPrefs.GetInt(k_localisationIndexKey, 0));
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_backMenuMsg:
                ControllingStateStack.PopState(this);
                break;

            case k_clearDataMsg:
                //TODO: Confirmation popup
                ClearSavedData();
                break;

            case string langMsg when langMsg.StartsWith(k_languageDropMsg):
                //TODO: Refresh all UI
                langMsg = langMsg.Remove(0, k_languageDropMsg.Length);
                int index = ToInt32(langMsg);

                LocalisationService locService = Object.FindObjectOfType<MenuSceneDirector>().LocalisationService;
                locService.LoadLocalisation((LocalisationService.LocalisableCultures)index, k_languageResourceFormat);
                PlayerPrefs.SetInt(k_localisationIndexKey, index);
                PlayerPrefs.Save();
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiSettings = Object.FindObjectOfType<UISettings>();
        m_ui = m_uiSettings;
        return m_ui != null;
    }

    private void ClearSavedData()
    {
        PlayerPrefs.DeleteKey(k_fixedValuesKey);
        PlayerPrefs.DeleteKey(k_variableValuesKey);
        PlayerPrefs.DeleteKey(k_monthCollectionKey);
        PlayerPrefs.Save();
    }
}
