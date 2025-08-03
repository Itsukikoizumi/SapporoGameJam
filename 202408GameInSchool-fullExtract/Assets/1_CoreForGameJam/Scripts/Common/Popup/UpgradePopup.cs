using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 最大4つの選択肢の内から1つを選ぶポップアップ
/// </summary>
public class UpgradePopup : Popup
{
    public class ButtonInfo
    {
        // 説明
        public string Description;

        public Action OnClick;
    }

    [SerializeField]
    private List<UpgradePopupButton> _buttons;

    private Queue<List<ButtonInfo>> _infoGroups = new Queue<List<ButtonInfo>>();

    public void SetButtons(Queue<List<ButtonInfo>> infoGroups)
    {
        _infoGroups = infoGroups;

        var infos = _infoGroups.Dequeue();
        SetButtons(infos);
    }

    public void SetButtons(List<ButtonInfo> infos)
    {
        _buttons.ForEach(b => b.gameObject.SetActive(false));

        infos.ForEachWithIndex((index, info) =>
        {
            var button = _buttons[index];
            button.gameObject.SetActive(true);
            button.Set(info, OnSelected);
        });

        EventSystem.current.SetSelectedGameObject(_buttons.First().gameObject);
    }

    public void OnSelected()    
    {
        if(_infoGroups.Any())
        {
            // 次の選択肢群を出さねばならない
            var infos = _infoGroups.Dequeue();
            SetButtons(infos);
        }
        else
        {
            // 選択肢終わったらポップアップを閉じる
            Close();
        }
    }
}
