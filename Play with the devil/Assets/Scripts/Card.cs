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
    private Vector2 from = Vector2.one, to = Vector2.one;
    private float timer = 0f, interval = 0f;
    private bool isFlying = false;
    private float rotateTimer = 0f, rotateInterval = 0f;
    private bool isRotating = false;
    private void Awake()
    {
        spriteRenderer.sprite = (isFaceDown) ? backSprite : frontSprite;
        canvas.worldCamera = Camera.main;
        initialScale = gameObject.transform.localScale;
    }
    private void FixedUpdate()
    {
        if (isFlying)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= interval)
            {
                transform.position = to;
                isFlying = false;
                timer = 0;
                interval = 0;
            }
            else
            {
                transform.position = Vector2.Lerp(from, to, timer/interval);
            }
        }
        if (isRotating)
        {
            rotateTimer += Time.fixedDeltaTime;
            if (rotateTimer >= rotateInterval)
            {
                rotateInterval = 0;
                rotateTimer = 0;
                isRotating = false;
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3 (0, 0, 360 * rotateTimer / rotateInterval);
            }
        }
    }
    public void FlyFromTo(Vector2 start, Vector2 end, float time)
    {
        isFlying = true;
        transform.position = start;
        from = start;
        to = end;
        interval = time;
        timer = 0f;
    }
    public void Rotate(float time)
    {
        isRotating = true;
        transform.eulerAngles = Vector3.zero;
        rotateInterval = time;
        rotateTimer = 0f;
    }
    public void TurnCard(bool up)
    {
        isFaceDown = !up;
        animator.SetTrigger("Turn");
        spriteRenderer.sprite = up ? frontSprite : backSprite;
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
