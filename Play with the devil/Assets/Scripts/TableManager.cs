using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI")]
    [SerializeField] private Button ansBtn;
    [SerializeField] private Button useBtn;
    [SerializeField] private TextMeshProUGUI numFakeCardsTMP;

    private List<Cell> cells = new List<Cell>();
    private List<Cell> selectedCells = new List<Cell>();
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
        numFakeCardsTMP.text = "Number of fake cards: " + numberOfFakeCards.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(TurnAllNormalCardsUp(0.1f));
        }
    }
    
    private IEnumerator TurnAllNormalCardsUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (Cell cell in cells)
        {
            cell.card.TurnCard(true);
            yield return new WaitForSeconds(delay);
        }
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
        CheckAnswerButtonCondition();
        CheckUseCardButtonCondition();
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
        CheckAnswerButtonCondition();
        CheckUseCardButtonCondition();
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
        CheckUseCardButtonCondition();
        CheckAnswerButtonCondition();
    }

    public void DeselectFunctionalCell(Card card)
    {
        Cell deselectedCell = null;
        for (int i = 0; i < functionalCells.Count; i++)
        {
            if (card == functionalCells[i].card)
            {
                deselectedCell = functionalCells[i];
                break;
            }
        }
        if (selectedFunctionalCell != deselectedCell) return;
        selectedFunctionalCell.card.Deselect();
        selectedFunctionalCell = null;
        CheckUseCardButtonCondition();
        CheckAnswerButtonCondition();
    }

    private void CheckAnswerButtonCondition()
    {
        if (selectedCells.Count == numberOfFakeCards && selectedFunctionalCell == null)
        {
            ansBtn.interactable = true;
        }
        else
        {
            ansBtn.interactable = false;
        }
    }

    private void CheckUseCardButtonCondition()
    {
        if (selectedCells.Count == 1 && selectedFunctionalCell != null)
        {
            useBtn.interactable = true;
        }
        else
        {
            useBtn.interactable = false;
        }
    }
}
