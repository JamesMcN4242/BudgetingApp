////////////////////////////////////////////////////////////
/////   BaseMenuState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class BaseMenuState : FlowStateBase
{
    private const string k_toMonthOverview = "currentMonthOverview";

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case string msg:
            {
                if(msg == k_toMonthOverview)
                {
                    ControllingStateStack.PushState(new CurrentMonthOverviewState());
                }
                break;
            }
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_ui = Object.FindObjectOfType<UIBaseMenu>();
        return m_ui != null;
    }
}
