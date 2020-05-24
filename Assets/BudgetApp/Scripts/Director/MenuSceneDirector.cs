////////////////////////////////////////////////////////////
/////   MenuSceneDirector.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class MenuSceneDirector : MonoBehaviour
{
    public LocalisationService LocalisationService { get; private set; }
    private StateController m_stateController = new StateController();

    // Start is called before the first frame update
    void Start()
    {
        m_stateController.PushState(new BaseMenuState());

        LocalisationService = new LocalisationService();
        LocalisationService.LoadLocalisation(LocalisationService.LocalisableCultures.ENGLISH, "Localisation/{0}_localisation");
    }

    // Update is called once per frame
    void Update()
    {
        m_stateController.UpdateStack();
    }
}
