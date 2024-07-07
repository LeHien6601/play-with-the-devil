using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private bool isFaceDown;
    [SerializeField] private Vector2 cardSize;
    private void Awake()
    {
        spriteRenderer.sprite = (isFaceDown) ? backSprite : frontSprite;
    }
    public void SelectCard()
    {

    }

    public void TurnCardUp()
    {
        isFaceDown = false;
        spriteRenderer.sprite = frontSprite;
    }

    private void OnMouseDown()
    {
        Debug.Log("Click mouse on card");
        if (isFaceDown)
        {
            TurnCardUp();
        }
    }
}
