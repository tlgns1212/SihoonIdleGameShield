using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        Slider,
    }

    enum Buttons
    {
        StartButton,
    }

    enum Texts
    {
        TouchStartText,
    }
    #endregion


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Application.targetFrameRate = 60;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = 0;
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
            Managers.Scene.LoadScene(Define.Scene.GameScene, transform);
        });
        GetButton((int)Buttons.StartButton).GetComponent<Button>().gameObject.SetActive(false);

        return true;
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");
            GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = (float)count / totalCount;
            if (count == totalCount)
            {
                Managers.Data.Init();

                // var result = Managers.Resource.gogogogo();
                // foreach (var res in result)
                // {
                //     print(res);
                // }
                Managers.Game.LoadGame();
                GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
                StartTextAnimation();
            }
        });
    }

    void StartTextAnimation()
    {
        GetText((int)Texts.TouchStartText).DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).Play();
    }
}
