using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaviourItem : UI_Base
{
    #region Enum
    enum Images
    {
        ItemSquareIcon,
        ItemIcon,
        PlusGoldImage,
        BuyGoldImage,
        ProgressBarFill
    }

    enum Texts
    {
        TitleText,
        LvText,
        ExtraLvText,
        PlusGoldText,
        ProgressTimeText,
        PlusNumText,
        BuyCostText
    }

    enum Buttons
    {
        BuySquareButton,
        LockButton,
    }

    #endregion

    int _id;
    ScrollRect _scrollRect;
    Action _action;
    bool _isDrag = false;
    Data.SaviourData _data;
    SaviourGameData _saviourGameData;
    float _lastRewardTime;
    int _goldIndex = 0;

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

    public void SetInfo(int saviourID, ScrollRect scrollRect, Action callback)
    {
        _id = saviourID;
        _scrollRect = scrollRect;
        _action = callback;
        _data = Managers.Data.SaviourDic[saviourID];

        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(_data.IconLabel);
        GetText((int)Texts.TitleText).text = _data.TitleText;

        if (Managers.Game.SaviourLevelDictionary.TryGetValue(saviourID, out SaviourGameData value))
        {
            _saviourGameData = value;
        }
        else
        {
            _saviourGameData = new SaviourGameData();
            Managers.Game.SaviourLevelDictionary.Add(saviourID, _saviourGameData);
        }

        Refresh();
    }

    public void Refresh()
    {
        if (_saviourGameData.isLocked == false)
        {
            GetButton((int)Buttons.LockButton).gameObject.SetActive(false);
        }

        GetText((int)Texts.LvText).text = $"LV. {_saviourGameData.Level}";
        GetText((int)Texts.ExtraLvText).text = $"[+{_saviourGameData.ExLevel}LV]";
        GetText((int)Texts.PlusGoldText).text = _saviourGameData.LValue.ToString();
        GetText((int)Texts.PlusNumText).text = _data.LevelDatas[0].LValue.ToString();
        GetText((int)Texts.BuyCostText).text = _saviourGameData.BuyGold.ToString();
    }

    private void Update()
    {
        if (_saviourGameData.isLocked)
            return;
        if (_saviourGameData.Level <= 0)
            return;

        _lastRewardTime += Time.deltaTime;

        float percentage = _lastRewardTime / _data.TotalTime;
        float timeLeft = _data.TotalTime - _lastRewardTime;
        if (percentage >= 1)
        {
            _lastRewardTime = 0;
            Managers.Game.Gold += (int)(_saviourGameData.LValue * Managers.Game.ContinueInfo.WaitGold);
        }

        GetImage((int)Images.ProgressBarFill).fillAmount = percentage;
        GetText((int)Texts.ProgressTimeText).text = $"{(int)(timeLeft / 3600):D2}:{(int)(timeLeft / 60 % 60):D2}:{(int)(timeLeft % 60):D2}";
    }

    void OnClickLockButton()
    {
        _saviourGameData.isLocked = false;
        _lastRewardTime = 0;
        _saviourGameData.LValue = _data.LevelDatas[0].LValue;
        _saviourGameData.BuyGold = _data.LevelDatas[0].NextCost;
        Refresh();
    }

    void OnClickBuySquareButton()
    {
        if (Managers.Game.Gold < _saviourGameData.BuyGold)
            return;
        Managers.Game.Gold -= _saviourGameData.BuyGold;
        LevelUp();
    }

    void LevelUp(int levelPoint = 1)
    {
        _saviourGameData.Level += levelPoint;
        _saviourGameData.BuyGold += GetBuyGoldByLevel();
        _saviourGameData.LValue += levelPoint * _data.LevelDatas[0].LValue;

        if (_saviourGameData.Level == 10)
        {
            if (Managers.Game.SaviourLevelDictionary.TryGetValue(_id + 1, out SaviourGameData value))
            {
                value.isLocked = false;
                _action?.Invoke();
            }
        }
        Refresh();
    }

    int GetBuyGoldByLevel()
    {
        while (true)
        {
            if (_goldIndex >= _data.RaiseLevelDatas.Count || _saviourGameData.Level >= _data.RaiseLevelDatas[_goldIndex].Level)
                break;
            _goldIndex++;
        }

        return _data.RaiseLevelDatas[_goldIndex].NextCost;
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
