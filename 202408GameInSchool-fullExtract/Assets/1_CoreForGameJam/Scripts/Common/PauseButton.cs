using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        if(button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                PopupManager.Instance.Open<PausePopup>();
            });
        }
    }
}
