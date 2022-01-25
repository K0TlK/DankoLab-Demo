using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Game_Ctrl : MonoBehaviour
{
    [SerializeField] private DataIF_Ctrl heightText;
    private int height = 10;
    [SerializeField] private DataIF_Ctrl widthText;
    private int width = 16;
    [SerializeField] private DataIF_Ctrl colorsText;
    private int colors = 3;

    [SerializeField] private Block blockPrefab;
    [SerializeField] private EndGameShow endPrefab;

    [SerializeField] private Text Info;

    private Block[,] field;
    private int[] colorArr;
    private float _blockSize = -1;
    private float _heightUpd = 0;
    private int counterBlocks = 0;
    private int counterStep = 0;
    private DateTime startTime;


    private void Start()
    {
        _heightUpd = this.gameObject.GetComponent<RectTransform>().rect.height;
        DOTween.Init();
        DOTween.SetTweensCapacity(500, 312);
        StartGame();
    }

    private void Update()
    {
        if (_heightUpd != this.gameObject.GetComponent<RectTransform>().rect.height)
        {
            Resize();
            _heightUpd = this.gameObject.GetComponent<RectTransform>().rect.height;
        }

    }

    public void StartGame()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        height = heightText.GetData();
        width = widthText.GetData();
        colors = colorsText.GetData();

        colorArr = new int[colors];

        var rand = new System.Random();
        var usedColors = new HashSet<int>();

        for (int i = 0; i < colors; i++)
        {
            int id;
            do
            {
                id = rand.Next(ColorManager.Size());
            } while (!usedColors.Add(id));

            colorArr[i] = id;
        }

        Resize();

        field = new Block[height, width];

        for (int x = 0; x < height; x++)
            for (int y = 0; y < width; y++)
            {
                field[x, y] = Instantiate(blockPrefab, transform, false);
                field[x, y].Color = colorArr[rand.Next(colors)];
                field[x, y].x = x;
                field[x, y].y = y;
                field[x, y].UpdateBlock();

                float time = rand.Next(20);
                field[x, y].Anim_Appear(time / 10);
            }

        counterBlocks = 0;
        counterStep = 0;
        startTime = DateTime.Now;

        Info.text = "Блоков 0 из " + (height * width).ToString();
    }

    public void Resize()
    {
        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        _blockSize = rect.rect.width / width;

        if (_blockSize > rect.rect.height / height)
            _blockSize = rect.rect.height / height;

        this.gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(_blockSize, _blockSize);
        this.gameObject.GetComponent<GridLayoutGroup>().constraintCount = width;
    }

    public void BlockClick(int x, int y)
    {
        if (HaveMoves(x, y))
        {
            counterStep++;
            ClearNew(x, y, field[x, y].Color);
            gameObject.GetComponent<AudioSource>().Play();
            Info.text = "Блоков " + counterBlocks + " из " + (height * width).ToString();
            ClearEmpty();
            if (EndGame())
            {
                EndGameShow stat = Instantiate(endPrefab, 
                    gameObject.GetComponentInParent<Canvas>().
                    gameObject.GetComponent<Transform>());

                stat.SetBlock(counterBlocks, height * width);
                stat.SetBlockPer(counterBlocks, height * width);
                stat.SetSteps(counterStep);
                stat.SetTime(startTime);
            }
        }
        else
        {
            field[x, y].Anim_shake();
        }
    }

    public void ClearNew(int x, int y, int color)
    {
        counterBlocks++;
        field[x, y].Color = -1;
        field[x, y].NeedUpdate = true;

        if (x - 1 >= 0)
            if (field[x - 1, y].Color == color)
                ClearNew(x - 1, y, color);

        if (y - 1 >= 0)
            if (field[x, y - 1].Color == color)
                ClearNew(x, y - 1, color);

        if (x + 1 < height)
            if (field[x + 1, y].Color == color)
                ClearNew(x + 1, y, color);

        if (y + 1 < width)
            if (field[x, y + 1].Color == color)
                ClearNew(x, y + 1, color);
    }

    public void ClearEmpty()
    {
        for (int y = 0; y < width; y++)
            for (int x = height - 1; x >= 0; x--)
            {
                if (field[x, y].Color == -1)
                {
                    bool isempty = true;
                    bool isColored = true;
                    for (int i = x; i >= 0; i--)
                    {
                        if(isColored)
                        {
                            if (field[i, y].Color == -1)
                                isColored = false;
                        }
                        else
                        {
                            if (field[i, y].Color != -1)
                            {
                                isempty = false;
                                break;
                            }
                        }
                    }
                    if (isempty)
                        break;

                    int j = x;

                    for (int i = x; i >=0; i--)
                    {
                        if (field[i, y].Color == -1)
                            continue;
                        field[j, y].Color = field[i, y].Color;
                        field[j, y].NeedUpdate = true;
                        j--;
                    }

                    for (int i = 0; i < j + 1; i++)
                    {
                        field[i, y].Color = -1;
                        field[i, y].NeedUpdate = true;
                    }
                }
            }    

        for (int x = 0; x < height; x++)
            for (int y = 0; y < width; y++)
                if (field[x, y].NeedUpdate)
                    field[x, y].Anim_updColor();
    }

    private bool HaveMoves(int x, int y, int colorForLast = -1)
    {
        int color = field[x, y].Color;
        if (color == -1) return false;

        int points = 0;

        if (x - 1 >= 0)
            if (field[x - 1, y].Color == color)
                points++;

        if (y - 1 >= 0)
            if (field[x, y - 1].Color == color)
                points++;

        if (x + 1 < height)
            if (field[x + 1, y].Color == color)
                points++;

        if (y + 1 < width)
            if (field[x, y + 1].Color == color)
                points++;

        if (points >= 2)
            return true;

        if (colorForLast != -1)
            return false;
        else
            return HaveMovesNeighbor(x, y);
    }

    private bool HaveMovesNeighbor(int x, int y)
    {
        int color = field[x, y].Color;
        if (x - 1 >= 0)
            if (field[x - 1, y].Color == color)
                if (HaveMoves(x - 1, y, color))
                    return true;

        if (y - 1 >= 0)
            if (field[x, y - 1].Color == color)
                if (HaveMoves(x, y - 1, color))
                    return true;

        if (x + 1 < height)
            if (field[x + 1, y].Color == color)
                if (HaveMoves(x + 1, y, color))
                    return true;

        if (y + 1 < width)
            if (field[x, y + 1].Color == color)
                if (HaveMoves(x, y + 1, color))
                    return true;

        return false;
    }

    private bool EndGame()
    {
        for (int x = 0; x < height; x++)
            for (int y = 0; y < width; y++)
                if (HaveMoves(x, y))
                {
                    return false;
                }

        return true;
    }
}
