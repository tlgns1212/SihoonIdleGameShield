using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JewelPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        JewelTab,
        JewelContent,
    }
    enum Buttons
    {
        JewelAssemble,
        JewelDisassemble,
        JewelSell,
        JewelSort,
    }
    #endregion

    ScrollRect _scrollRect;
    List<UI_JewelItem> _items = new List<UI_JewelItem>();

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        _scrollRect = Util.FindChild<ScrollRect>(gameObject);

        Refresh();

        GetObject((int)GameObjects.JewelTab).BindEvent(null, OnDrag, Define.UIEvent.Drag);
        GetObject((int)GameObjects.JewelTab).BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        GetObject((int)GameObjects.JewelTab).BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        for (int i = 0; i < 40; i++)
        {
            UI_JewelItem ji = Managers.UI.MakeSubItem<UI_JewelItem>(GetObject((int)GameObjects.JewelContent).transform);
            _items.Add(ji);
            ji.SetInfo(10001 + i, _scrollRect, () =>
            {
                foreach (UI_JewelItem item in _items)
                {
                    item.Refresh();
                }
            });
        }

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {

    }

    #region 버튼 스크롤 대응
    public void OnDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
    #endregion
}
