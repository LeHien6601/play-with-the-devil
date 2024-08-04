using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FunctionalCard : Card
{
    private string content;
    private FunctionType type;
    private string value;
    private bool isNumber;
    private int numberValue;
    public void UpdateData(FunctionType type, string value, Color32 color)
    {
        if (type == FunctionType.IsFake)
        {
            this.content = value;
        }
        else
        {
            this.content = type.ToString() + " " + value;
        }
        this.type = type;
        this.value = value;
        this.isNumber = int.TryParse(value, out numberValue) || (value.CompareTo("Number") == 0);
        this.color = color;
        this.contentTMP.faceColor = color;
        this.contentTMP.text = content;
        this.contentTMP.outlineWidth = .2f;
        this.contentTMP.outlineColor = new Color32((byte)(color.r / 5), (byte)(color.g / 5), (byte)(color.b / 5), 255);
    }

    public enum FunctionType
    {
        Before, After, Is, IsFake
    }

    public bool ActiveFunction(NormalCard card)
    {
        //For fake cards
        if (card.IsFake() && type != FunctionType.IsFake) { return card.GetContent().CompareTo("True") == 0 ? true : false; }
        //For number and letter cards
        //      (N + is/before/after L) || (L + is/before/after N) -> false 
        if (card.IsNumber() != isNumber && content.Length == 1) return false;
        switch (type)
        {
            case FunctionType.Before:
                //  (n + before N) -> true
                if (isNumber)
                {
                    return (card.GetNumber() < numberValue);
                }
                //  (l + before L) -> true
                return (value.CompareTo(card.GetContent()) > 0);
            case FunctionType.After:
                //  (N + after n) -> true 
                if (isNumber)
                {
                    return (card.GetNumber() > numberValue);
                }
                //  (L + after l) -> true
                return (value.CompareTo(card.GetContent()) < 0);
            case FunctionType.Is:
                if (isNumber)
                {
                    //      (N + is N || N + is number) -> true
                    return card.IsNumber() && (card.GetNumber() == numberValue || value.CompareTo("Number") == 0);
                }
                if (value.Length == 1)
                {
                    //      (L + is L) -> true
                    return (value.CompareTo(card.GetContent()) == 0);
                }
                if (value.CompareTo("Letter") == 0)
                {
                    //      (L + is letter) -> true
                    return !card.IsNumber();
                }
                return (card.GetColor().r == color.r && card.GetColor().g == color.g && card.GetColor().b == color.b);
            case FunctionType.IsFake:
                return card.IsFake();

            default: return true;
        }
    }

    private void OnMouseDown()
    {
        if (!isSelectable) return;
        this.isSelected = !isSelected;
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.CardClick);
        if (isSelected)
        {
            GetComponentInParent<TableManager>().SelectFunctionalCell(this);
            border.SetActive(true);
        }
        else
        {
            GetComponentInParent<TableManager>().DeselectFunctionalCell(this);
            border.SetActive(false);
        }
    }

    public void CreateRandomFunctionalCard(int limitContent, int limitColor, Dictionary<string, float> rates)
    {
        float r = Random.Range(0f, rates["isNumberLetter"] + rates["isSpecificNumberLetter"] + rates["isBeforeAfter"] + rates["isFake"] + rates["isColor"]);
        if (r < rates["isFake"])
        {
            CreateFunctionalFakeCard();
            return;
        }
        if (r < rates["isFake"] + rates["isNumberLetter"] + rates["isSpecificNumberLetter"] + rates["isBeforeAfter"])
        {
            int ran = Random.Range(0, 2);
            if (ran == 0)
                CreateFunctionalLetterCard(limitContent, rates);
            else
                CreateFunctionalNumberCard(limitContent, rates);
            return;
        }
        CreateFunctionalColorCard(limitColor);
    }
    public void CreateFunctionalLetterCard(int limitContent, Dictionary<string, float> rates)
    {
        float r = Random.Range(0f, rates["isNumberLetter"] + rates["isSpecificNumberLetter"]);
        if (r < rates["isNumberLetter"])
        {
            UpdateData(FunctionType.Is, "Letter", new Color32(153, 0, 51, 255));
            return;
        }
        UpdateData(GetRandomFunctionType(rates), GetRandomLetter(limitContent), new Color32(153, 0, 51, 255));
    }
    public void CreateFunctionalNumberCard(int limitContent, Dictionary<string, float> rates)
    {
        float r = Random.Range(0f, rates["isNumberLetter"] + rates["isSpecificNumberLetter"]);
        if (r < rates["isNumberLetter"])
        {
            UpdateData(FunctionType.Is, "Number", new Color32(102, 51, 0, 255));
            return;
        }
        UpdateData(GetRandomFunctionType(rates), GetRandomNumber(limitContent), new Color32(102, 51, 0, 255));
    }
    public void CreateFunctionalColorCard(int limitColor)
    {
        Color color = GetRandomColor(limitColor);
        UpdateData(FunctionType.Is, GetColorName(color), color);
    }
    public void CreateFunctionalFakeCard()
    {
        UpdateData(FunctionType.IsFake, "Is fake", new Color32(128, 128, 128, 255));
    }

    public FunctionType GetRandomFunctionType(Dictionary<string, float> rates)
    {
        float r = Random.Range(0f, rates["isSpecificNumberLetter"] + rates["isBeforeAfter"]);
        if (r < rates["isBeforeAfter"] / 2f) return FunctionType.After;
        if (r < rates["isBeforeAfter"]) return FunctionType.Before;
        return FunctionType.Is;
    }
}
