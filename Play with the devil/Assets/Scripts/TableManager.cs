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
    [SerializeField] private Clock clock;
    

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
        else if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(DealNormalCards());
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(SwapMultiplePairOfNormalCards(11));
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
    private IEnumerator DealNormalCards()
    {
        //Start from center
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.transform.position = transform.position;
            }
        }
        yield return new WaitForSeconds(2);
        //Shuffle
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].containCard)
            {
                cells[i].card.Rotate(2 * (float)(i + 1) / cells.Count);
            }
        }
        yield return new WaitForSeconds(3f);
        //Move to cell
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.FlyFromTo(transform.position, cell.position, 2f);
            }
        }
        yield return new WaitForSeconds(2.5f);
        //Turn up to show for player
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.TurnCard(true);
            }
        }
        //Start clock
        StartCoroutine(clock.TriggerClock(10f));
        yield return new WaitForSeconds(11f);
        //End clock
        //Shuffle cell's position
        //Turn down to hide from player
        ShuffleNormalCards();
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.TurnCard(false);
            }
        }
        yield return new WaitForSeconds(1f);
        //Move to center
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.FlyFromTo(cell.position, transform.position, 2f);
            }
        }
        yield return new WaitForSeconds(2.5f);
        //Shuffle
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].containCard)
            {
                cells[i].card.Rotate(2 * (float)(i + 1) / cells.Count);
            }
        }
        yield return new WaitForSeconds(3f);
        //Shuffle cell's position
        ShuffleNormalCards();
        //Move to new cell
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.FlyFromTo(transform.position, cell.position, 2f);
            }
        }
    }
    private void ShuffleNormalCards()
    {
        List<Card> cards = new List<Card>();
        foreach (Cell cell in cells)
        {
            if (cell.containCard) cards.Add(cell.card);
        }
        foreach (Cell cell in cells)
        {
            int randomIndex = Random.Range(0, cards.Count);
            cell.card = cards[randomIndex];
            cards.RemoveAt(randomIndex);
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
    
    private IEnumerator SwapMultiplePairOfNormalCards(int times)
    {
        for (int i = 1; i <= times; i++)
        {
            StartCoroutine(SwapTwoRandomNormalCards());
            yield return new WaitForSeconds(3f);
        }
    }
    private IEnumerator SwapTwoRandomNormalCards()
    {
        int index1 = Random.Range(0, cells.Count);
        int index2 = Random.Range(0, cells.Count);
        while (index1 == index2) index2 = Random.Range(0, cells.Count);
        cells[index1].card.Rotate(0.2f);
        cells[index2].card.Rotate(0.2f);
        Card tempCard = cells[index1].card;
        cells[index1].card = cells[index2].card;
        cells[index2].card = tempCard;
        yield return new WaitForSeconds(0.5f);
        cells[index1].card.FlyFromTo(cells[index1].card.transform.position, cells[index1].position, 2);
        cells[index2].card.FlyFromTo(cells[index2].card.transform.position, cells[index2].position, 2);
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
