////////////////////////////////////////////////////////////
/////   MonthGraphState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

using static FlowMessageDefs;

public class MonthGraphState : FlowStateBase
{
    public enum ShowStates
    {
        INCOME = 0,
        EXPENSES,
        REMAINING
    }

    private const string k_showStartingMsg = "show";
    private const string k_showIncomeMsg = k_showStartingMsg + "Income_";
    private const string k_showExpensesMsg = k_showStartingMsg + "Expenses_";
    private const string k_showRemainingMsg = k_showStartingMsg + "Remaining_";

    private UIMonthGraph m_uiGraph = null;
    private bool[] m_showingToggles = new bool[3] { true, true, true };

    protected override void StartPresentingState()
    {
        m_uiGraph.SetToggleValues(m_showingToggles);
        RefreshGraph();
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
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
        //TODO: Switch to range of months instead of debug values
        MonthlyValueData currentMonth = MonthDataUtils.BuildCurrentMonthData();
        MonthlyValueData[] monthDatas = new[] { currentMonth, currentMonth, currentMonth, currentMonth };
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
}
