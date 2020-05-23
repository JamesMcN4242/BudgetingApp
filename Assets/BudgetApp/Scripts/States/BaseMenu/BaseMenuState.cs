////////////////////////////////////////////////////////////
/////   BaseMenuState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class BaseMenuState : FlowStateBase
{
    private const string k_toMonthOverview = "monthlyOverview";
    private const string k_setFixedValues = "setFixedValues";
    private const string k_addVariableValue = "addVariableValue";

    private const string k_fixedValuesKey = "FixedValues";
    private const string k_variableValuesKey = "VariableValues";

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case string msg:
            {
                if(msg == k_toMonthOverview)
                {
                    ControllingStateStack.PushState(new MonthlyOverviewState());
                }
                else if (msg == k_setFixedValues)
                {
                    ControllingStateStack.PushState(new IncomeExpensesState(k_fixedValuesKey));
                }
                else if (msg == k_addVariableValue)
                {
                    ControllingStateStack.PushState(new IncomeExpensesState(k_variableValuesKey));
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
