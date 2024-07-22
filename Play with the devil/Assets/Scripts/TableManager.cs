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
    [SerializeField] private FunctionalCard functionalCardPref;

    [Header("Game's information")]
    [SerializeField] private LevelData levelData;
    private int row = 1;
    private int column = 1;
    private int numberOfFakeCards;
    private int numberOfFunctionalCards;
    private int limitContent;
    private int limitColor;
    private float showCardTime;
    private int numberOfSwapedPairs;
    private int numberOfSouls;

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

    private void Start()
    {
        UpdateLevelData();
        SoulController.instance.GenerateSouls(numberOfSouls);
        SetUpNormalCards();
        SetUpFunctionalCards();
        numFakeCardsTMP.text = "Number of fake cards: " + numberOfFakeCards.ToString();
    }
    

    private void Update()
    {
        
    }
    //      Update initial level's data
    private void UpdateLevelData()
    {
        row = levelData.row;
        column = levelData.column;
        numberOfFakeCards = levelData.numberOfFakeCards;
        numberOfFunctionalCards = levelData.numberOfFunctionalCards;
        limitColor = levelData.limitColor;
        limitContent = levelData.limitContent;
        showCardTime = levelData.showCardTime;
        numberOfSwapedPairs = levelData.numberOfSwapedPairs;
        numberOfSouls = levelData.numberOfSouls;
    }

    //      Turn up/down cards
    private IEnumerator TurnAllNormalCards(float delay, bool up)
    {
        yield return new WaitForSeconds(delay);
        foreach (Cell cell in cells)
        {
            cell.card.TurnCard(up);
            yield return new WaitForSeconds(delay);
        }
    }
    private IEnumerator TurnAllFunctionalCards(float delay, bool up)
    {
        yield return new WaitForSeconds(delay);
        foreach (Cell cell in functionalCells)
        {
            cell.card.TurnCard(up);
            yield return new WaitForSeconds(delay);
        }
    }


    //      Set up normal cards

    private void SetUpNormalCards()
    {
        Vector2 cardDistance = new Vector2(cardSize.x + gap.x, cardSize.y + gap.y);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector2 position = new Vector2(cardDistance.x * (j - (column - 1) / 2f), cardDistance.y * (i - (row - 1) / 2f));
                NormalCard card = Instantiate(cardPref, transform.position, Quaternion.identity, transform);
                if (cells.Count < numberOfFakeCards)
                {
                    card.CreateRandomFakeCard();
                }
                else
                {
                    card.CreateRandomNormalCard(limitContent, limitColor);
                }
                cells.Add(new Cell(true, card, position));
            }
        }
        StartCoroutine(DealNormalCards());
    }
    private IEnumerator DealNormalCards()
    {
        //Turn off selectable ability for normal cards
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.isSelectable = false;
            }
        }
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
        StartCoroutine(TurnAllNormalCards(0.1f, true));
        //Start clock
        StartCoroutine(clock.TriggerClock(showCardTime));
        yield return new WaitForSeconds(showCardTime + 1);
        //Turn down to hide from player
        StartCoroutine(TurnAllNormalCards(0.1f, false));
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
        //Move to new cell
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.FlyFromTo(transform.position, cell.position, 2f);
            }
        }
        //Shuffle cell's position
        ShuffleNormalCards();
        yield return new WaitForSeconds(2f);
        //Turn on selectable ability for normal cards
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.isSelectable = true;
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
    
    private IEnumerator SwapMultiplePairOfNormalCards(int times)
    {
        //Turn off selectable ability for normal cards
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.isSelectable = false;
            }
        }
        for (int i = 1; i <= times; i++)
        {
            StartCoroutine(SwapTwoRandomNormalCards());
            yield return new WaitForSeconds(3f);
        }
        //Turn on selectable ability for normal cards
        foreach (Cell cell in cells)
        {
            if (cell.containCard)
            {
                cell.card.isSelectable = true;
            }
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



    //      Set up functional cards

    private void SetUpFunctionalCards()
    {
        float cardDistance = gapFunctionalCard + cardSize.x;
        for (int i = 0; i < numberOfFunctionalCards; i++)
        {
            Vector2 position = new Vector2(startPosition.x + i * cardDistance, startPosition.y);
            FunctionalCard card = Instantiate(functionalCardPref, startPosition, Quaternion.identity, transform);
            card.CreateRandomFunctionalCard(limitContent, limitColor);
            functionalCells.Add(new Cell(true, card, position));
        }
        StartCoroutine(DealFunctionalCards());
    }
    private IEnumerator DealFunctionalCards()
    {
        //Turn off selectable ability for functional cards
        foreach (Cell cell in functionalCells)
        {
            if (cell.containCard)
            {
                cell.card.isSelectable = false;
            }
        }
        //Wait for normal cards
        yield return new WaitForSeconds(15f + showCardTime);
        //Start functional cards's animation
        foreach (Cell cell in functionalCells)
        {
            if (cell.containCard)
            {
                cell.card.Rotate(Random.Range(0f, 2f));
            }
        }
        yield return new WaitForSeconds(3f);
        //Move to cell's position
        foreach (Cell cell in functionalCells)
        {
            if (cell.containCard)
            {
                cell.card.FlyFromTo(startPosition, cell.position, 1f);
            }
        }
        //Turn up functional cards to show for player
        StartCoroutine(TurnAllFunctionalCards(0.1f, true));
        yield return new WaitForSeconds(2f);
        //Turn on selectable ability for functional cards
        foreach (Cell cell in functionalCells)
        {
            if (cell.containCard)
            {
                cell.card.isSelectable = true;
            }
        }
    }

    //      Select and deselect card

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


    //      UI's conditions

    private void CheckAnswerButtonCondition()
    {
        if (selectedCells.Count == numberOfFakeCards && selectedFunctionalCell == null)
        {
            ansBtn.interactable = true;
            ansBtn.GetComponent<UIDescriptionHandler>().SetActive(false);
        }
        else
        {
            ansBtn.interactable = false;
            ansBtn.GetComponent<UIDescriptionHandler>().SetActive(true);
        }
    }

    private void CheckUseCardButtonCondition()
    {
        if (selectedCells.Count == 1 && selectedFunctionalCell != null)
        {
            useBtn.interactable = true;
            useBtn.GetComponent<UIDescriptionHandler>().SetActive(false);
        }
        else
        {
            useBtn.interactable = false;
            useBtn.GetComponent<UIDescriptionHandler>().SetActive(true);
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
}
