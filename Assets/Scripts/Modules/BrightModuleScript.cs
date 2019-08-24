using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightModuleScript : Module
{
    [SerializeField]
    GameObject commandPrefab;

    GameObject submit;
    GameObject commandOutline;
    GameObject commands;
    GameObject map;
    GameObject[] commandButtons;
    GameObject[,] mapGrid;
    GameObject[,] commandGrid;

    int mapDimensions = 5;
    float mapWidth = 0.45f;
    float mapSquareHeight = 0.19f;
    [Range(0f,1f)]
    public float mapSquarePercentSize = 0.4f;
    Vector3 mapPos = new Vector3(0.2f, 0.5f, -0.2f);

    int delIndex;
    float commandDist = 0.2f;
    Vector3 commandTopLeftPos = new Vector3(-0.6f, 0, 0.2f);
    Vector2 currentCommandGrid = new Vector2(0, 0);

    public Material gridMat;
    public Color gridColor;
    public Color gridBGColor;
    public Color litColor;
    Color[] commandColors = { Color.blue, Color.yellow, Color.green, Color.red, Color.cyan, Color.magenta };
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        commandButtons = new GameObject[6];
        mapGrid = new GameObject[mapDimensions, mapDimensions];
        commandGrid = new GameObject[7, 3];

        submit = transform.Find("Submit").gameObject;
        map = transform.Find("Map").gameObject;
        commandOutline = transform.Find("Display").Find("Outline").gameObject;
        commands = transform.Find("Display").Find("Commands").gameObject;

        for (int i = 0; i < 6; i++) {
            commandButtons[i] = transform.Find("Buttons").Find("Button" + (i + 1)).gameObject;
            commandButtons[i].GetComponent<Renderer>().material.SetColor("_Color", commandColors[i]);
        }
        commandOutline.GetComponent<Renderer>().material.SetColor("_Color", gridColor);
        commandOutline.GetComponent<Renderer>().material.SetColor("_EmissionColor", gridColor);

        for (int i = 0; i < 7; i++) {
            for (int j = 0; j < 3; j++) {
                commandGrid[i, j] = Instantiate(commandPrefab);
                commandGrid[i, j].transform.SetParent(commands.transform);
                commandGrid[i, j].transform.localPosition = new Vector3(commandTopLeftPos.x + (commandDist * i), commandTopLeftPos.y, commandTopLeftPos.z - (commandDist * j));
                commandGrid[i, j].transform.localScale = new Vector3(0.09f, 0.09f, 2f);
                commandGrid[i, j].name = "Command" + (i * 3 + j + 1);
                commandGrid[i, j].GetComponent<MeshRenderer>().enabled = false;
            }
        }

        delIndex = 5;

        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moduleComplete) {
            if (Input.GetMouseButtonDown(0)) {
                if (IsMouseOver(submit)) {

                }


                if ((IsMouseOver(commandButtons[delIndex])) && !((currentCommandGrid.x == 0) && (currentCommandGrid.y == 0))) {

                    currentCommandGrid.x--;
                    if (currentCommandGrid.x < 0) {
                        currentCommandGrid.y--;
                        currentCommandGrid.x = 6;
                    }
                    commandGrid[(int)currentCommandGrid.x, (int)currentCommandGrid.y].GetComponent<MeshRenderer>().enabled = false;
                }

                if (!(currentCommandGrid.x == 0 && currentCommandGrid.y == 3)) {
                    for (int i = 0; i < 6; i++) {
                        if ((IsMouseOver(commandButtons[i])) && (i != delIndex)) {
                            GameObject command = commandGrid[(int)currentCommandGrid.x, (int)currentCommandGrid.y];
                            command.GetComponent<Renderer>().material.SetColor("_Color", commandColors[i]);
                            command.GetComponent<MeshRenderer>().enabled = true;
                            currentCommandGrid.x++;
                            if (currentCommandGrid.x > 6) {
                                currentCommandGrid.x = 0;
                                currentCommandGrid.y++;
                            }
                        }
                    }
                }
            }
        }
    }

    void GenerateGrid()
    {
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
