using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NoMaxAccessoriesItem : UI_Base
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
    int _id;
    bool _isDrag = false;
    string _itemEffectString;
    LevelData _levelData;
    List<Data.LevelData> _raiseLevelData = new List<Data.LevelData>();

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

        GetButton((int)Buttons.BuySquareButton).gameObject.BindEvent(HandleOnClickBuySquareButton);

        Refresh();

        return true;
    }

    public void SetInfo(int accessoriesID, ScrollRect scrollRect)
    {
        _id = accessoriesID;
        _scrollRect = scrollRect;
        Data.AccessoriesData data = Managers.Data.AccessoriesDic[accessoriesID];
        _raiseLevelData = data.RaiseLevelDatas;

        GetText((int)Texts.TitleText).text = data.TitleText;
        _itemEffectString = data.ItemEffectText;

        if (Managers.Game.AccLevelDictionary.TryGetValue(accessoriesID, out LevelData accLevel))
        {
            _levelData = accLevel;
        }
        else
        {
            _levelData = new LevelData() { isOpen = true, Level = 0, Value = 0 };
            Managers.Game.AccLevelDictionary.Add(accessoriesID, _levelData);
        }
        if (_levelData.isOpen)
        {
            // TODO 잠금 풀기(지금 잠금이 없음 만들어야 함)
        }
        GetText((int)Texts.LvText).text = $"LV. {_levelData.Level}";
        GetText((int)Texts.ItemEffectText).text = $"{_itemEffectString} {data.LevelDatas[_levelData.Level]}%증가";
        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>(data.IconLabel);
        // TODO 레벨에 맞게 구매 비용, 증가하는 공격력 등 해줘야 함.
        Refresh();
    }
    //TODOTODOTODOTODO 여기서 RaiseRate에 따라서 점차적으로 증가하는거 하는중, Level 하나씩 올라가는건 대략적으로 했음, 한번 확인 부탁
    public void ChangeLevel(int levelPlus = 1)
    {
        print(Managers.Game.AccLevelDictionary[_id].Level);
        _levelData.Level += levelPlus;
        print(Managers.Game.AccLevelDictionary[_id].Level);
        print(Managers.Game.AccLevelDictionary[_id].Value);
        int value = Managers.Game.AccLevelDictionary[_id].Value;
        GetText((int)Texts.LvText).text = $"LV. {_levelData.Level}";
        int raiseRate = 0;
        foreach (Data.LevelData lvlData in _raiseLevelData)
        {
            raiseRate = lvlData.LValue;
            if (lvlData.Level > _levelData.Level)
            {
                break;
            }
        }
        value += raiseRate;
        print(Managers.Game.AccLevelDictionary[_id].Value);
        GetText((int)Texts.ItemEffectText).text = $"{_itemEffectString} {value}%증가";
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

    void HandleOnClickBuySquareButton()
    {
        ChangeLevel();
    }
    #endregion
}
