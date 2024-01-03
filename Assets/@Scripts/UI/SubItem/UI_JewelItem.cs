using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JewelItem : UI_Base
{
    #region Enum
    enum Images
    {
        JewelImage,
        IsUsedImage,
        CheckedImage
    }

    enum Texts
    {
        GradeText,
    }

    #endregion

    int _id;
    int _dataID;
    ScrollRect _scrollRect;
    Data.JewelData _data;
    JewelGameData _jewelGameData;
    UI_JewelPopup _parent;
    bool _isDrag = false;
    Action _action;
    bool _isSelected = false;
    public bool IsSelected { get { return _isSelected; }
        set {
            _isSelected = value;
            if (_isSelected)
                _parent._selectedItems.Add(this);
            else
            {
                if (_parent._selectedItems.Contains(this))
                    _parent._selectedItems.Remove(this);
            }
            Refresh();
        }
    }

    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        gameObject.BindEvent(OnClickJewelItem);

        Refresh();

        return true;
    }

    public void SetInfo(JewelGameData jgd, UI_JewelPopup parent, ScrollRect scrollRect, Action callback)
    {
        _id = jgd.ID;
        _parent = parent;
        _dataID = jgd.DataID;
        _jewelGameData = jgd;
        _scrollRect = scrollRect;
        _action = callback;
        _data = Managers.Data.JewelDic[jgd.DataID];

        GetImage((int)Images.JewelImage).sprite = Managers.Resource.Load<Sprite>(_data.IconLabel);
        GetText((int)Texts.GradeText).text = _data.Grade;


        if (_jewelGameData.isUsed == false)
        {
            GetImage((int)Images.IsUsedImage).gameObject.SetActive(false);
        }

        Refresh();
    }

    public void Refresh()
    {
        if (!IsSelected)
            GetImage((int)Images.CheckedImage).gameObject.SetActive(false);
        else
            GetImage((int)Images.CheckedImage).gameObject.SetActive(true);
    }

    void OnClickJewelItem()
    {
        switch (_parent._selectType)
        {
            case Define.JewelSelectType.Nothing:
                // TODO Show Description Popup
                break;
            case Define.JewelSelectType.Assemble:
                // TODO Click And check and go Up
                if (IsSelected) {
                    IsSelected = false;
                }
                else if(_parent._selectedItems.Count >= 1)
                {
                    _parent._selectedItems[0].IsSelected = false;
                    _parent._selectedItems.Clear();
                    IsSelected = true;
                    _action?.Invoke();
                }
                else
                    IsSelected = true;
                break;
            case Define.JewelSelectType.Disassemble:
                // TODO Click and check and show sepate two
                if (IsSelected)
                {
                    IsSelected = false;
                }
                else if (_parent._selectedItems.Count >= 1)
                {
                    _parent._selectedItems[0].IsSelected = false;
                    _parent._selectedItems.Clear();
                    IsSelected = true;
                    _action?.Invoke();
                }
                else
                    IsSelected = true;
                break;
            case Define.JewelSelectType.Sell:
                // TODO Click and check all
                if (IsSelected)
                {
                    IsSelected = false;
                }
                else
                    IsSelected = true;
                break;
            case Define.JewelSelectType.Sort:
                // TODO No Click
                break;
        }
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
