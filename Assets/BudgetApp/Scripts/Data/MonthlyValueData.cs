////////////////////////////////////////////////////////////
/////   MonthlyValueData.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonthlyValueData
{
    public DateTime m_monthReflected;
    public float m_totalFixedIncome;
    public float m_totalFixedExpenses;
    public float m_totalVariableIncome;
    public float m_totalVariableExpenses;

    public float TotalIncome => m_totalFixedIncome + m_totalVariableIncome;
    public float TotalExpenses => m_totalFixedExpenses + m_totalVariableExpenses;
    public float MonthlyBalanceRemaining => TotalIncome + TotalExpenses;
}

[Serializable]
public struct PreviousMonthlyValues
{
    public List<MonthlyValueData> monthlyValues;
}

public static class MonthDataUtils
{
    public static MonthlyValueData BuildCurrentMonthData()
    {
        GridElements[] gridElements = new GridElements[2];
        gridElements[0] = GetElements(PlayerPrefDefs.k_fixedValuesKey);
        gridElements[1] = GetElements(PlayerPrefDefs.k_variableValuesKey);

        var fixedValues = TotalValues(gridElements[0].m_elements);
        var variableValues = TotalValues(gridElements[1].m_elements);

        return new MonthlyValueData
        {
            m_monthReflected = DateTime.Now,
            m_totalFixedIncome = fixedValues.income,
            m_totalFixedExpenses = fixedValues.expenses,
            m_totalVariableIncome = variableValues.income,
            m_totalVariableExpenses = variableValues.expenses
        };
    }

    private static GridElements GetElements(string playerPrefKey)
    {
        GridElements gridElements = default;
        string elementJson = PlayerPrefs.GetString(playerPrefKey, string.Empty);
        if (!string.IsNullOrEmpty(elementJson))
        {
            gridElements = JsonUtility.FromJson<GridElements>(elementJson);
        }
        return gridElements;
    }

    private static (float income, float expenses) TotalValues(List<GridElementData> elements)
    {
        float income = 0.0f;
        float expenses = 0.0f;

        if (elements != null)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].IsExpense)
                {
                    expenses += elements[i].m_variableValue;
                }
                else
                {
                    income += elements[i].m_variableValue;
                }
            }
        }

        return (income, expenses);
    }
}