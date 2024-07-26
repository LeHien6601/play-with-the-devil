using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalCard : Card
{
    private string content;
    private bool isNumber;
    private int numberContent;
    public void UpdateData(string content, bool isFake, Color color)
    {
        this.content = content;
        SetFake(isFake);
        this.isNumber = int.TryParse(content, out numberContent);
        this.color = color;
        this.contentTMP.text = content;
        this.contentTMP.faceColor = color;
        if (isFake)
        {
            this.contentTMP.fontSize = 0.55f;
        }
        Debug.Log(GetColorName(this.contentTMP.faceColor));
    }
    public string GetContent()
    {
        return content;
    }
    public int GetNumber() { return numberContent; }
    public bool IsNumber() { return isNumber; }


    private void OnMouseDown()
    {
        if (!isSelectable) return;
        this.isSelected = !isSelected;
        if (isSelected)
        {
            GetComponentInParent<TableManager>().SelectCell(this);
            border.SetActive(true);
        }
        else
        {
            GetComponentInParent<TableManager>().DeselectCell(this);
            border.SetActive(false);
        }
    }
    public void CreateRandomFakeCard()
    {
        int index = Random.Range(0, 2);
        UpdateData((index == 0) ? "False" : "True", true, Color.gray);
    }
    public void CreateRandomNormalCard(int limitContent, int limitColor)
    {
        int index = Random.Range(0, 2);
        UpdateData((index == 0) ? GetRandomLetter(limitContent) : GetRandomNumber(limitContent), false, GetRandomColor(limitColor));
    }
    public void InverseFakeCard()
    {
        if (!IsFake()) return;
        content = (content.CompareTo("True") == 0) ? "False" : "True";
        contentTMP.text = content;
    }
}
