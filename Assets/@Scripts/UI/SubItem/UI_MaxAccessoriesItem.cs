using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MaxAccessoriesItem : UI_Base
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
        DescriptionText,
        ItemEffectText,
        PlusNumText,
        BuyCostText,
    }

    enum Buttons
    {
        BuySquareButton,
    }
    #endregion

    ScrollRect _scrollRect;
    bool _isDrag = false;
    string _itemEffectString;
    LevelData _levelData;

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

    public void SetInfo(int accesoriesID, ScrollRect scrollRect)
    {
        _scrollRect = scrollRect;
        Data.AccessoriesData data = Managers.Data.AccessoriesDic[accesoriesID];

        GetText((int)Texts.TitleText).text = data.TitleText;
        _itemEffectString = data.ItemEffectText;

        if (Managers.Game.AccLevelDictionary.TryGetValue(accesoriesID, out LevelData accLevel))
        {
            _levelData = accLevel;
        }
        else
        {
            _levelData = new LevelData() { isOpen = true, Level = 0 };
            Managers.Game.AccLevelDictionary.Add(accesoriesID, _levelData);
        }
        if (_levelData.isOpen)
        {
            // TODO 잠금 풀기(지금 잠금이 없음 만들어야 함)
        }
        int maxLevel = data.LevelDatas.Count;
        GetText((int)Texts.LvText).text = $"LV{1}(MAX{maxLevel})";
        GetText((int)Texts.DescriptionText).text = $"{_itemEffectString} 단계 증가";
        GetText((int)Texts.ItemEffectText).text = $"{1}{data.ItemEffectNumText}";

        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(data.IconLabel);
        // TODO 레벨에 맞게 구매 비용, 증가하는 공격력 등 해줘야 함.

        // GetText((int)Texts.PlusNumText).text =

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
