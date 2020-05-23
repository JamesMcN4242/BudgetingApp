////////////////////////////////////////////////////////////
/////   CurrentMonthOverviewState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class CurrentMonthOverviewState : FlowStateBase
{
    private const string k_backMessage = "back";

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case string msg when msg == k_backMessage:
                ControllingStateStack.PopState(this);
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_ui = Object.FindObjectOfType<UICurrentMonthOverview>();
        return m_ui != null;
    }
}
