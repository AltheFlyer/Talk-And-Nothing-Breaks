using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightModuleScript : Module
{

    GameObject map;
    GameObject[,] grid;

    int gridDimensions = 5;
    float totalWidth = 0.45f;
    public float gridHeight = 0.2f;
    [Range(0f,1f)]
    public float squarePercentSize = 0.9f;
    float gridYPos = 0.5f;
    Vector2 gridPos = new Vector2(0.2f, -0.2f);

    public Material gridMat;
    public Color gridColor;
    public Color gridBGColor;
    public Color litColor;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        map = transform.Find("Map").gameObject;

        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridDimensions, gridDimensions];
        float squareWidth = totalWidth / gridDimensions;
        Vector2 botLeftPos = new Vector2(gridPos.x - 2 * squareWidth, gridPos.y - 2 * squareWidth);
        GameObject lines = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lines.transform.SetParent(map.transform);
        lines.transform.localPosition = new Vector3(gridPos.x, gridYPos, gridPos.y);
        lines.transform.localScale = new Vector3(totalWidth, gridHeight - 0.01f, totalWidth);
        lines.GetComponent<Renderer>().material = gridMat;
        lines.GetComponent<Renderer>().material.SetColor("_Color", gridBGColor);
        lines.GetComponent<Renderer>().material.SetColor("_EmissionColor", gridBGColor);
        lines.name = "GridLines";
        for (int i = 0; i < gridDimensions; i++) {
            for (int j = 0; j < gridDimensions; j++) {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.SetParent(map.transform);
                go.transform.localPosition = new Vector3(botLeftPos.x + squareWidth * i, gridYPos, botLeftPos.y + squareWidth * j);
                go.transform.localScale = new Vector3(squareWidth * squarePercentSize, gridHeight, squareWidth * squarePercentSize);
                go.GetComponent<Renderer>().material = gridMat;
                go.GetComponent<Renderer>().material.SetColor("_Color", gridColor);
                go.GetComponent<Renderer>().material.SetColor("_EmissionColor", gridColor);
                go.name = "Square" + (5 * i + j + 1);
            }
        }



    }
}
