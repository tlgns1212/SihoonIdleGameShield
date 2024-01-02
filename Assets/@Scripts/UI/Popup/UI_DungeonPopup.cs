using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DungeonPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        DungeonTab,
        DungeonItem,
    }
    #endregion

    ScrollRect _scrollRect;
    List<UI_DungeonItem> _items = new List<UI_DungeonItem>();

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

        GetObject((int)GameObjects.DungeonTab).BindEvent(null, OnDrag, Define.UIEvent.Drag);
        GetObject((int)GameObjects.DungeonTab).BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        GetObject((int)GameObjects.DungeonTab).BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        for (int i = 0; i < 12; i++)
        {
            UI_DungeonItem di = Managers.UI.MakeSubItem<UI_DungeonItem>(GetObject((int)GameObjects.DungeonItem).transform);
            _items.Add(di);
            di.SetInfo(10001 + i, _scrollRect, () =>
            {
                foreach (UI_DungeonItem item in _items)
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
