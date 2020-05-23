////////////////////////////////////////////////////////////
/////   MonthlyOverviewState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

using static FlowMessageDefs;

public class MonthlyOverviewState : FlowStateBase
{
    private UIMonthlyOverview m_uiOverview = null;

    protected override void StartPresentingState()
    {
        MonthlyValueData monthlyValue = MonthDataUtils.BuildCurrentMonthData();
        m_uiOverview.SetData(monthlyValue);
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_backMenuMsg:
                ControllingStateStack.PopState(this);
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiOverview = Object.FindObjectOfType<UIMonthlyOverview>();
        m_ui = m_uiOverview;
        return m_ui != null;
    }
}
