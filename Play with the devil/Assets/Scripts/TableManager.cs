using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private int row = 1;
    [SerializeField] private int column = 1;
    [SerializeField] private Vector2 gap = new Vector2(0.2f, 0.2f);
    [SerializeField] private Vector2 cardSize = Vector2.one;

    private List<Vector2> cells = new List<Vector2>();

    private void Awake()
    {
        Vector2 cardDistance = new Vector2(cardSize.x + gap.x, cardSize.y + gap.y);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                cells.Add(new Vector2(cardDistance.x * (j - (column - 1) / 2f), cardDistance.y * (i - (row - 1) / 2f)));
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (cells == null) return;
        Gizmos.color = Color.green;
        foreach (Vector2 cell in cells)
        {
            Gizmos.DrawWireCube(cell, cardSize);
        }
    }
}
