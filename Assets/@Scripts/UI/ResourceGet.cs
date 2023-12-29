using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGet : MonoBehaviour
{
    TextMeshPro _amountText;
    SpriteRenderer _spriteRenderer;

    public void SetInfo(Vector2 pos, Transform parent = null, Define.ResourceType rType = Define.ResourceType.Gold, string rAmount = "0")
    {
        _amountText = GetComponentInChildren<TextMeshPro>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        transform.position = pos + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.2f));

        switch (rType)
        {
            case Define.ResourceType.Gold:
                _spriteRenderer.sprite = Managers.Resource.Load<Sprite>("GUI_21.sprite");
                break;
            case Define.ResourceType.Mana:
                _spriteRenderer.sprite = Managers.Resource.Load<Sprite>("GUI_26.sprite");
                break;
            case Define.ResourceType.DimensionEnergy:
                _spriteRenderer.sprite = Managers.Resource.Load<Sprite>("GUI_20.sprite");
                break;
            case Define.ResourceType.Ruby:
                _spriteRenderer.sprite = Managers.Resource.Load<Sprite>("GUI_22.sprite");
                break;
        }

        _amountText.text = rAmount;
        _amountText.alpha = 1;
        if (parent != null)
        {
            GetComponentInChildren<MeshRenderer>().sortingOrder = 321;
        }
        _spriteRenderer.sortingOrder = 321;
        DoAnimation();
    }

    private void DoAnimation()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(transform.position + Vector3.up * 0.2f, 0.3f).SetEase(Ease.Linear))
            .Join(_amountText.DOFade(0, 0.3f).SetEase(Ease.InQuint))
            .OnComplete(() =>
            {
                Managers.Resource.Destroy(gameObject);
            });
    }
}
