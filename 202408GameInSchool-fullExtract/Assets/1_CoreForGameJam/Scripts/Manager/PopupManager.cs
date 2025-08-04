using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : SingletonMonoBehaviour<PopupManager>
{
    private Popup[] _popups;
    protected override void doAwake()
    {
        _popups = GetComponentsInChildren<Popup>(includeInactive: true);
        foreach (Popup popup in _popups)
        {
            popup.Initialize();
        }
    }

    public T Open<T>() where T : Popup
    {
        var popup = _popups.FirstOrDefault(p => p.GetType() == typeof(T));
        if (popup != null)
        {
            popup.Open();
        }
        return (T)popup;
    }
}
