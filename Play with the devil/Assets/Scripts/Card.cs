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
    [SerializeField] protected GameObject border;
    protected bool isSelected = false;
    private Vector3 initialScale;
    private void Awake()
    {
        spriteRenderer.sprite = (isFaceDown) ? backSprite : frontSprite;
        canvas.worldCamera = Camera.main;
        initialScale = gameObject.transform.localScale;
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
    public void Deselect()
    {
        isSelected = false;
        border.SetActive(false);
        transform.localScale = initialScale;
    }
    private void OnMouseEnter()
    {
        transform.localScale = initialScale * 1.05f;
        //play sfx
    }
    private void OnMouseExit()
    {
        if (!isSelected) { transform.localScale = initialScale; }
    }
}
