using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene
{
    protected override void Init()
    {
        // Managers.Resource.Instantiate("UI_StartScene").GetOrAddComponent<UI_StartScene>();
        base.Init();
        SceneType = Define.Scene.TitleScene;
        Application.targetFrameRate = 60;
    }


    public override void Clear()
    {
    }

}
