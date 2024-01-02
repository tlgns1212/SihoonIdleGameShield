using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NoMaxAccessoriesItem : UI_Base
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
        LvText,
        ItemEffectText,
        PlusNumText,
        BuyCostText,
    }

    enum Buttons
    {
        BuySquareButton,
        LockButton
    }
    #endregion

    ScrollRect _scrollRect;
    Action _action;
    int _id;
    bool _isDrag = false;
    string _itemEffectString;
    Data.AccessoriesData _data;
    AccessoriesGameData _accessoriesGameData;
    int _levelIndex = 0;

    // TODO RaiseGoldCost, RaiseLValue, 등등 여기에 들어가는 것들을 프로퍼티로 만들어서 두번 작업하지 않고 한번에 되게 하기

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

        return true;
    }

    public void SetInfo(int accessoriesID, ScrollRect scrollRect, Action callback)
    {
        _id = accessoriesID;
        _scrollRect = scrollRect;
        _action = callback;

        _data = Managers.Data.AccessoriesDic[accessoriesID];

        GetText((int)Texts.TitleText).text = _data.TitleText;
        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(_data.IconLabel);
        _itemEffectString = _data.ItemEffectText;

        if (Managers.Game.AccessoriesLevelDictionary.TryGetValue(accessoriesID, out AccessoriesGameData accLevel))
        {
            _accessoriesGameData = accLevel;
        }
        else
        {
            _accessoriesGameData = new AccessoriesGameData();
            Managers.Game.AccessoriesLevelDictionary.Add(accessoriesID, _accessoriesGameData);
        }



        Refresh();
    }

    public void LevelUp(int levelPlus = 1)
    {
        _accessoriesGameData.LValue += GetLValue();
        _accessoriesGameData.BuyGold += GetCost();
        _accessoriesGameData.Level += levelPlus;

        Refresh();
    }

    public void Refresh()
    {
        MakeLevelIndex();
        GetText((int)Texts.LvText).text = $"LV. {_accessoriesGameData.Level}";
        GetText((int)Texts.ItemEffectText).text = $"{_itemEffectString} {_accessoriesGameData.LValue}%증가";
        GetText((int)Texts.PlusNumText).text = GetLValue().ToString();
        GetText((int)Texts.BuyCostText).text = GetCost().ToString();

        UpdateContinueInfo((Define.AccessoriesItemType)_data.ItemType);
    }

    void UpdateContinueInfo(Define.AccessoriesItemType itemType)
    {
        switch (itemType)
        {
            case Define.AccessoriesItemType.AtkDamage:
                Managers.Game.ContinueInfo.AccAtk = _accessoriesGameData.LValue;
                break;
            case Define.AccessoriesItemType.CriticalDamage:
                Managers.Game.ContinueInfo.AccCriDamage = _accessoriesGameData.LValue / 100.0f;
                break;
            case Define.AccessoriesItemType.KillGoldRate:
                Managers.Game.ContinueInfo.AccKillGold = _accessoriesGameData.LValue / 100.0f;
                break;
            case Define.AccessoriesItemType.SaveGoldRate:
                Managers.Game.ContinueInfo.AccWaitGold = _accessoriesGameData.LValue / 100.0f;
                break;
            case Define.AccessoriesItemType.ManaGetRate:
                Managers.Game.ContinueInfo.AccManaGetRate = _accessoriesGameData.LValue / 100.0f;
                break;
            case Define.AccessoriesItemType.DEnergyGetRate:
                Managers.Game.ContinueInfo.AccDEnergyGetRate = _accessoriesGameData.LValue / 100.0f;
                break;
            case Define.AccessoriesItemType.RubyGetRate:
                Managers.Game.ContinueInfo.AccRubyGetRate = _accessoriesGameData.LValue / 100.0f;
                break;
        }
    }

    void OnClickLockButton()
    {
        _accessoriesGameData.isLocked = false;
        _accessoriesGameData.LValue = _data.LevelDatas[0].LValue;
        _accessoriesGameData.BuyGold = _data.LevelDatas[0].NextCost;

        if (_accessoriesGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }
        Refresh();
    }

    void OnClickBuySquareButton()
    {
        if (Managers.Game.Gold >= _accessoriesGameData.BuyGold)
        {
            Managers.Game.Gold -= _accessoriesGameData.BuyGold;
            LevelUp();
        }
    }

    void MakeLevelIndex()
    {
        while (true)
        {
            if (_levelIndex >= _data.RaiseLevelDatas.Count - 1)
                break;
            if (_data.RaiseLevelDatas[_levelIndex].Level > _accessoriesGameData.Level)
                break;
            _levelIndex++;
        }
    }

    int GetLValue() 
    {
        return _data.RaiseLevelDatas[_levelIndex].LValue;
    }

    int GetCost()
    {
        return _data.RaiseLevelDatas[_levelIndex].NextCost;
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
