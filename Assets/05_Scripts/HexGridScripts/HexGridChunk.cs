using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGridChunk : MonoBehaviour
{
    private HexCell[] cells;

    private HexMesh hexMesh;

    private Canvas gridCanvas;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        
        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }


    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.transform.SetParent(transform, false);
       // cell.SetParent(gridCanvas.transform, false);
    }
}
