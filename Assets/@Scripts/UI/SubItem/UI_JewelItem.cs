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
        GradeImage,
    }

    #endregion

    ScrollRect _scrollRect;
    bool _isDrag = false;
    Action _action;

    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        Refresh();

        return true;
    }

    public void SetInfo(int jewelID, ScrollRect scrollRect, Action callback)
    {
        _scrollRect = scrollRect;
        _action = callback;
        Data.JewelData data = Managers.Data.JewelDic[jewelID];

        GetImage((int)Images.JewelImage).sprite = Managers.Resource.Load<Sprite>(data.IconLabel);
        // TODO Grade 메기기
        // GetImage((int)Images.GradeImage).sprite =

        Refresh();
    }

    public void Refresh()
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
