using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentPopup : UI_Popup
{
    #region Enum
    enum Texts
    {
    }

    enum Buttons
    {
        ExitButton
    }
    #endregion

    Action _exitAction;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        Refresh();

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
    }

    public void SetExit(Action callback)
    {
        _exitAction = callback;
    }

    void OnClickExitButton()
    {
        _exitAction?.Invoke();
    }
}
