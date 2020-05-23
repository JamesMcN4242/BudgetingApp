////////////////////////////////////////////////////////////
/////   BaseMenuState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

using static PlayerPrefDefs;

public class BaseMenuState : FlowStateBase
{
    private const string k_toMonthOverview = "monthlyOverview";
    private const string k_setFixedValues = "setFixedValues";
    private const string k_addVariableValue = "addVariableValue";

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_toMonthOverview:
                ControllingStateStack.PushState(new MonthlyOverviewState());
                break;
                
            case k_setFixedValues:
                ControllingStateStack.PushState(new IncomeExpensesState(k_fixedValuesKey, false));
                break;
                
            case k_addVariableValue:
                ControllingStateStack.PushState(new IncomeExpensesState(k_variableValuesKey, true));                
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_ui = Object.FindObjectOfType<UIBaseMenu>();
        return m_ui != null;
    }
}
