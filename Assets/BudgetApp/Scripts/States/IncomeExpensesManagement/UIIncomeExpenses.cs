////////////////////////////////////////////////////////////
/////   UIIncomeExpenses.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using static FlowMessageDefs;

public class UIIncomeExpenses : UIStateBase
{
    private static readonly Color k_expenseColor = Color.red;
    private static readonly Color k_incomeColor = Color.green;
    private static readonly Color k_selectedColor = Color.yellow;

    private const int k_elementsInRow = 3;
    private const int k_elementsInColumn = 7;
    private const float k_rowSpacing = 0.03f;
    private const float k_columnSpacing = 0.03f;
    private const float k_rowElementSize = (1.0f / k_elementsInRow) - k_columnSpacing;
    private const float k_columnElementSize = (1.0f / k_elementsInColumn) - k_rowSpacing;
    private const int k_elementsPerGrid = k_elementsInColumn * k_elementsInRow;

    private Transform m_gridTransform = null;
    private TextMeshProUGUI m_titleText = null;
    private Button m_editButton = null;
    private Button m_removeButton = null;

    void Start()
    {
        m_gridTransform = gameObject.FindChildByName("Grid").transform;
        m_titleText = gameObject.GetComponentFromChild<TextMeshProUGUI>("Title");
        m_editButton = gameObject.GetComponentFromChild<Button>("Edit");
        m_removeButton = gameObject.GetComponentFromChild<Button>("Remove");
    }

    public void BuildGridElements(List<GridElementData> gridElements)
    {
        //TODO: Page the grid on overflowing values
        m_gridTransform.DestroyAllChildren();
        if (gridElements == null || gridElements.Count == 0) return;

        GameObject gridPrefab = Resources.Load<GameObject>("UIGridElement");
        float yMaxAnchor = 1.0f;
        for (int rowIndex = 0, gridIndex = 0; rowIndex < k_elementsInColumn; rowIndex++)
        {
            float xMinAnchor = 0.0f;
            for (int columnIndex = 0; columnIndex < k_elementsInRow; columnIndex++)
            {
                RectTransform element = Object.Instantiate(gridPrefab, m_gridTransform).GetComponent<RectTransform>();
                element.name = $"Element_{gridIndex}";

                Image image = element.GetComponent<Image>();
                image.color = gridElements[gridIndex].IsExpense ? k_expenseColor : k_incomeColor;

                UIButtonInteraction buttonInteraction = element.GetComponent<UIButtonInteraction>();
                buttonInteraction.m_message = k_selectElementMsg + gridIndex;

                TextMeshProUGUI text = element.gameObject.GetComponentFromChild<TextMeshProUGUI>("Text");
                text.text = $"{gridElements[gridIndex].m_variableName}:\n{gridElements[gridIndex].m_variableValue.ToString("C", CultureInfo.CurrentCulture)}";

                element.anchorMin = new Vector2(xMinAnchor, yMaxAnchor - k_columnElementSize);
                element.anchorMax = new Vector2(xMinAnchor + k_rowElementSize, yMaxAnchor);

                xMinAnchor += k_rowElementSize + k_rowSpacing;
                gridIndex++;
                if(gridIndex >= gridElements.Count)
                {
                    return;
                }
            }
            yMaxAnchor -= k_columnElementSize + k_columnSpacing;
        }
    }

    public void SetTitle(string titleText)
    {
        m_titleText.text = titleText;
    }

    public void SetEditRemoveInteractablity(bool interactable)
    {
        m_editButton.interactable = interactable;
        m_removeButton.interactable = interactable;
    }

    public void SetButtonSelected(int indexToSelect, int previousIndex = -1, bool previousIsExpense = false)
    {
        Image toSelectImage = gameObject.GetComponentFromChild<Image>($"Element_{indexToSelect}");
        toSelectImage.color = k_selectedColor;

        if(previousIndex > -1)
        {
            Image toUnselectImage = gameObject.GetComponentFromChild<Image>($"Element_{previousIndex}");
            toUnselectImage.color = previousIsExpense ? k_expenseColor : k_incomeColor;
        }
    }
}
