using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendItem : UI_Base
{
    #region Enum
    enum Images
    {
        ItemSquareIcon,
        ItemIcon,
        GoldImage,

    }

    enum Texts
    {
        TitleText,
        ATKText,
        ATKStatText,
        PlusNumText,
        BuyCostText,
    }

    enum Buttons
    {
        BuySquareButton,
        EquipmentButton,
        QuestionButton,
        LockButton,
    }
    #endregion

    ScrollRect _scrollRect;
    Action _action;
    bool _isDrag = false;
    Data.FriendData _data;
    FriendGameData _friendGameData;

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

        GetButton((int)Buttons.BuySquareButton).gameObject.BindEvent(OnClickBuySquareButton);
        GetButton((int)Buttons.LockButton).gameObject.BindEvent(OnClickLockButton);
        GetButton((int)Buttons.QuestionButton).gameObject.BindEvent(OnClickQuestionButton);
        GetButton((int)Buttons.EquipmentButton).gameObject.BindEvent(OnClickEquipmentButton);

        return true;
    }

    public void SetInfo(int friendID, ScrollRect scrollRect, Action callback)
    {
        _scrollRect = scrollRect;
        _action = callback;
        _data = Managers.Data.FriendDic[friendID];

        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(_data.IconLabel);
        GetText((int)Texts.ATKText).text = _data.ItemEffectText;

        if (Managers.Game.FriendLevelDictionary.TryGetValue(friendID, out FriendGameData friendLevel))
        {
            _friendGameData = friendLevel;
        }
        else
        {
            _friendGameData = new FriendGameData();
            Managers.Game.FriendLevelDictionary.Add(friendID, _friendGameData);
        }
        if (_friendGameData.Level >= _data.LevelDatas.Count - 1)
        {
            GetButton((int)Buttons.BuySquareButton).gameObject.SetActive(false);
        }
        if (_friendGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }


        Refresh();
    }

    public void Refresh()
    {
        GetText((int)Texts.TitleText).text = $"{_data.TitleText} LV{10} (MAX{_data.LevelDatas.Count})";
        GetText((int)Texts.ATKStatText).text = _friendGameData.LValue.ToString();
        GetText((int)Texts.PlusNumText).text = GetLValue().ToString();
        GetText((int)Texts.BuyCostText).text = GetCost().ToString();
    }

    public void LevelUp(int levelPlus = 1)
    {
        if (_friendGameData.Level >= _data.LevelDatas.Count - 1)
        {
            GetButton((int)Buttons.BuySquareButton).gameObject.SetActive(false);
            return;
        }

        _friendGameData.LValue = _data.LevelDatas[_friendGameData.Level].LValue;
        _friendGameData.BuyCost = _data.LevelDatas[_friendGameData.Level].NextCost;
        _friendGameData.Level += levelPlus;

        Refresh();
    }

    int GetLValue()
    {
        return _data.LevelDatas[_friendGameData.Level].LValue;
    }

    int GetCost()
    {
        return _data.LevelDatas[_friendGameData.Level].NextCost;
    }

    void OnClickLockButton()
    {
        _friendGameData.isLocked = false;
        _friendGameData.Level = _data.LevelDatas[0].Level;
        _friendGameData.LValue = _data.LevelDatas[0].LValue;
        _friendGameData.BuyCost = _data.LevelDatas[0].NextCost;

        if (_friendGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }
        Refresh();
    }

    void OnClickBuySquareButton()
    {
        // TODO Ruby로 바꾸기
        if (Managers.Game.Gold >= _friendGameData.BuyCost)
        {
            Managers.Game.Gold -= _friendGameData.BuyCost;
            LevelUp();
        }
    }

    void OnClickQuestionButton()
    {

    }

    void OnClickEquipmentButton()
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
