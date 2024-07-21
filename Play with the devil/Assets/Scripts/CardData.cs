using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]
public class CardData : ScriptableObject
{
    public string cardContent;
    public string description;
    public Color color = Color.white;
    public CardType cardType;
    public enum CardType
    {
        Normal, Functional
    }
    public Color[] cardColors = { Color.white, Color.blue, Color.green, Color.yellow, Color.red, Color.black};

}
