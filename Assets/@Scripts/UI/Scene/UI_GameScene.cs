using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        ToggleGroup,
        CheckShieldImage,
        ShieldToggleRedDotObject,
        CheckSaviourImage,
        SaviourToggleRedDotObject,
        CheckAccessoriesImage,
        AccessoriesToggleRedDotObject,
        CheckEquipmentImage,
        EquipmentToggleRedDotObject,
        CheckFriendImage,
        FriendToggleRedDotObject,
        CheckJewelImage,
        JewelToggleRedDotObject,
        CheckDungeonImage,
        DungeonToggleRedDotObject,
        CheckShopImage,
        ShopToggleRedDotObject,
    }

    enum Buttons
    {
        SettingButton,
    }

    enum Toggles
    {
        ShieldToggle,
        SaviourToggle,
        AccessoriesToggle,
        EquipmentToggle,
        FriendToggle,
        JewelToggle,
        DungeonToggle,
        ShopToggle,
    }

    enum Texts
    {
        ShieldText,
        SaviourText,
        AccessoriesText,
        EquipmentText,
        FriendText,
        JewelText,
        DungeonText,
        ShopText,
    }

    #endregion


    UI_ResourcesPopup _resourcesPopupUI;
    UI_AccessoriesPopup _accessoriesPopupUI;
    bool _isSelectedAccessories = false;
    UI_DungeonPopup _dungeonPopupUI;
    bool _isSelectedDungeon = false;
    UI_EquipmentPopup _equipmentPopupUI;
    bool _isSelectedEquipment = false;
    UI_FriendPopup _friendPopupUI;
    bool _isSelectedFriend = false;
    UI_JewelPopup _jewelPopupUI;
    bool _isSelectedJewel = false;
    UI_SaviourPopup _saviourPopupUI;
    bool _isSelectedSaviour = false;
    UI_ShieldPopup _shieldPopupUI;
    bool _isSelectedShield = false;
    UI_ShopPopup _shopPopupUI;
    bool _isSelectedShop = false;


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Application.targetFrameRate = 60;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindToggle(typeof(Toggles));
        BindText(typeof(Texts));

        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);

        GetToggle((int)Toggles.AccessoriesToggle).gameObject.BindEvent(OnClickAccessoriesToggle);
        GetToggle((int)Toggles.DungeonToggle).gameObject.BindEvent(OnClickDungeonToggle);
        GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle((int)Toggles.FriendToggle).gameObject.BindEvent(OnClickFriendToggle);
        GetToggle((int)Toggles.JewelToggle).gameObject.BindEvent(OnClickJewelToggle);
        GetToggle((int)Toggles.SaviourToggle).gameObject.BindEvent(OnClickSaviourToggle);
        GetToggle((int)Toggles.ShieldToggle).gameObject.BindEvent(OnClickShieldToggle);
        GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);


        _resourcesPopupUI = Managers.UI.ShowPopupUI<UI_ResourcesPopup>();
        _accessoriesPopupUI = Managers.UI.ShowPopupUI<UI_AccessoriesPopup>();
        _dungeonPopupUI = Managers.UI.ShowPopupUI<UI_DungeonPopup>();
        _equipmentPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentPopup>();
        _friendPopupUI = Managers.UI.ShowPopupUI<UI_FriendPopup>();
        _jewelPopupUI = Managers.UI.ShowPopupUI<UI_JewelPopup>();
        _saviourPopupUI = Managers.UI.ShowPopupUI<UI_SaviourPopup>();
        _shieldPopupUI = Managers.UI.ShowPopupUI<UI_ShieldPopup>();
        _shopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();


        _equipmentPopupUI.SetExit(OnClickShieldToggle);
        _shopPopupUI.SetExit(OnClickShieldToggle);

        TogglesInit();
        GetToggle((int)Toggles.ShieldToggle).isOn = true;
        OnClickShieldToggle();


        Refresh();

        return true;
    }

    void TogglesInit()
    {
        #region 팝업 초기화
        _accessoriesPopupUI.gameObject.SetActive(false);
        _dungeonPopupUI.gameObject.SetActive(false);
        _equipmentPopupUI.gameObject.SetActive(false);
        _friendPopupUI.gameObject.SetActive(false);
        _jewelPopupUI.gameObject.SetActive(false);
        _saviourPopupUI.gameObject.SetActive(false);
        _shieldPopupUI.gameObject.SetActive(false);
        _shopPopupUI.gameObject.SetActive(false);
        #endregion

        #region 토글 버튼 초기화
        // 재 클릭 방지 트리거 초기화
        _isSelectedAccessories = false;
        _isSelectedDungeon = false;
        _isSelectedEquipment = false;
        _isSelectedFriend = false;
        _isSelectedJewel = false;
        _isSelectedSaviour = false;
        _isSelectedShield = false;
        _isSelectedShop = false;

        // 버튼 레드닷 초기화
        GetObject((int)GameObjects.AccessoriesToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.DungeonToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.EquipmentToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.FriendToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.JewelToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.SaviourToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.ShieldToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.ShopToggleRedDotObject).SetActive(false);

        // 선택 토글 아이콘 초기화
        GetObject((int)GameObjects.CheckAccessoriesImage).SetActive(false);
        GetObject((int)GameObjects.CheckDungeonImage).SetActive(false);
        GetObject((int)GameObjects.CheckEquipmentImage).SetActive(false);
        GetObject((int)GameObjects.CheckFriendImage).SetActive(false);
        GetObject((int)GameObjects.CheckJewelImage).SetActive(false);
        GetObject((int)GameObjects.CheckSaviourImage).SetActive(false);
        GetObject((int)GameObjects.CheckShieldImage).SetActive(false);
        GetObject((int)GameObjects.CheckShopImage).SetActive(false);

        // 선택 토글 텍스트 초기화
        GetText((int)Texts.AccessoriesText).gameObject.SetActive(false);
        GetText((int)Texts.DungeonText).gameObject.SetActive(false);
        GetText((int)Texts.EquipmentText).gameObject.SetActive(false);
        GetText((int)Texts.FriendText).gameObject.SetActive(false);
        GetText((int)Texts.JewelText).gameObject.SetActive(false);
        GetText((int)Texts.SaviourText).gameObject.SetActive(false);
        GetText((int)Texts.ShieldText).gameObject.SetActive(false);
        GetText((int)Texts.ShopText).gameObject.SetActive(false);
        #endregion
    }

    void Refresh()
    {


        // 토글 선택 시 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ToggleGroup).GetComponent<RectTransform>());
    }

    void ShowUI(GameObject contentPopup, TMP_Text text, GameObject obj2)
    {
        TogglesInit();

        contentPopup.SetActive(true);
        text.gameObject.SetActive(true);
        obj2.SetActive(true);

        Refresh();
    }

    void OnClickAccessoriesToggle()
    {
        if (_isSelectedAccessories == true)
            return;

        GetToggle((int)Toggles.AccessoriesToggle).isOn = true;
        ShowUI(_accessoriesPopupUI.gameObject, GetText((int)Texts.AccessoriesText), GetObject((int)GameObjects.CheckAccessoriesImage));
        _isSelectedAccessories = true;
    }

    void OnClickDungeonToggle()
    {
        if (_isSelectedDungeon == true)
            return;

        GetToggle((int)Toggles.DungeonToggle).isOn = true;
        ShowUI(_dungeonPopupUI.gameObject, GetText((int)Texts.DungeonText), GetObject((int)GameObjects.CheckDungeonImage));
        _isSelectedDungeon = true;
    }

    void OnClickEquipmentToggle()
    {
        if (_isSelectedEquipment == true)
            return;

        GetToggle((int)Toggles.EquipmentToggle).isOn = true;
        ShowUI(_equipmentPopupUI.gameObject, GetText((int)Texts.EquipmentText), GetObject((int)GameObjects.CheckEquipmentImage));
        _isSelectedEquipment = true;
    }

    void OnClickFriendToggle()
    {
        if (_isSelectedFriend == true)
            return;

        GetToggle((int)Toggles.FriendToggle).isOn = true;
        ShowUI(_friendPopupUI.gameObject, GetText((int)Texts.FriendText), GetObject((int)GameObjects.CheckFriendImage));
        _isSelectedFriend = true;
    }

    void OnClickJewelToggle()
    {
        if (_isSelectedJewel == true)
            return;

        GetToggle((int)Toggles.JewelToggle).isOn = true;
        ShowUI(_jewelPopupUI.gameObject, GetText((int)Texts.JewelText), GetObject((int)GameObjects.CheckJewelImage));
        _isSelectedJewel = true;
    }

    void OnClickSaviourToggle()
    {
        if (_isSelectedSaviour == true)
            return;

        GetToggle((int)Toggles.SaviourToggle).isOn = true;
        ShowUI(_saviourPopupUI.gameObject, GetText((int)Texts.SaviourText), GetObject((int)GameObjects.CheckSaviourImage));
        _isSelectedSaviour = true;
    }

    void OnClickShieldToggle()
    {
        if (_isSelectedShield == true)
            return;

        GetToggle((int)Toggles.ShieldToggle).isOn = true;
        ShowUI(_shieldPopupUI.gameObject, GetText((int)Texts.ShieldText), GetObject((int)GameObjects.CheckShieldImage));
        _isSelectedShield = true;
    }

    void OnClickShopToggle()
    {
        if (_isSelectedShop == true)
            return;

        GetToggle((int)Toggles.ShopToggle).isOn = true;
        ShowUI(_shopPopupUI.gameObject, GetText((int)Texts.ShopText), GetObject((int)GameObjects.CheckShopImage));
        _isSelectedShop = true;
    }

    void OnClickSettingButton()
    {
        // TODO Temp임시로 게임 초기화로 해 놓았음
        Managers.Game.RefreshGame();
        Managers.Game.Gold = 100000;
    }

    void Awake()
    {
        Init();
    }

    void Start()
    {

    }
}
