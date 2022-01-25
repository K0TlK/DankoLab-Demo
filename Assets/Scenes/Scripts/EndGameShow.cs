using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndGameShow : MonoBehaviour
{
    private Sequence sequence;

    [SerializeField] private Text block;
    [SerializeField] private Text blockPer;
    [SerializeField] private Text steps;
    [SerializeField] private Text time;

    private void Start()
    {
        sequence = DOTween.Sequence();
        transform.localScale = Vector2.zero;

        sequence.Append(transform.DOScale(1.2f, .6f));
        sequence.Append(transform.DOScale(1f, .3f));
    }

    public void SetBlock(int count, int max)
    {
        block.text = "������: " + count + " �� " + max;
    }

    public void SetBlockPer(int count, int max)
    {
        float per = (float)count / (float)max;
        blockPer.text = "�������: " + (Math.Round(per, 4) * 100).ToString() + "%";
    }

    public void SetSteps(int Steps)
    {
        steps.text = "�����: " + Steps;
    }
    public void SetTime(DateTime start)
    {
        TimeSpan dif = DateTime.Now - start;
        time.text = "�����: " + dif;
    }

    public void OnDestroy()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}
