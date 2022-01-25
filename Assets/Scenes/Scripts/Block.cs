using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Block : MonoBehaviour
{
    //[HideInInspector]
    public int x;
    //[HideInInspector]
    public int y;
    [HideInInspector]
    public bool NeedUpdate = false;

    private int _color = 0;
    public int Color
    {
        get => _color;
        set
        {
            if ((value == -1 || value >= 0) && value < ColorManager.Size())
                _color = value;
            else
                throw new Exception("SetColor out of index");
        }
    }

    private Sequence sequence;

    public void UpdateBlock()
    {
        if (_color == -1)
            gameObject.GetComponent<Image>().color = UnityEngine.Color.white;
        else
            gameObject.GetComponent<Image>().color = ColorManager.GetColor(_color);
    }

    public void SetZero(Sprite EmptyBlock)
    {
        gameObject.GetComponent<Image>().sprite = EmptyBlock;
        gameObject.GetComponent<Image>().color = UnityEngine.Color.white;
    }

    public Color GetColor()
    {
        return gameObject.GetComponent<Image>().color;
    }

    public void Anim_Appear(float delay)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        transform.localScale = Vector2.zero;

        sequence.AppendInterval(delay);
        sequence.Append(transform.DOScale(1.2f, .5f));
        sequence.Append(transform.DOScale(1f, .5f));
    }

    public void Anim_updColor(bool kill = false)
    {
        if (_color == -1 && gameObject.GetComponent<Image>().color == UnityEngine.Color.white)
            return;

        if (kill)
        {
            if (sequence.active)
                UpdateBlock();
            sequence.Kill();
        }

        NeedUpdate = false;
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScaleX(0.85f, .5f));
        sequence.Join(transform.DOScaleY(1.2f, .6f));
        sequence.AppendInterval(.3f);
        sequence.Append(transform.DOScaleX(1f, .2f));
        sequence.Join(transform.DOScaleY(1f, .2f));

        sequence.AppendCallback(() =>
        {
            UpdateBlock();
        });
    }

    public void Anim_shake()
    {
        if (sequence.active)
            UpdateBlock();

        sequence.Kill();
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOShakePosition(0.5f, 3));
    }

    public void OnClick()
    {
        gameObject.GetComponentInParent<Game_Ctrl>().BlockClick(x, y);
    }

    private void OnDestroy()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}
