////////////////////////////////////////////////////////////
/////   SettingsState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
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
    private LocalisationService m_locService = null;
    private Action m_refreshBaseText = null;

    public SettingsState(LocalisationService localisationService, Action refreshUIText)
    {
        m_locService = localisationService;
        m_refreshBaseText = refreshUIText;
    }

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
                var popupText = new ConfirmationPopupState.PopupText
                {
                    m_title = m_locService.GetLocalised("CONFIRMATION_TITLE"),
                    m_description = m_locService.GetLocalised("DELETE_CONFIRM_DESC"),
                    m_accept = m_locService.GetLocalised("CONTINUE"),
                    m_decline = m_locService.GetLocalised("CANCEL")
                };
                ControllingStateStack.PushState(new ConfirmationPopupState(popupText, ClearSavedData));
                break;

            case string langMsg when langMsg.StartsWith(k_languageDropMsg):
                langMsg = langMsg.Remove(0, k_languageDropMsg.Length);
                int index = ToInt32(langMsg);
                
                m_locService.LoadLocalisation((LocalisationService.LocalisableCultures)index, k_languageResourceFormat);
                PlayerPrefs.SetInt(k_localisationIndexKey, index);
                PlayerPrefs.Save();

                //TODO: Refresh this and base UI
                m_refreshBaseText?.Invoke();
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiSettings = GameObject.FindObjectOfType<UISettings>();
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
