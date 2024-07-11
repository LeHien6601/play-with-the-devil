using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private int row = 1;
    [SerializeField] private int column = 1;
    [SerializeField] private Vector2 gap = new Vector2(0.2f, 0.2f);
    [SerializeField] private Vector2 cardSize = Vector2.one;
    [SerializeField] private NormalCard cardPref;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] float gapFunctionalCard;
    [SerializeField] private FuntionalCard functionalCardPref;

    private List<Vector2> cells = new List<Vector2>();
    private List<NormalCard> cards = new List<NormalCard>();

    private List<Vector2> funtionalCells = new List<Vector2>();
    private List<FuntionalCard> funtionalCards = new List<FuntionalCard>();

    private void Awake()
    {
        SetUpNormalCards();
        SetUpFunctionalCards();
    }
    private void Start()
    {
        
    }

    private void SetUpNormalCards()
    {
        Vector2 cardDistance = new Vector2(cardSize.x + gap.x, cardSize.y + gap.y);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                cells.Add(new Vector2(cardDistance.x * (j - (column - 1) / 2f), cardDistance.y * (i - (row - 1) / 2f)));
            }
        }
        if (cells != null)
        {
            foreach (var cell in cells)
            {
                cards.Add(Instantiate(cardPref, cell, Quaternion.identity, transform));
            }
        }
    }

    private void SetUpFunctionalCards()
    {
        float cardDistance = gapFunctionalCard + cardSize.x;
        for (int i = 0; i < 5; i++)
        {
            funtionalCells.Add(new Vector2(startPosition.x + i * cardDistance, startPosition.y));
        }
        foreach (var cell in funtionalCells)
        {
            funtionalCards.Add(Instantiate(functionalCardPref, cell, Quaternion.identity, transform));
        }
    }

    private void OnDrawGizmos()
    {
        if (cells == null) return;
        Gizmos.color = Color.green;
        foreach (Vector2 cell in cells)
        {
            Gizmos.DrawWireCube(cell, cardSize);
        }
        if (funtionalCells == null) return;
        Gizmos.color = Color.blue;
        foreach (Vector2 cell in funtionalCells)
        {
            Gizmos.DrawWireCube(cell, cardSize);
        }
    }
}
