using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ResourcesPopup : UI_Popup
{
    #region Enum
    enum Texts
    {
        GoldNumText,
        ManaNumText,
        DimensionEnergyNumText,
        RubyNumText
    }

    enum Images
    {
        GoldImage,
        ManaImage,
        DimensionEnergyImage,
        RubyImage
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

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Refresh();
        Managers.Game.OnResourcesChanged += Refresh;

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetText((int)Texts.GoldNumText).text = Managers.Game.Gold.ToString();
        GetText((int)Texts.ManaNumText).text = Managers.Game.Mana.ToString();
        GetText((int)Texts.DimensionEnergyNumText).text = Managers.Game.DimensionEnergy.ToString();
        GetText((int)Texts.RubyNumText).text = Managers.Game.Ruby.ToString();
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnResourcesChanged -= Refresh;
    }
}
