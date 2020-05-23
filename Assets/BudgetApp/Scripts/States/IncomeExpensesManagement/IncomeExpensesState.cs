////////////////////////////////////////////////////////////
/////   UIIncomeExpenses.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class IncomeExpensesState : FlowStateBase
{
    private readonly string k_incomeExpensesKey;

    private UIIncomeExpenses m_uiIncomeExpenses = null;
    private GridElements m_gridElements;

    public IncomeExpensesState(string incomeExpensesKey)
    {
        k_incomeExpensesKey = incomeExpensesKey;
        string elementJson = PlayerPrefs.GetString(k_incomeExpensesKey, string.Empty);
        if(!string.IsNullOrEmpty(elementJson))
        {
            m_gridElements = JsonUtility.FromJson<GridElements>(elementJson);
        }
    }

    protected override void StartPresentingState()
    {
        m_uiIncomeExpenses.BuildGridElements(m_gridElements.m_elements);
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case "back":
                ControllingStateStack.PopState(this);
                break;

            case string msg when msg.StartsWith("remove"):
                break;

            case string msg when msg == "add":
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiIncomeExpenses = Object.FindObjectOfType<UIIncomeExpenses>();
        m_ui = m_uiIncomeExpenses;
        return m_ui != null;
    }
}
