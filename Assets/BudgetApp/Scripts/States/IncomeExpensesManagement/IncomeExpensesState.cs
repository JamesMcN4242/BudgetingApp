////////////////////////////////////////////////////////////
/////   UIIncomeExpenses.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using UnityEngine;

using static FlowMessageDefs;

public class IncomeExpensesState : FlowStateBase
{
    private const string k_addMsg = "add";
    private const string k_editMsg = "edit";
    private const string k_removeMsg = "remove";
    private const string k_breakdownMsg = "breakdown";
    private const string k_nextPageMsg = "nextPage";
    private const string k_previousPageMsg = "previousPage";

    private readonly string k_incomeExpensesKey;
    private readonly bool k_showingVariableValues;

    private UIIncomeExpenses m_uiIncomeExpenses = null;
    private LocalisationService m_locService = null;
    private GridElements m_gridElements;
    private int m_selectedElementIndex;
    private int m_pageNumber;
    
    public IncomeExpensesState(string incomeExpensesKey, bool showingVariableValues, LocalisationService locService)
    {
        k_incomeExpensesKey = incomeExpensesKey;
        k_showingVariableValues = showingVariableValues;
        m_locService = locService;

        string elementJson = PlayerPrefs.GetString(k_incomeExpensesKey, string.Empty);
        if(!string.IsNullOrEmpty(elementJson))
        {
            m_gridElements = JsonUtility.FromJson<GridElements>(elementJson);
        }
        m_pageNumber = 0;
    }

    protected override void StartPresentingState()
    {
        //TODO: Localise UI Text strings
        m_uiIncomeExpenses.SetTitle((k_showingVariableValues ? "Variable" : "Fixed") + " Values");
        BuildGridElements();
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_addMsg:
                ControllingStateStack.PushState(new AddOrEditElementState(OnNewElementAdded));
                break;

            case k_editMsg:
                ControllingStateStack.PushState(new AddOrEditElementState(OnElementEdited, m_gridElements.m_elements[m_selectedElementIndex]));
                break;

            case k_backMenuMsg:
                ControllingStateStack.PopState(this);
                break;

            case k_removeMsg:
                var popupText = new ConfirmationPopupState.PopupText
                {
                    m_title = m_locService.GetLocalised("CONFIRMATION_TITLE"),
                    m_description = m_locService.GetLocalised("DELETE_ENTRY_DESC"),
                    m_accept = m_locService.GetLocalised("CONTINUE"),
                    m_decline = m_locService.GetLocalised("CANCEL")
                };
                ControllingStateStack.PushState(new ConfirmationPopupState(popupText, RemoveSelectedElement));
                break;

            case k_breakdownMsg:
                ControllingStateStack.PushState(new MonthlyOverviewState());
                break;

            case k_nextPageMsg:
                m_pageNumber++;
                BuildGridElements();
                break;
                
            case k_previousPageMsg:
                m_pageNumber = Mathf.Max(m_pageNumber-1, 0);
                BuildGridElements();
                break;

            case string msg when msg.StartsWith(k_selectElementMsg):
                msg = msg.Replace(k_selectElementMsg, string.Empty);
                if(int.TryParse(msg, out int index))
                {
                    if (index == m_selectedElementIndex) break;

                    if(m_selectedElementIndex > -1)
                    {
                        bool selectionWasExpense = m_gridElements.m_elements[m_selectedElementIndex].IsExpense;
                        m_uiIncomeExpenses.SetButtonSelected(index, m_selectedElementIndex, selectionWasExpense);
                    }
                    else
                    {
                        m_uiIncomeExpenses.SetButtonSelected(index);
                    }

                    m_selectedElementIndex = index;
                    m_uiIncomeExpenses.SetEditRemoveInteractablity(true);
                }
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiIncomeExpenses = Object.FindObjectOfType<UIIncomeExpenses>();
        m_ui = m_uiIncomeExpenses;
        return m_ui != null;
    }

    private void OnNewElementAdded(GridElementData elementData)
    {
        if(m_gridElements.m_elements == null)
        {
            m_gridElements.m_elements = new List<GridElementData>();
        }
        m_gridElements.m_elements.Add(elementData);
        SaveToPlayerPrefs();

        //TODO: Delete singular element instead of rebuilding full grid
        BuildGridElements();
    }

    private void OnElementEdited(GridElementData elementData)
    {
        m_gridElements.m_elements[m_selectedElementIndex] = elementData;
        SaveToPlayerPrefs();

        //TODO: Delete singular element instead of rebuilding full grid
        BuildGridElements();
    }

    private void RemoveSelectedElement()
    {
        m_gridElements.m_elements.RemoveAt(m_selectedElementIndex);
        m_selectedElementIndex = -1;
        SaveToPlayerPrefs();

        if (!HasAnotherPage())
        {
            m_pageNumber = Mathf.Max(m_pageNumber - 1, 0);
        }

        //TODO: Delete singular element instead of rebuilding full grid
        BuildGridElements();
    }

    private void BuildGridElements()
    {
        m_uiIncomeExpenses.BuildGridElements(m_gridElements.m_elements, m_pageNumber);
        RebuildObserverList();

        m_selectedElementIndex = -1;
        m_uiIncomeExpenses.SetEditRemoveInteractablity(false);
        m_uiIncomeExpenses.SetPageNavigatorInteractability(m_pageNumber > 0, HasAnotherPage());
    }

    private void SaveToPlayerPrefs()
    {
        string json = JsonUtility.ToJson(m_gridElements);
        PlayerPrefs.SetString(k_incomeExpensesKey, json);
        PlayerPrefs.Save();
    }

    private bool HasAnotherPage()
    {
        return m_gridElements.m_elements?.Count > (m_pageNumber + 1) * UIIncomeExpenses.k_elementsPerGrid;
    }
}
