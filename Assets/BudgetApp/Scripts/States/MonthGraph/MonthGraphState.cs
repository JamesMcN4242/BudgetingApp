////////////////////////////////////////////////////////////
/////   MonthGraphState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

using static FlowMessageDefs;

public class MonthGraphState : FlowStateBase
{
    private UIMonthGraph m_uiGraph = null;

    protected override void StartPresentingState()
    {
        //TODO: Switch to range of months
        m_uiGraph.SetMonthData(new []{ MonthDataUtils.BuildCurrentMonthData(), MonthDataUtils.BuildCurrentMonthData() , MonthDataUtils.BuildCurrentMonthData() , MonthDataUtils.BuildCurrentMonthData() , MonthDataUtils.BuildCurrentMonthData() });
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_backMenuMsg:
                ControllingStateStack.PopState(this);
                break;

            case string msg when msg.StartsWith("show"):
                m_uiGraph.SetMonthData(new[] { MonthDataUtils.BuildCurrentMonthData() , MonthDataUtils.BuildCurrentMonthData() , MonthDataUtils.BuildCurrentMonthData() , MonthDataUtils.BuildCurrentMonthData() , MonthDataUtils.BuildCurrentMonthData() });
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiGraph = GameObject.FindObjectOfType<UIMonthGraph>();
        m_ui = m_uiGraph;
        return m_ui != null;
    }
}
