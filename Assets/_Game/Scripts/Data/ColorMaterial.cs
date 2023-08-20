using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum ColorType
{
    INVISIBLE = 0,
    RED = 1,
    BLUE = 2,
    GREEN = 3,
    ORANGE = 4,
    PURPLE = 5,
    MAX_COLOUR = 6
}

[CreateAssetMenu(menuName = "ColorData")]
public class ColorMaterial : ScriptableObject
{
    [SerializeField] private Material[] colourMaterial = new Material[(int)ColorType.MAX_COLOUR];
    public Material GetMat(ColorType color)
    {
        return colourMaterial[(int)color];
    }
}
