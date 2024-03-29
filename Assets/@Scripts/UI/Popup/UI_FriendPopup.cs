using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        FriendTab,
        FriendItem,
    }
    #endregion

    ScrollRect _scrollRect;
    List<UI_FriendItem> _items = new List<UI_FriendItem>();

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

        GetObject((int)GameObjects.FriendTab).BindEvent(null, OnDrag, Define.UIEvent.Drag);
        GetObject((int)GameObjects.FriendTab).BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        GetObject((int)GameObjects.FriendTab).BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        for (int i = 0; i < 12; i++)
        {
            UI_FriendItem ai = Managers.UI.MakeSubItem<UI_FriendItem>(GetObject((int)GameObjects.FriendItem).transform);
            _items.Add(ai);
            ai.SetInfo(10001 + i, _scrollRect, RefreshAllItems, CalculateAllLValue);
        }

        return true;
    }

    void RefreshAllItems()
    {
        foreach (UI_FriendItem item in _items)
        {
            item.Refresh();
        }
    }

    void CalculateAllLValue()
    {
        int atk = 0;
        int atkRate = 0;
        // TODO 공격력, 공격속도 말고 다른것도 추가하기
        foreach (UI_FriendItem item in _items)
        {
            switch (item._effectType)
            {
                case Define.FriendEffectType.Atk:
                    atk += item._friendGameData.LValue;
                    break;
                case Define.FriendEffectType.AtkRate:
                    atkRate += item._friendGameData.LValue;
                    break;
            }
        }
        Managers.Game.ContinueInfo.FriAtk = atk;
        Managers.Game.ContinueInfo.FriAtkRate = atkRate;
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
