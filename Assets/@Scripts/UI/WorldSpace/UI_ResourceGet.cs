using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResourceGet : UI_Base
{
    enum Images
    {
        Image
    }
    enum Texts
    {
        Text
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

    public void SetInfo(string image, string msg)
    {
        // 메시지 변경
        transform.localScale = Vector3.one;
        GetImage((int)Images.Image).sprite = Managers.Resource.Load<Sprite>(image);
        GetText((int)Texts.Text).text = msg;
        Refresh();
    }

    void Refresh()
    {


    }


    public void OnEnable()
    {
        PopupOpenAnimation(gameObject);
    }
}
