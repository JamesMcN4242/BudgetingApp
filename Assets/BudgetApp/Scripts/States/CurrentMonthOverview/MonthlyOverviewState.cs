////////////////////////////////////////////////////////////
/////   MonthlyOverviewState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

using static FlowMessageDefs;

public class MonthlyOverviewState : FlowStateBase
{
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
        m_ui = Object.FindObjectOfType<UIMonthlyOverview>();
        return m_ui != null;
    }
}
