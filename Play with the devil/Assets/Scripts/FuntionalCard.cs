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
        this.isNumber = int.TryParse(value, out numberValue);
        this.color = color;
    }

    public enum FunctionType
    {
        Before, After, Is, IsFake
    }

    public bool ActiveFunction(NormalCard card)
    {
        if (card.IsNumber() != isNumber) return false;
        switch (type)
        {
            case FunctionType.Before:
                if (isNumber)
                {
                    return (card.GetNumber() < numberValue);
                }
                return (value.CompareTo(card.GetContent()) > 0);
            case FunctionType.After:
                if (isNumber)
                {
                    return (card.GetNumber() > numberValue);
                }
                return (value.CompareTo(card.GetContent()) < 0);
            case FunctionType.Is:
                if (isNumber)
                {
                    return (card.GetNumber() == numberValue);
                }
                else if (value.Length == 1)
                {
                    return (value.CompareTo(card.GetContent()) == 0);
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

    public FunctionalCard CreateRandomFunctionalCard(int limitContent, int limitColor)
    {
        if (limitColor > 1)
        {
            int index = Random.Range(0, 10);
            if (index == 0)
            {
                return CreateFunctionalFakeCard();
            }
            else if (index >= 1 && index <= 3)
            {
                return CreateFunctionalLetterCard(limitContent);
            }
            else if (index >= 4 && index <= 6)
            {
                return CreateFunctionalNumberCard(limitContent);
            }
            else
            {
                return CreateFunctionalColorCard(limitColor);
            }
        }
        else
        {
            int index = Random.Range(0, 9);
            if (index == 0)
            {
                return CreateFunctionalFakeCard();
            }
            else if (index >= 1 && index <= 4)
            {
                return CreateFunctionalLetterCard(limitContent);
            }
            else
            {
                return CreateFunctionalNumberCard(limitContent);
            }
        }
    }
    public FunctionalCard CreateFunctionalLetterCard(int limitContent)
    {
        FunctionalCard card = new FunctionalCard();
        card.UpdateData(GetRandomFunctionType(), GetRandomLetter(limitContent), Color.magenta);
        return card;
    }
    public FunctionalCard CreateFunctionalNumberCard(int limitContent)
    {
        FunctionalCard card = new FunctionalCard();
        card.UpdateData(GetRandomFunctionType(), GetRandomNumber(limitContent), Color.magenta);
        return card;
    }
    public FunctionalCard CreateFunctionalColorCard(int limitColor)
    {
        FunctionalCard card = new FunctionalCard();
        Color color = GetRandomColor(limitColor);
        card.UpdateData(FunctionType.Is, GetColorName(color), color);
        return card;
    }
    public FunctionalCard CreateFunctionalFakeCard()
    {
        FunctionalCard card = new FunctionalCard();
        card.UpdateData(FunctionType.IsFake, "Is fake", Color.gray);
        return card;
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
