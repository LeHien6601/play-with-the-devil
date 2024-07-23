using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] protected TextMeshProUGUI contentTMP;
    public bool isSelectable = false;
    protected bool isSelected = false;
    protected Color color = Color.white;
    private Vector3 initialScale;
    private Vector2 from = Vector2.one, to = Vector2.one;
    private float timer = 0f, interval = 0f;
    private bool isFlying = false;
    private float rotateTimer = 0f, rotateInterval = 0f;
    private bool isRotating = false;
    private bool isFake = false;
    
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

    public bool IsFake() { return isFake; }
    public void SetFake(bool isFake) { this.isFake = isFake; } 
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
    public void CardFace()
    {
        isFaceDown = false;
        animator.SetTrigger("Fade");
    }
    public void Deselect()
    {
        if (!isSelectable) return;
        isSelected = false;
        border.SetActive(false);
        transform.localScale = initialScale;
    }
    private void OnMouseEnter()
    {
        if (!isSelectable) return;
        transform.localScale = initialScale * 1.05f;
        //play sfx
    }
    private void OnMouseExit()
    {
        if (!isSelectable) return;
        if (!isSelected) { transform.localScale = initialScale; }
    }
    public string GetRandomLetter(int limit)
    {
        return cardLetters[Random.Range(0, limit) % cardLetters.Length];
    }
    public string[] cardLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};

    public string GetRandomNumber(int limit)
    {
        return cardNumbers[Random.Range(0, limit) % cardNumbers.Length];
    }
    public string[] cardNumbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    public Color GetRandomColor(int limit)
    {
        return cardColors[Random.Range(0, limit) % cardColors.Length];
    }
    public Color[] cardColors = { Color.white, Color.green, Color.blue, Color.yellow, Color.red, Color.black };
    public string[] cardColorNames = { "White", "Green", "Blue", "Yellow", "Red", "Black" };
    public Color GetColorByName(string name)
    {
        for (int i = 0; i < cardColorNames.Length; i++)
        {
            if (cardColorNames[i].Equals(name)) return cardColors[i];
        }
        return Color.white;
    }
    public string GetColorName(Color color)
    {
        for (int i = 0; i < cardColors.Length; i++)
        {
            if (color == cardColors[i]) return cardColorNames[i];   
        }
        return "White";
    }
    public Color GetColor() { return color; }
}
