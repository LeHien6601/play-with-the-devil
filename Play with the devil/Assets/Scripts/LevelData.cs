using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public int row;
    public int column;
    public int numberOfFakeCards;
    public int numberOfFunctionalCards;
    public int limitContent;
    public int limitColor;
    public float showCardTime;
    public int numberOfSwapedPairs;
    public int numberOfSouls;
}