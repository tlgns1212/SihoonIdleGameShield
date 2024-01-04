using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        JewelAssembleGroup,
        JewelDisassembleGroup,
        Viewport,
    }
    enum Toggles
    {
        JewelAssemble,
        JewelDisassemble,
        JewelSell,
    }
    enum Buttons
    {
        JewelSort,
    }
    enum Images
    {
        AFirstImage,
        ASecondImage,
        AThirdImage,
        DFirstImage,
        DSecondImage,
        DThirdImage,
    }
    enum Texts
    {
        AFirstGradeText,
        ASecondGradeText,
        AThirdGradeText,
        DFirstGradeText,
        DSecondGradeText,
        DThirdGradeText,
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
        BindToggle(typeof(Toggles));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _scrollRect = Util.FindChild<ScrollRect>(gameObject);

        GetToggle((int)Toggles.JewelAssemble).gameObject.BindEvent(OnClickJewelAssemble);
        GetToggle((int)Toggles.JewelDisassemble).gameObject.BindEvent(OnClickJewelDisassemble);
        GetToggle((int)Toggles.JewelSell).gameObject.BindEvent(OnClickJewelSell);
        GetButton((int)Buttons.JewelSort).gameObject.BindEvent(OnClickJewelSort);

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
            ji.SetInfo(jgd, this, _scrollRect, RefreshAllItems, UpdateJewelImage);
        }

        Refresh();

        return true;
    }

    void RefreshAllItems()
    {
        foreach (UI_JewelItem item in _items)
        {
            item.Refresh();
        }
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        for (int i = _selectedItems.Count - 1; i >= 0; i--)
            _selectedItems[i].IsSelected = false;
        _selectedItems.Clear();

        RectTransform viewportRT = GetObject((int)GameObjects.Viewport).GetComponent<RectTransform>();
        if (_selectType == Define.JewelSelectType.Assemble)
        {
            GetObject((int)GameObjects.JewelDisassembleGroup).SetActive(false);
            GetObject((int)GameObjects.JewelAssembleGroup).SetActive(true);
            viewportRT.offsetMax = new Vector2(viewportRT.offsetMax.x, -310);
        }
        else if (_selectType == Define.JewelSelectType.Disassemble)
        {
            GetObject((int)GameObjects.JewelAssembleGroup).SetActive(false);
            GetObject((int)GameObjects.JewelDisassembleGroup).SetActive(true);
            viewportRT.offsetMax = new Vector2(viewportRT.offsetMax.x, -310);
        }
        else
        {
            GetObject((int)GameObjects.JewelAssembleGroup).SetActive(false);
            GetObject((int)GameObjects.JewelDisassembleGroup).SetActive(false);
            viewportRT.offsetMax = new Vector2(viewportRT.offsetMax.x, -160);
        }

        GetImage((int)Images.AFirstImage).gameObject.SetActive(false);
        GetImage((int)Images.ASecondImage).gameObject.SetActive(false);
        GetImage((int)Images.AThirdImage).gameObject.SetActive(false);
        GetImage((int)Images.DFirstImage).gameObject.SetActive(false);
        GetImage((int)Images.DSecondImage).gameObject.SetActive(false);
        GetImage((int)Images.DThirdImage).gameObject.SetActive(false);
    }

    void UpdateJewelImage()
    {
        if (_selectType == Define.JewelSelectType.Assemble)
        {
            if (_selectedItems.Count == 1)
            {
                GetImage((int)Images.AFirstImage).gameObject.SetActive(true);

                GetImage((int)Images.AFirstImage).sprite = Managers.Resource.Load<Sprite>(_selectedItems[0]._data.IconLabel);
                GetText((int)Texts.AFirstGradeText).text = _selectedItems[0]._data.Grade;
                // TODO Sort
            }
            else if (_selectedItems.Count == 2)
            {
                GetImage((int)Images.ASecondImage).gameObject.SetActive(true);
                GetImage((int)Images.AThirdImage).gameObject.SetActive(true);
                GetImage((int)Images.ASecondImage).sprite = Managers.Resource.Load<Sprite>(_selectedItems[1]._data.IconLabel);
                GetText((int)Texts.ASecondGradeText).text = _selectedItems[1]._data.Grade;

                GetImage((int)Images.AThirdImage).sprite = Managers.Resource.Load<Sprite>(_selectedItems[1]._data.IconLabel);
                GetText((int)Texts.AThirdGradeText).text = ((Define.JewelGrade)_selectedItems[1]._jewelGameData.GradeNum - 1).ToString();
            }
            else
            {
                GetImage((int)Images.AFirstImage).gameObject.SetActive(false);
                GetImage((int)Images.ASecondImage).gameObject.SetActive(false);
                GetImage((int)Images.AThirdImage).gameObject.SetActive(false);
            }
        }
        else if (_selectType == Define.JewelSelectType.Disassemble)
        {
            if (_selectedItems.Count == 1)
            {
                GetImage((int)Images.DFirstImage).gameObject.SetActive(true);
                GetImage((int)Images.DSecondImage).gameObject.SetActive(true);
                GetImage((int)Images.DThirdImage).gameObject.SetActive(true);

                GetImage((int)Images.DFirstImage).sprite = Managers.Resource.Load<Sprite>(_selectedItems[0]._data.IconLabel);
                GetText((int)Texts.DFirstGradeText).text = _selectedItems[0]._data.Grade;

                GetImage((int)Images.DSecondImage).sprite = Managers.Resource.Load<Sprite>(_selectedItems[0]._data.IconLabel);
                GetText((int)Texts.DSecondGradeText).text = ((Define.JewelGrade)_selectedItems[0]._jewelGameData.GradeNum + 1).ToString();
                GetImage((int)Images.DThirdImage).sprite = Managers.Resource.Load<Sprite>(_selectedItems[0]._data.IconLabel);
                GetText((int)Texts.DThirdGradeText).text = ((Define.JewelGrade)_selectedItems[0]._jewelGameData.GradeNum + 1).ToString();
            }
            else
            {
                GetImage((int)Images.DFirstImage).gameObject.SetActive(false);
                GetImage((int)Images.DSecondImage).gameObject.SetActive(false);
                GetImage((int)Images.DThirdImage).gameObject.SetActive(false);
            }
        }
    }

    void OnClickJewelAssemble()
    {
        if (_selectType == Define.JewelSelectType.Assemble)
            _selectType = Define.JewelSelectType.Nothing;
        else
            _selectType = Define.JewelSelectType.Assemble;
        Refresh();
    }

    void OnClickJewelDisassemble()
    {
        if (_selectType == Define.JewelSelectType.Disassemble)
            _selectType = Define.JewelSelectType.Nothing;
        else
            _selectType = Define.JewelSelectType.Disassemble;
        Refresh();
    }

    void OnClickJewelSell()
    {
        if (_selectType == Define.JewelSelectType.Sell)
            _selectType = Define.JewelSelectType.Nothing;
        else
            _selectType = Define.JewelSelectType.Sell;
        Refresh();
    }

    void OnClickJewelSort()
    {
        // TODOTODO 정렬하기
        // _items = _items.OrderBy(x => x._data)

        Refresh();
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
