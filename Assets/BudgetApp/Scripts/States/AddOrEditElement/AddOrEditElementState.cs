////////////////////////////////////////////////////////////
/////   AddOrEditElementState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
using System.Globalization;
using UnityEngine;

using static FlowMessageDefs;

public class AddOrEditElementState : FlowStateBase
{
    private UIAddOrEditElement m_uiAddition = null;
    private Action<GridElementData> m_saveAction = null;
    private GridElementData? m_elementData;

    public AddOrEditElementState(Action<GridElementData> saveAction, GridElementData? slotData = null)
    {
        m_saveAction = saveAction;
        m_elementData = slotData;
    }

    protected override void StartPresentingState()
    {
        if(m_elementData.HasValue)
        {
            m_uiAddition.SetTitle("Edit Entry");
            m_uiAddition.SetStartingInputs(m_elementData.Value.m_variableName, m_elementData.Value.m_variableValue.ToString("N2", CultureInfo.CurrentCulture));
        }
        else
        {
            m_uiAddition.SetTitle("Add Entry");
            m_uiAddition.SetStartingInputs(string.Empty, string.Empty);
        }
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_cancelMenuMsg:
                ControllingStateStack.PopState(this);
                break;

            case k_saveMsg:
                var enteredData = m_uiAddition.GetInputFieldContent();

                //TODO: Fix for commas - being seen as thousands indicator
                if (float.TryParse(enteredData.value, NumberStyles.Float,
                              CultureInfo.InvariantCulture, out float value))
                {
                    m_elementData = new GridElementData { m_variableName = enteredData.name, m_variableValue = value };
                    m_saveAction?.Invoke(m_elementData.Value);
                    ControllingStateStack.PopState(this);
                }
                else
                {
                    //TODO: Error popup
                }
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiAddition = Component.FindObjectOfType<UIAddOrEditElement>();
        m_ui = m_uiAddition;
        return m_ui != null;
    }
}
