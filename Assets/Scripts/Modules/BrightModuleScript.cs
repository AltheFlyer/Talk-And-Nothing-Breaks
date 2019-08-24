using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightModuleScript : Module
{
    [SerializeField]
    GameObject commandPrefab;

    GameObject display;
    GameObject commandOutline;
    GameObject commands;
    GameObject map;
    GameObject[,] mapGrid;
    GameObject[,] commandGrid;

    int mapDimensions = 5;
    float mapWidth = 0.45f;
    float mapSquareHeight = 0.19f;
    [Range(0f,1f)]
    public float mapSquarePercentSize = 0.4f;
    Vector3 mapPos = new Vector3(0.2f, 0.5f, -0.2f);

    public Material gridMat;
    public Color gridColor;
    public Color gridBGColor;
    public Color litColor;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        map = transform.Find("Map").gameObject;
        display = transform.Find("Display").gameObject;
        commandOutline = display.transform.Find("Outline").gameObject;
        commands = display.transform.Find("Commands").gameObject;

        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        mapGrid = new GameObject[mapDimensions, mapDimensions];
        float squareWidth = mapWidth / mapDimensions;
        Vector3 botLeftPos = new Vector3(mapPos.x - 2 * squareWidth, mapPos.y, mapPos.z - 2 * squareWidth);
        GameObject lines = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lines.transform.SetParent(map.transform);
        lines.transform.localPosition = mapPos;
        lines.transform.localScale = new Vector3(mapWidth, mapSquareHeight - 0.01f, mapWidth);
        lines.GetComponent<Renderer>().material = gridMat;
        lines.GetComponent<Renderer>().material.SetColor("_Color", gridBGColor);
        lines.GetComponent<Renderer>().material.SetColor("_EmissionColor", gridBGColor);
        lines.name = "GridLines";
        for (int i = 0; i < mapDimensions; i++) {
            for (int j = 0; j < mapDimensions; j++) {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.SetParent(map.transform);
                go.transform.localPosition = new Vector3(botLeftPos.x + squareWidth * i, botLeftPos.y, botLeftPos.z + squareWidth * j);
                go.transform.localScale = new Vector3(squareWidth * mapSquarePercentSize, mapSquareHeight, squareWidth * mapSquarePercentSize);
                go.GetComponent<Renderer>().material = gridMat;
                go.GetComponent<Renderer>().material.SetColor("_Color", gridColor);
                go.GetComponent<Renderer>().material.SetColor("_EmissionColor", gridColor);
                go.name = "Square" + (5 * i + j + 1);
                mapGrid[i, j] = go;
            }
        }



    }
}
