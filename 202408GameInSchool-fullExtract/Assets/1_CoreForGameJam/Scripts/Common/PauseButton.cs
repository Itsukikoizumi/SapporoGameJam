using Surviver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    private SurviverSceneUI _sceneUI;
    // Start is called before the first frame update
    void Start()
    {
        _sceneUI = FindObjectOfType<SurviverSceneUI>();
        var button = GetComponent<UnityEngine.UI.Button>();
        if(button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                PopupManager.Instance.Open<PausePopup>();
                _sceneUI.ToggleTimer();
            });
        }
    }
}
