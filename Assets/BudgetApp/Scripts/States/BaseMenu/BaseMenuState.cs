////////////////////////////////////////////////////////////
/////   BaseMenuState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

using static PlayerPrefDefs;

public class BaseMenuState : FlowStateBase
{
    private const string k_toMonthOverview = "monthlyOverview";
    private const string k_setFixedValues = "setFixedValues";
    private const string k_addVariableValue = "addVariableValue";
    private const string k_monthlyGraphMsg = "monthlyGraph";
    private const string k_settingsMsg = "settings";

    private LocalisationService m_localisationService = null;

    public BaseMenuState(LocalisationService locService)
    {
        //TODO: Localise values
        m_localisationService = locService;
    }

    protected override void StartPresentingState()
    {
        string monthTrackedPreviously = PlayerPrefs.GetString(k_monthTrackingKey, string.Empty);
        if(string.IsNullOrEmpty(monthTrackedPreviously))
        {
            PlayerPrefs.SetString(k_monthTrackingKey, string.Format(k_monthTrackingFormat, DateTime.Now.Month, DateTime.Now.Year));
            PlayerPrefs.Save();
            return;
        }
        
        if(string.Format(k_monthTrackingFormat, DateTime.Now.Month, DateTime.Now.Year) != monthTrackedPreviously)
        {
            //TODO: Allow last additions to month before saving
            MonthlyValueData monthData = MonthDataUtils.BuildCurrentMonthData();
            monthData.m_monthReflected = monthTrackedPreviously;

            string previousJson = PlayerPrefs.GetString(k_monthCollectionKey, string.Empty);
            PreviousMonthlyValues values = default;
            if(!string.IsNullOrEmpty(previousJson))
            {
                values = JsonUtility.FromJson<PreviousMonthlyValues>(previousJson);
            }
            
            if (values.monthlyValues == null) values.monthlyValues = new List<MonthlyValueData>();
            values.monthlyValues.Add(monthData);
            PlayerPrefs.SetString(k_monthCollectionKey, JsonUtility.ToJson(values));
            PlayerPrefs.SetString(k_monthTrackingKey, string.Format(k_monthTrackingFormat, DateTime.Now.Month, DateTime.Now.Year));
            PlayerPrefs.DeleteKey(k_variableValuesKey);
            PlayerPrefs.Save();
        }
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_toMonthOverview:
                ControllingStateStack.PushState(new MonthlyOverviewState());
                break;
                
            case k_setFixedValues:
                ControllingStateStack.PushState(new IncomeExpensesState(k_fixedValuesKey, false, m_localisationService));
                break;
                
            case k_addVariableValue:
                ControllingStateStack.PushState(new IncomeExpensesState(k_variableValuesKey, true, m_localisationService));                
                break;

            case k_monthlyGraphMsg:
                ControllingStateStack.PushState(new MonthGraphState());
                break;

            case k_settingsMsg:
                ControllingStateStack.PushState(new SettingsState(m_localisationService, RefreshTextLanguage));
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_ui = GameObject.FindObjectOfType<UIBaseMenu>();
        return m_ui != null;
    }

    private void RefreshTextLanguage()
    {

    }
}
