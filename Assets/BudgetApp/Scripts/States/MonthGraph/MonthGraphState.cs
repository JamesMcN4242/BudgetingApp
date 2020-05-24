////////////////////////////////////////////////////////////
/////   MonthGraphState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

using static FlowMessageDefs;
using static PlayerPrefDefs;

public class MonthGraphState : FlowStateBase
{
    public enum ShowStates
    {
        INCOME = 0,
        EXPENSES,
        REMAINING
    }

    private const int k_intialMonthRange = 3;
    private const string k_setTimeScaleMsg = "setTimeScale";
    private const string k_showStartingMsg = "show";
    private const string k_showIncomeMsg = k_showStartingMsg + "Income_";
    private const string k_showExpensesMsg = k_showStartingMsg + "Expenses_";
    private const string k_showRemainingMsg = k_showStartingMsg + "Remaining_";

    private readonly string k_currentMonthId;

    private UIMonthGraph m_uiGraph = null;
    private List<MonthlyValueData> m_monthsToDisplay = null;
    private PreviousMonthlyValues m_previousMonths = default;
    private bool[] m_showingToggles = new bool[3] { true, true, true };

    public MonthGraphState()
    {
        k_currentMonthId = string.Format(PlayerPrefDefs.k_monthTrackingFormat, DateTime.Now.Month, DateTime.Now.Year);
        LoadPreviousMonths();

        m_monthsToDisplay = new List<MonthlyValueData>(m_previousMonths.monthlyValues.Count);
        LoadInitialDataRange();
    }

    protected override void StartPresentingState()
    {
        m_uiGraph.SetToggleValues(m_showingToggles);
        RefreshGraph();
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_setTimeScaleMsg:
                MonthlyValueData[] allValueRanges = new MonthlyValueData[m_previousMonths.monthlyValues.Count + 1];
                m_previousMonths.monthlyValues.CopyTo(allValueRanges);
                allValueRanges[allValueRanges.Length - 1] = MonthDataUtils.BuildCurrentMonthData();

                string[] ranges = Array.ConvertAll(allValueRanges, (element) => element.m_monthReflected);
                var timeState = new TimeScalePopupState(ranges, m_monthsToDisplay[0].m_monthReflected, 
                    m_monthsToDisplay[m_monthsToDisplay.Count - 1].m_monthReflected, LoadDataRangeAndRefreshGraph);
                ControllingStateStack.PushState(timeState);
                break;

            case k_backMenuMsg:
                ControllingStateStack.PopState(this);
                break;

            case string msg when msg.StartsWith(k_showStartingMsg):
                SetToggleStateFromMsg(msg);
                RefreshGraph();
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiGraph = GameObject.FindObjectOfType<UIMonthGraph>();
        m_ui = m_uiGraph;
        return m_ui != null;
    }

    private void SetToggleStateFromMsg(string msg)
    {
        if (msg.StartsWith(k_showIncomeMsg))
        {
            SetToggleState(msg, k_showIncomeMsg, ShowStates.INCOME);
        }
        else if (msg.StartsWith(k_showExpensesMsg))
        {
            SetToggleState(msg, k_showExpensesMsg, ShowStates.EXPENSES);
        }
        else if (msg.StartsWith(k_showRemainingMsg))
        {
            SetToggleState(msg, k_showRemainingMsg, ShowStates.REMAINING);
        }
    }

    private void SetToggleState(string msg, in string msgToRemove, in ShowStates showState)
    {
        msg = msg.Remove(0, msgToRemove.Length);
        m_showingToggles[(int)showState] = bool.Parse(msg);
    }

    private void RefreshGraph()
    {
        MonthlyValueData[] monthDatas = m_monthsToDisplay.ToArray();
        m_uiGraph.SetMonthData(monthDatas, GetHighestDisplayedValue(monthDatas));
    }

    private float GetHighestDisplayedValue(MonthlyValueData[] monthlyValues)
    {
        float maxValue = 0.0f;

        for(int i = 0; i < monthlyValues.Length; i++)
        {
            maxValue = GetMaxValueIfAllowed(maxValue, monthlyValues[i].TotalIncome, ShowStates.INCOME);
            maxValue = GetMaxValueIfAllowed(maxValue, -1f * monthlyValues[i].TotalExpenses, ShowStates.EXPENSES);
            maxValue = GetMaxValueIfAllowed(maxValue, Mathf.Abs(monthlyValues[i].MonthlyBalanceRemaining), ShowStates.REMAINING);
        }

        return maxValue;
    }

    private float GetMaxValueIfAllowed(float currentMax, float comparedVal, ShowStates showState)
    {
        return m_showingToggles[(int)showState] ? Mathf.Max(currentMax, comparedVal) : currentMax;
    }

    private void LoadInitialDataRange()
    {
        string startingId = k_currentMonthId;
        if(m_previousMonths.monthlyValues.Count > 0)
        {
            int index = Mathf.Max(m_previousMonths.monthlyValues.Count - k_intialMonthRange, 0);
            startingId = m_previousMonths.monthlyValues[index].m_monthReflected;
        }
        LoadDataRange(startingId, k_currentMonthId);
    }

    private void LoadDataRangeAndRefreshGraph(string identifierBegin, string identifierEnd)
    {
        LoadDataRange(identifierBegin, identifierEnd);
        RefreshGraph();
    }

    private void LoadDataRange(string identifierBegin, string identifierEnd)
    {
        m_monthsToDisplay.Clear();

        int index = m_previousMonths.monthlyValues.FindIndex((e) => e.m_monthReflected == identifierBegin);
        if (index >= 0)
        {
            for (; index < m_previousMonths.monthlyValues.Count; index++)
            {
                m_monthsToDisplay.Add(m_previousMonths.monthlyValues[index]);
                if (identifierEnd == m_previousMonths.monthlyValues[index].m_monthReflected)
                {
                    return;
                }
            }
        }

        if(identifierEnd == k_currentMonthId)
        {
            m_monthsToDisplay.Add(MonthDataUtils.BuildCurrentMonthData());
        }
    }

    private void LoadPreviousMonths()
    {
        string json = PlayerPrefs.GetString(k_monthCollectionKey, string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            m_previousMonths = JsonUtility.FromJson<PreviousMonthlyValues>(json);
        }
        else
        {
            m_previousMonths.monthlyValues = new List<MonthlyValueData>(0);
        }
    }
}
