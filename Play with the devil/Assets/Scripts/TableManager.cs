using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [Header("Normal Cards")]
    [SerializeField] private Vector2 gap = new Vector2(0.2f, 0.2f);
    [SerializeField] private Vector2 cardSize = Vector2.one;
    [SerializeField] private NormalCard cardPref;

    [Header("Functional Cards")]
    [SerializeField] private Vector2 startPosition;
    [SerializeField] float gapFunctionalCard;
    [SerializeField] private FuntionalCard functionalCardPref;

    [Header("Game's information")]
    [SerializeField] private int row = 1;
    [SerializeField] private int column = 1;
    [SerializeField] private int numberOfFakeCards;
    [SerializeField] private int numberOfFunctionalCards;

    private List<Cell> cells = new List<Cell>();
    [SerializeField]public List<Cell> selectedCells = new List<Cell>();
    private List<Cell> functionalCells = new List<Cell>();
    private Cell selectedFunctionalCell = null;

    public class Cell {
        public bool containCard;
        public Card card;
        public Vector2 position;
        public Cell(bool containCard, Card card, Vector2 position)
        {
            this.containCard = containCard;
            this.card = card;
            this.position = position;
        }
    };

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
                Vector2 position = new Vector2(cardDistance.x * (j - (column - 1) / 2f), cardDistance.y * (i - (row - 1) / 2f));
                Card card = Instantiate(cardPref, position, Quaternion.identity, transform);
                cells.Add(new Cell(true, card, position));
            }
        }
    }

    private void SetUpFunctionalCards()
    {
        float cardDistance = gapFunctionalCard + cardSize.x;
        for (int i = 0; i < 5; i++)
        {
            Vector2 position = new Vector2(startPosition.x + i * cardDistance, startPosition.y);
            Card card = Instantiate(functionalCardPref, position, Quaternion.identity, transform);
            functionalCells.Add(new Cell(true, card, position));
        }
    }

    private void OnDrawGizmos()
    {
        if (cells == null) return;
        Gizmos.color = Color.green;
        foreach (Cell cell in cells)
        {
            Gizmos.DrawWireCube(cell.position, cardSize);
        }
        if (functionalCells == null) return;
        Gizmos.color = Color.blue;
        foreach (Cell cell in functionalCells)
        {
            Gizmos.DrawWireCube(cell.position, cardSize);
        }
    }

    public void SelectCell(Card card)
    {
        Cell selectedCell = null;
        for (int i = 0; i < cells.Count; i++)
        {
            if (card == cells[i].card)
            {
                selectedCell = cells[i];
                break;
            }
        }
        if (selectedCells.Contains(selectedCell)) return;
        if (selectedCells.Count == numberOfFakeCards)
        {
            selectedCells[0].card.Deselect();
            selectedCells.RemoveAt(0);
        }
        selectedCells.Add(selectedCell);
    }

    public void DeselectCell(Card card)
    {
        Cell deselectedCell = null;
        for (int i = 0; i < cells.Count; i++)
        {
            if (card == cells[i].card)
            {
                deselectedCell = cells[i];
                break;
            }
        }
        if (!selectedCells.Contains(deselectedCell)) return;
        if (selectedCells.Count == 0) return;
        deselectedCell.card.Deselect();
        selectedCells.Remove(deselectedCell);
    }

    public void SelectFunctionalCell(Card card)
    {
        Cell selectedCell = null;
        for (int i = 0; i < functionalCells.Count; i++)
        {
            if (card == functionalCells[i].card)
            {
                selectedCell = functionalCells[i];
                break;
            }
        }
        if (selectedFunctionalCell == selectedCell) return;
        if (selectedFunctionalCell == null) selectedFunctionalCell = selectedCell;
        else
        {
            selectedFunctionalCell.card.Deselect();
            selectedFunctionalCell = selectedCell;
        }
    }

    public void DeselectFunctionalCell(Card card)
    {
        Cell deselectedCell = null;
        for (int i = 0; i < functionalCells.Count; i++)
        {
            if (card == cells[i].card)
            {
                deselectedCell = cells[i];
                break;
            }
        }
        if (selectedFunctionalCell != deselectedCell) return;
        selectedFunctionalCell.card.Deselect();
        selectedFunctionalCell = null;
    }
}
