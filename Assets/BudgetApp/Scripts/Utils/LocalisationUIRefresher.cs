////////////////////////////////////////////////////////////
/////   LocalisationUIRefresher.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalisationUIRefresher : MonoBehaviour
{
    [SerializeField] private string m_locKey = string.Empty;

    private LocalisationService m_locService = null;
    private TextMeshProUGUI m_text = null;

    void OnEnable()
    {
        RefreshText();
    }

    public void RefreshText()
    {
        TryGatherReferences();
        m_text.text = m_locService.GetLocalised(m_locKey);
    }

    private void TryGatherReferences()
    {
        if (m_locService == null)
        {
            m_locService = FindObjectOfType<MenuSceneDirector>().LocalisationService;
        }

        if(m_text == null)
        {
            m_text = GetComponent<TextMeshProUGUI>();
        }
    }
}
