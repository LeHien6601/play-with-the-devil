using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private bool isFaceDown;
    [SerializeField] private Vector2 cardSize;
    [SerializeField] private Canvas canvas;
    private Vector3 initialScale;
    private void Awake()
    {
        spriteRenderer.sprite = (isFaceDown) ? backSprite : frontSprite;
        canvas.worldCamera = Camera.main;
        initialScale = gameObject.transform.localScale;
    }
    public void SelectCard()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) { TurnCardUp(); }
    }

    public void TurnCardUp()
    {
        isFaceDown = false;
        animator.SetTrigger("TurnUp");
        spriteRenderer.sprite = frontSprite;
    }
    private void OnMouseEnter()
    {
        transform.localScale = initialScale * 1.05f;
        //play sfx
    }
    private void OnMouseExit()
    {
        transform.localScale = initialScale;
    }
}
