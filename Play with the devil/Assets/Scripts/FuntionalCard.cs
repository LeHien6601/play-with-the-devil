using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FuntionalCard : Card
{
    private string content;
    private FunctionType type;
    private string value;
    private bool isNumber;
    private int numberValue;
    public void UpdateData(FunctionType type, string value)
    {
        if (type == FunctionType.IsFake)
        {
            content = value;
        }
        else
        {
            content = type.ToString() + " " + value;
        }
        this.type = type;
        this.value = value;
        this.isNumber = int.TryParse(value, out numberValue);
    }

    public enum FunctionType
    {
        Before, After, Contain, IsFake
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
            case FunctionType.Contain:
                if (isNumber)
                {
                    return (card.GetNumber() == numberValue);
                }
                return (value.CompareTo(card.GetContent()) == 0);
            case FunctionType.IsFake:
                return card.IsFake();

            default: return true;
        }
    }

    private void OnMouseDown()
    {
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
}
