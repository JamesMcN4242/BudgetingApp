////////////////////////////////////////////////////////////
/////   MenuSceneDirector.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

using static PlayerPrefDefs;
using static ResourceDefs;

public class MenuSceneDirector : MonoBehaviour
{
    public LocalisationService LocalisationService { get; private set; }
    private StateController m_stateController = new StateController();

    // Start is called before the first frame update
    void Start()
    {
        LocalisationService = new LocalisationService();
        int index = PlayerPrefs.GetInt(k_localisationIndexKey, 0);
        LocalisationService.LoadLocalisation((LocalisationService.LocalisableCultures)index, k_languageResourceFormat);
        
        m_stateController.PushState(new BaseMenuState(LocalisationService));
    }

    // Update is called once per frame
    void Update()
    {
        m_stateController.UpdateStack();
    }
}
