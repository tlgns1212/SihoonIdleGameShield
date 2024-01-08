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
        LockResourceImage,
    }

    enum Texts
    {
        TitleText,
        ATKText,
        ATKStatText,
        PlusNumText,
        BuyCostText,
        LockCostText,
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
    Action _calcAllLValue;
    bool _isDrag = false;
    Data.FriendData _data;
    public FriendGameData _friendGameData;
    public Define.FriendEffectType _effectType = Define.FriendEffectType.Atk;
    Define.ResourceType _buyResource = Define.ResourceType.DimensionEnergy;

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

    public void SetInfo(int friendID, ScrollRect scrollRect, Action callback, Action calcLValue)
    {
        _scrollRect = scrollRect;
        _action = callback;
        _calcAllLValue = calcLValue;
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
        else
        {
            _buyResource = Define.ResourceType.Ruby;
            GetText((int)Texts.LockCostText).text = _data.FirstBuyCost.ToString();
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

        _effectType = (Define.FriendEffectType)_data.ItemEffect;

        Refresh();
    }

    public void Refresh()
    {
        GetText((int)Texts.TitleText).text = $"{_data.TitleText} LV{_friendGameData.Level} (MAX{_data.LevelDatas.Count})";
        GetText((int)Texts.ATKStatText).text = _friendGameData.LValue.ToString();
        GetText((int)Texts.PlusNumText).text = GetLValue().ToString();
        GetText((int)Texts.BuyCostText).text = GetCost().ToString();
    }

    // TODO : 구매까지는 했는데, 능력 적용 다 빼놨음, 어떤 능력을 적용해야 할 지 몰라서, 추후에 능력적용 바람
    public void LevelUp(int levelPlus = 1)
    {
        if (_friendGameData.Level >= _data.LevelDatas.Count - 1)
        {
            GetButton((int)Buttons.BuySquareButton).gameObject.SetActive(false);
            return;
        }

        _friendGameData.Level += levelPlus;
        _friendGameData.LValue = _data.LevelDatas[_friendGameData.Level].LValue;
        _friendGameData.BuyCost = _data.LevelDatas[_friendGameData.Level].NextCost;

        _calcAllLValue?.Invoke();
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
        if (Managers.Game.Ruby >= _data.FirstBuyCost)
        {
            Managers.Game.Ruby -= _data.FirstBuyCost;

            _friendGameData.isLocked = false;
            _friendGameData.Level = _data.LevelDatas[0].Level;
            _friendGameData.LValue = _data.LevelDatas[0].LValue;
            _friendGameData.BuyCost = _data.LevelDatas[0].NextCost;
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
        }

        if (_friendGameData.isLocked == false)
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
                if (Managers.Game.Mana >= _friendGameData.BuyCost)
                {
                    Managers.Game.Mana -= _friendGameData.BuyCost;
                    LevelUp();
                }
                break;
            case Define.ResourceType.DimensionEnergy:
                if (Managers.Game.DimensionEnergy >= _friendGameData.BuyCost)
                {
                    Managers.Game.DimensionEnergy -= _friendGameData.BuyCost;
                    LevelUp();
                }
                break;
            case Define.ResourceType.Ruby:
                if (Managers.Game.Ruby >= _friendGameData.BuyCost)
                {
                    Managers.Game.Ruby -= _friendGameData.BuyCost;
                    LevelUp();
                }
                break;
            default:
                if (Managers.Game.Gold >= _friendGameData.BuyCost)
                {
                    Managers.Game.Gold -= _friendGameData.BuyCost;
                    LevelUp();
                }
                break;
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
