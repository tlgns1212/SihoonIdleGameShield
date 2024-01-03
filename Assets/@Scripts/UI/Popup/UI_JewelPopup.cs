using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JewelPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        JewelTab,
        JewelContent,
        ToggleGroup,
    }
    enum Toggles
    {
        JewelAssemble,
        JewelDisassemble,
        JewelSell,
        JewelSort,
    }
    #endregion

    ScrollRect _scrollRect;
    List<UI_JewelItem> _items = new List<UI_JewelItem>();
    public List<UI_JewelItem> _selectedItems = new List<UI_JewelItem>();
    public Define.JewelSelectType _selectType = Define.JewelSelectType.Nothing;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        _scrollRect = Util.FindChild<ScrollRect>(gameObject);

        GetToggle((int)Toggles.JewelAssemble).gameObject.BindEvent(OnClickJewelAssemble);
        GetToggle((int)Toggles.JewelDisassemble).gameObject.BindEvent(OnClickJewelDisassemble);
        GetToggle((int)Toggles.JewelSell).gameObject.BindEvent(OnClickJewelSell);
        GetToggle((int)Toggles.JewelSort).gameObject.BindEvent(OnClickJewelSort);

        GetObject((int)GameObjects.JewelTab).BindEvent(null, OnDrag, Define.UIEvent.Drag);
        GetObject((int)GameObjects.JewelTab).BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        GetObject((int)GameObjects.JewelTab).BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        if (Managers.Game.JewelLevels.Count < 5)
        {
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < UnityEngine.Random.Range(1, 3); j++)
                {
                    Define.JewelGrade gradeNum = Util.ParseEnum<Define.JewelGrade>(Managers.Data.JewelDic[10001 + i].Grade);
                    JewelGameData jgd = new JewelGameData() { ID = Managers.Game.JewelNewID, DataID = 10001 + i, isUsed = false, LValue = 0, GradeNum = (int)gradeNum };

                    Managers.Game.JewelLevels.Add(jgd);
                }
            }
        }


        foreach (JewelGameData jgd in Managers.Game.JewelLevels)
        {
            UI_JewelItem ji = Managers.UI.MakeSubItem<UI_JewelItem>(GetObject((int)GameObjects.JewelContent).transform);
            _items.Add(ji);
            ji.SetInfo(jgd, this, _scrollRect, () =>
            {
                foreach (UI_JewelItem item in _items)
                {
                    item.Refresh();
                }
            });
        }

        Refresh();

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {

    }

    void OnClickJewels()
    {

    }

    void OnClickJewelAssemble()
    {

    }

    void OnClickJewelDisassemble()
    {

    }

    void OnClickJewelSell()
    {

    }

    void OnClickJewelSort()
    {

    }

    #region 버튼 스크롤 대응
    public void OnDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
    #endregion
}
