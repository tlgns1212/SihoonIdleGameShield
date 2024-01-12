using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipItem : UI_Base
{
    // TODO UI_EquipItem
    #region UI 기능 리스트
    // 정보 갱신
    // GradeBackgroundImage : 장비 등급의 테두리 (색상 변경)
    // - 일반(Common) : #AC9B83
    // - 고급(Uncommon)  : #73EC4E
    // - 희귀(Rare) : #0F84FF
    // - 유일(Epic) : #B740EA
    // - 전설(Legendary) : #F19B02
    // - 신화(Myth) : #FC2302
    // EnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 이미지 색깔 변경
    // - 유일(Epic) : #9F37F2
    // - 전설(Legendary) : #F67B09
    // - 신화(Myth) : #F1331A
    // EnforceValueText : 유일 +1 등의 등급 벨류
    // EquipmentImage : 장비의 아이콘
    // EquipmentLevelValueText : 장비의 현재 레벨
    // EquipmentRedDotObject : 장비가 강화가 가능할때 출력
    // NewTextObject : 장비를 처음 습득했을때 출력
    // EquippedObject : 합성 팝업에서 착용장비 표시용
    // SelectObject : 합성 팝업에서 장비 선택 표시용
    // LockObject : 합성 팝업에서 선택 불가 표시용
    #endregion

    #region Enum
    enum GameObjects
    {
        EquipmentRedDotObject,
        NewTextObject,
        EquippedObject,
        SelectObject,
        LockObject,
        SpecialImage,
        GetEffectObject,
    }

    enum Texts
    {
        EquipmentLevelValueText,
        EquipmentEnforceValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
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

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetObject((int)GameObjects.GetEffectObject).SetActive(false);
        gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        gameObject.BindEvent(OnClickEquipItemButton);

        return true;
    }

    // public void SetInfo(Equipment item, UI_ItemParentType parentType, ScrollRect scrollRect = null)
    public void SetInfo(ScrollRect scrollRect = null)
    {
        _scrollRect = scrollRect;
    }

    void OnClickEquipItemButton()
    {
        if (_isDrag)
            return;
    }

    public void OnDrag(BaseEventData baseEventData)
    {
        // if (_parentType == UI_ItemParentType.GachaResultPopup)
        //     return;
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        // if (_parentType == UI_ItemParentType.GachaResultPopup)
        //     return;

        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        // if (_parentType == UI_ItemParentType.GachaResultPopup)
        //     return;
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);
    }
}
