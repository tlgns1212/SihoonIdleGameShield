using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MaxAccessoriesItem : UI_Base
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
        DescriptionText,
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
    bool _isDrag = false;
    string _itemEffectString;
    AccessoriesGameData _accessoriesGameData;
    Data.AccessoriesData _data;
    int _levelIndex = 0;

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
        if (_accessoriesGameData.Level >= _data.LevelDatas.Count - 1)
        {
            GetButton((int)Buttons.BuySquareButton).gameObject.SetActive(false);
        }
        if (_accessoriesGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }

        Refresh();
    }

    public void Refresh()
    {
        int maxLevel = _data.LevelDatas.Count;
        GetText((int)Texts.LvText).text = $"LV{_accessoriesGameData.Level}(MAX{maxLevel})";
        GetText((int)Texts.DescriptionText).text = $"{_itemEffectString} 단계 증가";
        GetText((int)Texts.ItemEffectText).text = $"{_accessoriesGameData.LValue}{_data.ItemEffectNumText}";
        GetText((int)Texts.PlusNumText).text = GetLValue().ToString();
        GetText((int)Texts.BuyCostText).text = GetCost().ToString();

        UpdateContinueInfo((Define.AccessoriesItemType)_data.ItemType);
    }

    void OnClickLockButton()
    {
        _accessoriesGameData.isLocked = false;
        _accessoriesGameData.LValue = _data.LevelDatas[0].LValue;
        _accessoriesGameData.BuyGold = _data.LevelDatas[0].NextCost;
        _accessoriesGameData.Level = _data.LevelDatas[0].Level;

        if (_accessoriesGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }
        Refresh();
    }

    public void LevelUp(int levelPlus = 1)
    {
        if (_accessoriesGameData.Level >= _data.LevelDatas.Count - 1)
        {
            GetButton((int)Buttons.BuySquareButton).gameObject.SetActive(false);
            return;
        }

        _accessoriesGameData.LValue = GetLValue();
        _accessoriesGameData.BuyGold = GetCost();
        _accessoriesGameData.Level += levelPlus;

        Refresh();
    }

    void UpdateContinueInfo(Define.AccessoriesItemType itemType)
    {
        // TODOTODOTODO 기본적인 AtkDamage, AtkRate 이런거 건드리지 않고 AccAtkDamage, AccAtkRAte 이런거 건드린느걸로 바꾸기, 그 이후 이것들 취합해서 AtkRate 구하기
        switch (itemType)
        {
            case Define.AccessoriesItemType.AtkRate:
                Managers.Game.Player.AtkRate = 1 + _accessoriesGameData.LValue / 100.0f;
                break;
            case Define.AccessoriesItemType.CriticalRate:
                Managers.Game.ContinueInfo.CriRate = _accessoriesGameData.LValue / 100.0f;
                print(Managers.Game.ContinueInfo.CriRate);
                break;
            case Define.AccessoriesItemType.MoveSpeed:
                Managers.Game.Player.MoveSpeed = _accessoriesGameData.LValue;
                break;
                // TODO 할인율 이거 일단 놨뒀음 나중에 할것!
            case Define.AccessoriesItemType.SaveSaleRate:
                Managers.Game.ContinueInfo.SaveSale = _accessoriesGameData.LValue / 100.0f;
                break;
            case Define.AccessoriesItemType.ShieldSaleRate:
                Managers.Game.ContinueInfo.ShieldSale = _accessoriesGameData.LValue / 100.0f;
                break;
        }
    }

    void OnClickBuySquareButton()
    {
        if (Managers.Game.Gold >= _accessoriesGameData.BuyGold)
        {
            Managers.Game.Gold -= _accessoriesGameData.BuyGold;
            LevelUp();
        }
    }

    int GetLValue()
    {
        return _data.LevelDatas[_accessoriesGameData.Level].LValue;
    }

    int GetCost()
    {
        return _data.LevelDatas[_accessoriesGameData.Level].NextCost;
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
