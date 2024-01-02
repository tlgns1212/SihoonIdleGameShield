using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShieldItem : UI_Base
{
    #region Enum
    enum Images
    {
        ItemSquareIcon,
        ItemIcon,
        GoldImage,
        LockButtonImage
    }

    enum Texts
    {
        TitleText,
        ATKText,
        ATKStatText,
        PlusNumText,
        BuyCostText,
        PlusText
    }

    enum Buttons
    {
        BuySquareButton,
    }
    #endregion

    ScrollRect _scrollRect;
    Action _action;
    int _id;
    Data.ShieldData _data;
    ShieldGameData _shieldGameData;
    bool _isDrag = false;
    int _level = 0;
    int _buyCost = 0;
    int _totalLevel = 0;

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

        // Refresh();

        return true;
    }

    public void SetInfo(int shieldID, ScrollRect scrollRect, Action callback)
    {
        _id = shieldID;
        _data = Managers.Data.ShieldDic[shieldID];
        _scrollRect = scrollRect;
        _action = callback;

        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(_data.IconLabel);
        GetText((int)Texts.ATKText).text = _data.ItemEffectText;

        if (Managers.Game.ShieldLevelDictionary.TryGetValue(_id, out ShieldGameData sD))
        {
            _shieldGameData = sD;
        }
        else
        {
            _shieldGameData = new ShieldGameData();
            Managers.Game.ShieldLevelDictionary.Add(_id, _shieldGameData);
        }
        _level = _shieldGameData.Level;
        _totalLevel = _data.LevelDatas.Count - 1;

        Refresh();
    }

    public void Refresh()
    {
        if (_shieldGameData.isCompleted == true)
        {
            GetImage((int)Images.LockButtonImage).gameObject.SetActive(false);
            GetButton((int)Buttons.BuySquareButton).gameObject.SetActive(false);
        }
        else if (_shieldGameData.isLocked == true)
        {
            // 잠금
        }
        else
        {
            GetImage((int)Images.LockButtonImage).gameObject.SetActive(false);
        }

        GetText((int)Texts.TitleText).text = _data.TitleText;
        GetText((int)Texts.ATKStatText).text = _data.LevelDatas[_level].LValue.ToString(); ;

        if (_level == _totalLevel)
        {
            GetText((int)Texts.PlusText).gameObject.SetActive(false);
            GetText((int)Texts.PlusNumText).text = "다음 무기 열기";
        }
        else
        {
            GetText((int)Texts.PlusText).gameObject.SetActive(true);
            GetText((int)Texts.PlusNumText).text = _data.LevelDatas[_level].LValue.ToString();
        }
        _buyCost = _data.LevelDatas[_level].NextCost;
        GetText((int)Texts.BuyCostText).text = _buyCost.ToString();
    }
    void LevelUp()
    {
        if (_level + 1 <= _totalLevel)
        {
            _level += 1;
            _shieldGameData.Level = _level;
            Managers.Game.ContinueInfo.ShiAtk += _data.LevelDatas[_level].LValue;
        }
        else
        {
            Managers.Game.ContinueInfo.ShiAtk += _data.LevelDatas[_level].LValue;
            _shieldGameData.isCompleted = true;
            Managers.Game.ShieldLevelDictionary[_id + 1].isLocked = false;
            _action?.Invoke();
        }
        Refresh();
    }

    void OnClickBuySquareButton()
    {
        if (Managers.Game.Gold < _buyCost)
        {
            print("재화가 부족합니다.");
        }
        else
        {
            Managers.Game.Gold -= _buyCost;
            LevelUp();
        }
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
