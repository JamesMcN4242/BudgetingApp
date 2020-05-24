////////////////////////////////////////////////////////////
/////   UIMonthGraph.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static System.Convert;

public class UIMonthGraph : UIStateBase
{
    private const float k_graphSlotSpacing = 0.03f;

    private TextMeshProUGUI m_topVal = null;
    private TextMeshProUGUI m_midVal = null;
    private TextMeshProUGUI m_quartVal = null;
    private TextMeshProUGUI m_bottomVal = null;

    private Toggle m_showIncomeToggle = null;
    private Toggle m_showExpensesToggle = null;
    private Toggle m_showRemainingToggle = null;

    private Transform m_graphContent = null;

    void Start()
    {
        m_topVal = gameObject.GetComponentFromChild<TextMeshProUGUI>("TopValue");
        m_midVal = gameObject.GetComponentFromChild<TextMeshProUGUI>("MidValue");
        m_quartVal = gameObject.GetComponentFromChild<TextMeshProUGUI>("QuarterValue");
        m_bottomVal = gameObject.GetComponentFromChild<TextMeshProUGUI>("BottomValue");

        m_showIncomeToggle = gameObject.FindChildByName("ShowTotalIncome").GetComponentInChildren<Toggle>();
        m_showExpensesToggle = gameObject.FindChildByName("ShowTotalExpenses").GetComponentInChildren<Toggle>();
        m_showRemainingToggle = gameObject.FindChildByName("ShowTotalRemaining").GetComponentInChildren<Toggle>();

        m_graphContent = gameObject.FindChildByName("GraphContent").transform;
    }

    public void SetMonthData(MonthlyValueData[] monthlyValues, float highestValue)
    {
        m_graphContent.DestroyAllChildren();

        float xSize = 1.0f / monthlyValues.Length - k_graphSlotSpacing;
        float startingX = 0.0f;
        GameObject graphSlotPrefab = Resources.Load<GameObject>("UIGraphSlot");

        for(int i = 0; i < monthlyValues.Length; i++)
        {
            RectTransform slot = Object.Instantiate(graphSlotPrefab, m_graphContent).GetComponent<RectTransform>();

            //Set full slot size
            slot.anchorMin = new Vector2(startingX, 0.0f);
            slot.anchorMax = new Vector2(startingX + xSize, 1.0f);
            startingX += xSize + k_graphSlotSpacing;

            RectTransform incomeBar = slot.gameObject.GetComponentFromChild<RectTransform>("TotalIncome");
            RectTransform expensesBar = slot.gameObject.GetComponentFromChild<RectTransform>("TotalExpenses");
            RectTransform leftOverBar = slot.gameObject.GetComponentFromChild<RectTransform>("MonthlyLeftOver");
            incomeBar.gameObject.SetActive(m_showIncomeToggle.isOn);
            expensesBar.gameObject.SetActive(m_showExpensesToggle.isOn);
            leftOverBar.gameObject.SetActive(m_showRemainingToggle.isOn);

            float barOnStartPos = 0.0f;
            float barSize = 1.0f / GetBarsActive();
            if(m_showIncomeToggle.isOn)
            {
                incomeBar.anchorMin = new Vector2(barOnStartPos, incomeBar.anchorMin.y);
                incomeBar.anchorMax = new Vector2(barOnStartPos + barSize, monthlyValues[i].TotalIncome / highestValue);
                barOnStartPos += barSize;
            }
            if (m_showExpensesToggle.isOn)
            {
                expensesBar.anchorMin = new Vector2(barOnStartPos, incomeBar.anchorMin.y);
                expensesBar.anchorMax = new Vector2(barOnStartPos + barSize, -1f * monthlyValues[i].TotalExpenses / highestValue);
                barOnStartPos += barSize;
            }
            if (m_showRemainingToggle.isOn)
            {
                leftOverBar.anchorMin = new Vector2(barOnStartPos, incomeBar.anchorMin.y);
                leftOverBar.anchorMax = new Vector2(barOnStartPos + barSize, Mathf.Abs(monthlyValues[i].MonthlyBalanceRemaining) / highestValue);
            }

            //TODO: Month Desc of the slots
        }
    }

    public void SetToggleValues(bool[] toggleStates)
    {
        m_showIncomeToggle.SetIsOnWithoutNotify(toggleStates[(int)MonthGraphState.ShowStates.INCOME]);
        m_showExpensesToggle.SetIsOnWithoutNotify(toggleStates[(int)MonthGraphState.ShowStates.EXPENSES]);
        m_showRemainingToggle.SetIsOnWithoutNotify(toggleStates[(int)MonthGraphState.ShowStates.REMAINING]);
    }

    private int GetBarsActive()
    {
        return ToInt32(m_showIncomeToggle.isOn) + ToInt32(m_showExpensesToggle.isOn) + ToInt32(m_showRemainingToggle.isOn);
    }
}
