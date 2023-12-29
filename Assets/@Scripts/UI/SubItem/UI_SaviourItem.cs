using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaviourItem : UI_Base
{
    #region Enum
    enum Images
    {
        ItemSquareIcon,
        ItemIcon,
        PlusGoldImage,
        BuyGoldImage
    }

    enum Texts
    {
        TitleText,
        LvText,
        ExtraLvText,
        PlusGoldText,
        ProgressTimeText,
        PlusNumText,
        BuyCostText
    }

    enum Buttons
    {
        BuySquareButton,
    }
    #endregion

    ScrollRect _scrollRect;
    bool _isDrag = false;

    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        Refresh();

        return true;
    }

    public void SetInfo(int saviourID, ScrollRect scrollRect)
    {
        _scrollRect = scrollRect;
        Data.SaviourData data = Managers.Data.SaviourDic[saviourID];

        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(data.IconLabel);
        GetText((int)Texts.TitleText).text = data.TitleText;
        // TODO LV + ExtraLV
        GetText((int)Texts.LvText).text = $"LV. {10000}";
        GetText((int)Texts.ExtraLvText).text = $"[+{10}LV]";

        // TODO PRogressBar


        Refresh();
    }

    void Refresh()
    {

    }

    #region 버튼 스크롤 대응
    public void OnDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
    #endregion
}
