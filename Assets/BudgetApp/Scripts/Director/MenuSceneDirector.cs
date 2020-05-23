////////////////////////////////////////////////////////////
/////   MenuSceneDirector.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class MenuSceneDirector : MonoBehaviour
{
    private StateController m_stateController = new StateController();

    // Start is called before the first frame update
    void Start()
    {
        m_stateController.PushState(new BaseMenuState());
    }

    // Update is called once per frame
    void Update()
    {
        m_stateController.UpdateStack();
    }
}
