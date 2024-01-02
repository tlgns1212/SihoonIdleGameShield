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
    int _id;
    Data.ShieldData _data;
    ShieldData _shieldData;
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

    public void SetInfo(int shieldID, ScrollRect scrollRect)
    {
        _id = shieldID;
        _data = Managers.Data.ShieldDic[shieldID];
        _scrollRect = scrollRect;
        
        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(_data.IconLabel);
        GetText((int)Texts.ATKText).text = _data.ItemEffectText;

        if (Managers.Game.ShieldLevelDictionary.TryGetValue(_id, out ShieldData sD))
        {
            _shieldData = sD;
        }
        else
        {
            _shieldData = new ShieldData();
            Managers.Game.ShieldLevelDictionary.Add(_id, _shieldData);
        }
        _level = _shieldData.Level;
        _totalLevel = _data.LevelDatas.Count - 1;

        if(_shieldData.isCompleted == true || _shieldData.isLocked == true)
        {
            GetButton((int)Buttons.BuySquareButton).interactable = false;
            GetButton((int)Buttons.BuySquareButton).GetComponent<Image>().color = Color.black;
        }
        else
        {
            GetButton((int)Buttons.BuySquareButton).interactable = true;
            GetButton((int)Buttons.BuySquareButton).GetComponent<Image>().color = Color.white;
        }

        Refresh();
    }

    void Refresh()
    {
        if (Managers.Game.ShieldLevelDictionary[_id].isCompleted == true || Managers.Game.ShieldLevelDictionary[_id].isLocked == true)
        {
            GetButton((int)Buttons.BuySquareButton).interactable = false;
            GetButton((int)Buttons.BuySquareButton).GetComponent<Image>().color = Color.black;
        }

        GetText((int)Texts.TitleText).text = _data.TitleText;
        // TODO 수치 입력하기

        GetText((int)Texts.ATKStatText).text = _data.LevelDatas[_level].LValue.ToString(); ;
        
        
        if (_level == _totalLevel)
        {
            GetText((int)Texts.PlusText).gameObject.SetActive(false);
            GetText((int)Texts.PlusNumText).text = "다음 무기 열기";
        }
        else
        {
            GetText((int)Texts.PlusText).gameObject.SetActive(true);
            GetText((int)Texts.PlusNumText).text = (_data.LevelDatas[_level + 1].LValue - _data.LevelDatas[_level].LValue).ToString();
        }
        _buyCost = _data.LevelDatas[_level].NextCost;
        GetText((int)Texts.BuyCostText).text = _buyCost.ToString();
    }
    void LevelUp()
    {
        if (_level + 1 <= _totalLevel)
        {
            _level += 1;
            _shieldData.Level = _level;
            Managers.Game.ContinueInfo.Atk = _data.LevelDatas[_level].LValue;
        }
        else
        {
            Data.ShieldData nextData = Managers.Data.ShieldDic[_id + 1];
            Managers.Game.ContinueInfo.Atk = nextData.LevelDatas[0].LValue;
            Managers.Game.ShieldLevelDictionary[_id].isCompleted = true;
        }
        Refresh();
    }

    void OnClickBuySquareButton()
    {
        if (Managers.Game.ShieldLevelDictionary[_id].isCompleted == true || Managers.Game.ShieldLevelDictionary[_id].isLocked == true)
            return;
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
