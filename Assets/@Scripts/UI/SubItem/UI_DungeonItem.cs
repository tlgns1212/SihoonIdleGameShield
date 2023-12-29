using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DungeonItem : UI_Base
{
    #region Enum
    enum Images
    {
        ItemSquareIcon,
        ItemIcon,
    }

    enum Texts
    {
        TitleText,
        VisitedText,
        VisitedLeftText,
        VisitText,
    }

    enum Buttons
    {
        VisitButton,
    }
    #endregion

    ScrollRect _scrollRect;
    bool _isDrag = false;

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

        Refresh();

        return true;
    }

    public void SetInfo(int dungeonID, ScrollRect scrollRect)
    {
        _scrollRect = scrollRect;
        Data.DungeonData data = Managers.Data.DungeonDic[dungeonID];

        GetText((int)Texts.TitleText).text = data.TitleText;
        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(data.IconLabel);
        GetText((int)Texts.VisitedText).text = data.MaxVisitText;
        // TODO : 현재 출입 횟수 지정하기
        GetText((int)Texts.VisitedLeftText).text = $"10/{data.MaxVisitNum}";

        if (data.LockOpenLevel <= Managers.Game.UserLevel)
        {
            // TODO 잠금 풀기(지금 잠금이 없음 만들어야 함)
        }

        Refresh();
    }

    void Refresh()
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
