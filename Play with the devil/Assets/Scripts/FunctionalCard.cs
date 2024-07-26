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
    public void UpdateData(FunctionType type, string value, Color color)
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
        this.contentTMP.color = color;
        this.contentTMP.text = content;
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
                    return (card.GetNumber() == numberValue) || (value.CompareTo("Number") == 0);
                }
                if (value.Length == 1)
                {
                    //      (L + is L) -> true
                    return (value.CompareTo(card.GetContent()) == 0);
                }
                if (value.CompareTo("Letter") == 0)
                {
                    //      (L + is letter) -> true
                    return true;
                }
                return card.GetColor() == color;
            case FunctionType.IsFake:
                return card.IsFake();

            default: return true;
        }
    }

    private void OnMouseDown()
    {
        if (!isSelectable) return;
        this.isSelected = !isSelected;
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

    public void CreateRandomFunctionalCard(int limitContent, int limitColor)
    {
        if (limitColor > 1)
        {
            int index = Random.Range(0, 10);
            if (index == 0)
            {
                CreateFunctionalFakeCard();
            }
            else if (index >= 1 && index <= 3)
            {
                CreateFunctionalLetterCard(limitContent);
            }
            else if (index >= 4 && index <= 6)
            {
                CreateFunctionalNumberCard(limitContent);
            }
            else
            {
                CreateFunctionalColorCard(limitColor);
            }
        }
        else
        {
            int index = Random.Range(0, 9);
            if (index == 0)
            {
                CreateFunctionalFakeCard();
            }
            else if (index >= 1 && index <= 4)
            {
                CreateFunctionalLetterCard(limitContent);
            }
            else
            {
                CreateFunctionalNumberCard(limitContent);
            }
        }
    }
    public void CreateFunctionalLetterCard(int limitContent)
    {
        float index = Random.Range(0f, 5f);
        if (index < 4f)
        {
            UpdateData(FunctionType.Is, "Letter", new Color(0.6f, 0f, 0.2f));
        }
        else UpdateData(GetRandomFunctionType(), GetRandomLetter(limitContent), new Color(0.6f, 0f, 0.2f));
    }
    public void CreateFunctionalNumberCard(int limitContent)
    {
        float index = Random.Range(0f, 5f);
        if (index < 4f)
        {
            UpdateData(FunctionType.Is, "Number", new Color(0.4f, 0.2f, 0f));
        }
        else UpdateData(GetRandomFunctionType(), GetRandomNumber(limitContent), new Color(0.4f, 0.2f, 0f));
    }
    public void CreateFunctionalColorCard(int limitColor)
    {
        Color color = GetRandomColor(limitColor);
        UpdateData(FunctionType.Is, GetColorName(color), color);
    }
    public void CreateFunctionalFakeCard()
    {
        UpdateData(FunctionType.IsFake, "Is fake", Color.gray);
    }

    public FunctionType GetRandomFunctionType()
    {
        int index = Random.Range(0, 3);
        switch (index)
        {
            case 0:
                return FunctionType.Before;
            case 1:
                return FunctionType.After;
            case 2:
                return FunctionType.Is;
            default:
                return FunctionType.Is;
        }
    }
}
