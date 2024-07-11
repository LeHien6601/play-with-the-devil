using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCard : Card
{
    private string content;
    private bool isFake = false;
    private bool isNumber;
    private int numberContent;
    public void UpdateData(string content, bool isFake)
    {
        this.content = content;
        this.isFake = isFake;
        this.isNumber = int.TryParse(content, out numberContent);
    }
    public string GetContent()
    {
        return content;
    }
    public bool IsFake() { return isFake; }
    public int GetNumber() { return numberContent; }
    public bool IsNumber() { return isNumber; }
}
