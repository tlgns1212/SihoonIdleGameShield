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
    Define.ResourceType _buyResource = Define.ResourceType.Gold;
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
            // 기본적인 악세서리는 다 잠금 풀린 상태로 존재
            if (accessoriesID >= 10001 || accessoriesID <= 10012)
                _accessoriesGameData = new AccessoriesGameData() { isLocked = false, LValue = _data.LevelDatas[0].LValue, BuyCost = _data.LevelDatas[0].NextCost };
            else
                _accessoriesGameData = new AccessoriesGameData();
            Managers.Game.AccessoriesLevelDictionary.Add(accessoriesID, _accessoriesGameData);
        }
        if (_accessoriesGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }

        if (_data.BuyResource == "Mana")
        {
            _buyResource = Define.ResourceType.Mana;
            GetImage((int)Images.GoldImage).sprite = Managers.Resource.Load<Sprite>("GUI_26");
        }
        else if (_data.BuyResource == "DEnergy")
        {
            _buyResource = Define.ResourceType.DimensionEnergy;
            GetImage((int)Images.GoldImage).sprite = Managers.Resource.Load<Sprite>("GUI_25");
        }
        else if (_data.BuyResource == "Ruby")
        {
            _buyResource = Define.ResourceType.Ruby;
            GetImage((int)Images.GoldImage).sprite = Managers.Resource.Load<Sprite>("GUI_22");
        }
        else
        {
            _buyResource = Define.ResourceType.Gold;
            GetImage((int)Images.GoldImage).sprite = Managers.Resource.Load<Sprite>("GUI_21");
        }


        Refresh();
    }

    public void LevelUp(int levelPlus = 1)
    {
        MakeLevelIndex();
        _accessoriesGameData.Level += levelPlus;
        _accessoriesGameData.LValue += GetLValue();
        _accessoriesGameData.BuyCost = GetCost();


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
        _accessoriesGameData.BuyCost = _data.LevelDatas[0].NextCost;

        if (_accessoriesGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }
        Refresh();
    }

    void OnClickBuySquareButton()
    {
        switch (_buyResource)
        {
            case Define.ResourceType.Mana:
                if (Managers.Game.Mana >= _accessoriesGameData.BuyCost)
                {
                    Managers.Game.Mana -= _accessoriesGameData.BuyCost;
                    LevelUp();
                }
                break;
            case Define.ResourceType.DimensionEnergy:
                if (Managers.Game.DimensionEnergy >= _accessoriesGameData.BuyCost)
                {
                    Managers.Game.DimensionEnergy -= _accessoriesGameData.BuyCost;
                    LevelUp();
                }
                break;
            case Define.ResourceType.Ruby:
                if (Managers.Game.Ruby >= _accessoriesGameData.BuyCost)
                {
                    Managers.Game.Ruby -= _accessoriesGameData.BuyCost;
                    LevelUp();
                }
                break;
            default:
                if (Managers.Game.Gold >= _accessoriesGameData.BuyCost)
                {
                    Managers.Game.Gold -= _accessoriesGameData.BuyCost;
                    LevelUp();
                }
                break;
        }
    }

    void MakeLevelIndex()
    {
        while (true)
        {
            if (_levelIndex >= _data.RaiseLevelDatas.Count - 1)
                break;
            if (_data.RaiseLevelDatas[_levelIndex + 1].Level > _accessoriesGameData.Level)
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
