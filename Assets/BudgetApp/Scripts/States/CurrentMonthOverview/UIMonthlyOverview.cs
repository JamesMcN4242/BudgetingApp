////////////////////////////////////////////////////////////
/////   UIMonthlyOverview.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMonthlyOverview : UIStateBase
{
    private struct SegmentData
    {
        public Image m_backingImage;
        public TextMeshProUGUI m_valueText;
    }

    private static readonly Color k_incomeCol = new Color(0.5235f, 1.0f, 0.6443f);
    private static readonly Color k_expensesCol = new Color(1.0f, 0.5254f, 0.5383f);

    private SegmentData m_totalFixedIncome;
    private SegmentData m_totalFixedExpenses;
    private SegmentData m_totalVariableIncome;
    private SegmentData m_totalVariableExpenses;
    private SegmentData m_currentMonthlyBalance;

    void Start()
    {
        m_totalFixedIncome = BuildSegmentData("TotalFixedIncome");
        m_totalFixedExpenses = BuildSegmentData("TotalFixedExpenses");
        m_totalVariableIncome = BuildSegmentData("TotalVariableIncome");
        m_totalVariableExpenses = BuildSegmentData("TotalVariableExpenses");
        m_currentMonthlyBalance = BuildSegmentData("CurrentLeft");
        SetKnownSegmentColours();
    }

    public void SetData(MonthlyValueData monthlyData)
    {
        SetSegmentText(m_totalFixedIncome, monthlyData.m_totalFixedIncome);
        SetSegmentText(m_totalFixedExpenses, monthlyData.m_totalFixedExpenses);
        SetSegmentText(m_totalVariableIncome, monthlyData.m_totalVariableIncome);
        SetSegmentText(m_totalVariableExpenses, monthlyData.m_totalVariableExpenses);

        float monthlyBalance = monthlyData.MonthlyBalanceRemaining;
        SetSegmentText(m_currentMonthlyBalance, monthlyBalance);

        //TODO: Set target balance remaining in app to decide colour
        m_currentMonthlyBalance.m_backingImage.color = monthlyBalance >= 0.0f ? k_incomeCol : k_expensesCol;
    }

    private void SetSegmentText(SegmentData segmentData, float value)
    {
        segmentData.m_valueText.text = value.ToString("C", CultureInfo.CurrentCulture);
    }

    private void SetKnownSegmentColours()
    {
        m_totalFixedIncome.m_backingImage.color = k_incomeCol;
        m_totalVariableIncome.m_backingImage.color = k_incomeCol;
        m_totalFixedExpenses.m_backingImage.color = k_expensesCol;
        m_totalVariableExpenses.m_backingImage.color = k_expensesCol;
    }

    private SegmentData BuildSegmentData(string backingImageName)
    {
        Image backingImage = gameObject.GetComponentFromChild<Image>(backingImageName);
        return new SegmentData
        {
            m_backingImage = backingImage,
            m_valueText = backingImage.gameObject.GetComponentFromChild<TextMeshProUGUI>("Value")
        };
    }
}
