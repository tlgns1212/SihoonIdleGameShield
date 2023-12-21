using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_JewelPopup : UI_Popup
{
    #region Enum
    enum Texts
    {
    }

    enum Buttons
    {
    }
    #endregion

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

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
}
