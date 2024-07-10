using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]
public class CardData : ScriptableObject
{
    public string cardContent;
    public string description;
    public CardType cardType;
    public enum CardType
    {
        Normal, Functional
    }
}
