using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField]
    private Color[] BlockColors;

    private static Color[] _blockColors;

    void Awake()
    {
        _blockColors = BlockColors;

        //BlockColors = null;
    }

    public static Color GetColor(int id)
    {
        return _blockColors[id];
    }

    public static int Size()
    {
        return _blockColors.Length;
    }
}
