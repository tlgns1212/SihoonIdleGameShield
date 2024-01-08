using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DungeonItem : UI_Base
{
    #region Enum
    enum Images
    {
        ItemSquareIcon,
        ItemIcon,
        LockButtonImage
    }

    enum Texts
    {
        TitleText,
        VisitedText,
        VisitedLeftText,
        VisitText,
    }

    enum Buttons
    {
        VisitButton,
    }
    #endregion

    ScrollRect _scrollRect;
    Action _action;
    bool _isDrag = false;
    Data.DungeonData _data;
    DungeonGameData _dungeonGameData;

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

        GetButton((int)Buttons.VisitButton).gameObject.BindEvent(OnClickVisitButton);

        // Refresh();

        return true;
    }

    public void SetInfo(int dungeonID, ScrollRect scrollRect, Action callback)
    {
        _scrollRect = scrollRect;
        _action = callback;
        _data = Managers.Data.DungeonDic[dungeonID];

        GetText((int)Texts.TitleText).text = _data.TitleText;
        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(_data.IconLabel);

        if (Managers.Game.DungeonLevelDictionary.TryGetValue(dungeonID, out DungeonGameData value))
        {
            _dungeonGameData = value;
        }
        else
        {
            DungeonGameData dGameData = new DungeonGameData() { TodayMaxNum = _data.MaxVisitNum, Type = _data.DungeonType };
            _dungeonGameData = dGameData;
        }

        if (_data.LockOpenLevel <= Managers.Game.UserLevel)
        {
            GetImage((int)Images.LockButtonImage).gameObject.SetActive(false);
        }

        if (_dungeonGameData.Type == Define.DungeonType.Mine)
        {
            GetText((int)Texts.VisitedText).gameObject.SetActive(false);
            GetText((int)Texts.VisitedLeftText).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.VisitedText).text = _data.MaxVisitText;
        }


        Refresh();
    }

    public void Refresh()
    {
        if (_dungeonGameData.Type == Define.DungeonType.Mine)
            return;
        GetText((int)Texts.VisitedLeftText).text = $"{_dungeonGameData.TodayMaxNum - _dungeonGameData.TodayVisitedNum}/{_dungeonGameData.TodayMaxNum}";
    }

    void OnClickVisitButton()
    {
        if (_dungeonGameData.TodayVisitedNum >= _dungeonGameData.TodayMaxNum)
            return;
        if (_dungeonGameData.Type != Define.DungeonType.Mine)
            _dungeonGameData.TodayVisitedNum += 1;
        // TODO GOTO Popup;
        Refresh();

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
