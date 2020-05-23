////////////////////////////////////////////////////////////
/////   GridElementData.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Collections.Generic;

[System.Serializable]
public struct GridElementData
{
    public string m_variableName;
    public float m_variableValue;
    public bool IsExpense => m_variableValue < 0.0f;
}

[System.Serializable]
public struct GridElements
{
    public List<GridElementData> m_elements;
}