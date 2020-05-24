////////////////////////////////////////////////////////////
/////   UIMonthGraph.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Globalization;
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
        LocalisationService locService = GameObject.FindObjectOfType<MenuSceneDirector>().LocalisationService; 
        m_graphContent.DestroyAllChildren();
        int activeBars = GetBarsActive();
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
            float barSize = 1.0f / activeBars;
            barOnStartPos = SetBarSizeAndText(m_showIncomeToggle.isOn, incomeBar, monthlyValues[i].TotalIncome, highestValue, barOnStartPos, barSize);
            barOnStartPos = SetBarSizeAndText(m_showExpensesToggle.isOn, expensesBar, -1f*monthlyValues[i].TotalExpenses, highestValue, barOnStartPos, barSize);
            SetBarSizeAndText(m_showRemainingToggle.isOn, leftOverBar, Mathf.Abs(monthlyValues[i].MonthlyBalanceRemaining), highestValue, barOnStartPos, barSize);
            
            TextMeshProUGUI monthText = slot.gameObject.GetComponentFromChild<TextMeshProUGUI>("MonthText");
            int indexOfSeperator = monthlyValues[i].m_monthReflected.IndexOf('_');
            string monthKey = monthlyValues[i].m_monthReflected.Remove(indexOfSeperator);
            string year = monthlyValues[i].m_monthReflected.Remove(0, indexOfSeperator + 1);            
            monthText.text = $"{locService.GetLocalised($"MONTH_{monthKey}")} {year}";
        }

        m_topVal.text = highestValue.ToString("C", CultureInfo.CurrentCulture);
        m_midVal.text = (highestValue * 0.5f).ToString("C", CultureInfo.CurrentCulture);
        m_quartVal.text = (highestValue * 0.25f).ToString("C", CultureInfo.CurrentCulture);
        m_bottomVal.text = 0.0f.ToString("C", CultureInfo.CurrentCulture);
    }

    public void SetToggleValues(bool[] toggleStates)
    {
        m_showIncomeToggle.SetIsOnWithoutNotify(toggleStates[(int)MonthGraphState.ShowStates.INCOME]);
        m_showExpensesToggle.SetIsOnWithoutNotify(toggleStates[(int)MonthGraphState.ShowStates.EXPENSES]);
        m_showRemainingToggle.SetIsOnWithoutNotify(toggleStates[(int)MonthGraphState.ShowStates.REMAINING]);
    }

    private float SetBarSizeAndText(bool isOn, RectTransform rectTrans, float barValue, float highestValue, float barOnStartPos, float barSize)
    {
        if (isOn)
        {
            rectTrans.anchorMin = new Vector2(barOnStartPos, rectTrans.anchorMin.y);
            rectTrans.anchorMax = new Vector2(barOnStartPos + barSize, barValue / highestValue);
            TextMeshProUGUI valueText = rectTrans.gameObject.GetComponentFromChild<TextMeshProUGUI>("Value");
            valueText.text = barValue.ToString("C", CultureInfo.CurrentCulture);
            barOnStartPos += barSize;
        }
        return barOnStartPos;
    }

    private int GetBarsActive()
    {
        return ToInt32(m_showIncomeToggle.isOn) + ToInt32(m_showExpensesToggle.isOn) + ToInt32(m_showRemainingToggle.isOn);
    }
}
