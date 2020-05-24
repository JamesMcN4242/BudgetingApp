////////////////////////////////////////////////////////////
/////   TimeScalePopupState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
using UnityEngine;

using static System.Convert;

using static FlowMessageDefs;

public class TimeScalePopupState : FlowStateBase
{
    private const string k_fromDropMsg = "fromDropIndex_";

    private readonly string[] k_dateRanges = null;
    private UITimeScalePopup m_uiTimeScalePopup;

    private Action<string, string> m_saveAction = null;
    private string m_startingID;
    private string m_endingID;

    public TimeScalePopupState(string[] ranges, string startID, string endID, Action<string, string> saveAction)
    {
        k_dateRanges = ranges;
        m_saveAction = saveAction;
        m_startingID = startID;
        m_endingID = endID;
    }

    protected override void StartPresentingState()
    {
        RefreshDropdownValues();
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_saveMsg:
                var dropVals = m_uiTimeScalePopup.GetDropdownValues();
                m_saveAction?.Invoke(dropVals.from, dropVals.to);
                break;

            case k_cancelMenuMsg:
                ControllingStateStack.PopState(this);
                break;

            case string dropChanged when dropChanged.StartsWith(k_fromDropMsg):
                int startIndex = ToInt32(dropChanged.Remove(0, k_fromDropMsg.Length));
                (m_startingID, m_endingID) = m_uiTimeScalePopup.GetDropdownValues();                
                RefreshDropdownValues(startIndex);
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiTimeScalePopup = GameObject.FindObjectOfType<UITimeScalePopup>();
        m_ui = m_uiTimeScalePopup;
        return m_ui != null;
    }

    private void RefreshDropdownValues()
    {
        int startIndex = Array.FindIndex(k_dateRanges, (element) => element == m_startingID);
        RefreshDropdownValues(startIndex);
    }

    private void RefreshDropdownValues(int startIndex)
    {
        int endIndex = Mathf.Max(startIndex, Array.FindIndex(k_dateRanges, (element) => element == m_endingID));
        m_uiTimeScalePopup.SetDropdownValues(k_dateRanges, startIndex, endIndex);
    }
}