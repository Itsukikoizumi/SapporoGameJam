using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI _desc;
    public void Set(UpgradePopup.ButtonInfo info, Action onSelected)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => 
        {
            info.OnClick();
            onSelected();
        });

        _desc.text = info.Description;
    }
}
