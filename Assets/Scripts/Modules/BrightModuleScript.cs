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
    float squareWidth;
    float mapSquareHeight = 0.2f;
    [Range(0f,1f)]
    public float mapSquarePercentSize = 0.4f;
    Vector3 mapPos = new Vector3(0.2f, 0.5f, -0.2f);
    Vector3 mapBotLeftPos;
    Vector2 botGridPos = new Vector2(0, 0);

    int delIndex;
    float commandDist = 0.2f;
    Vector3 commandTopLeftPos = new Vector3(-0.6f, 0, 0.2f);
    Vector2 currentCommandGrid = new Vector2(6, -1);
    

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
            SetObjectColor(commandButtons[i], "_Color", commandColors[i]);
        }
        SetObjectColor(commandOutline, "_EmissionColor", gridColor);
        SetObjectColor(commandOutline, "_Color", gridColor);

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

    bool CheckAnswer()
    {
        int numLit = 0;
        for (int i = 0; i < currentCommandGrid.y + 1; i++) {
            for (int j = 0; j < 7; j++) {
                if (commandGrid[j, i].GetComponent<MeshRenderer>().enabled) {
                    Color color = commandGrid[j, i].GetComponent<Renderer>().material.GetColor("_Color");
                    if (color == commandColors[0] && botGridPos.x > 0) {
                        MoveBot(-1, 0);
                        botGridPos.x--;
                    } else if (color == commandColors[1] && botGridPos.x < 5) {
                        MoveBot(1, 0);
                        botGridPos.x++;
                    } else if (color == commandColors[2] && botGridPos.y > 0) {
                        MoveBot(0, -1);
                        botGridPos.y--;
                    } else if (color == commandColors[3] && botGridPos.y < 5) {
                        MoveBot(0, 1);
                        botGridPos.y++;
                    } else if (color == commandColors[4]) {
                        Brighten(botGridPos);
                    }
                }
            }
        }
        return true;
    }

    void Brighten (Vector2 pos)
    {
        GameObject bright = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bright.transform.SetParent(map.transform);

        bright.transform.localPosition = new Vector3(mapBotLeftPos.x + squareWidth * botGridPos.x, mapBotLeftPos.y + 0.02f, mapBotLeftPos.z + squareWidth * botGridPos.y);
        bright.transform.localScale = new Vector3(squareWidth * 0.75f, mapSquareHeight, squareWidth * 0.75f);
        bright.GetComponent<Renderer>().material = gridMat;
        SetObjectColor(bright, "_Color", Color.green);
        SetObjectColor(bright, "_EmissionColor", Color.green);
    }

    void MoveBot(float dx, float dy)
    {
        for (int k = 0; k < 2; k++) {
            GameObject path = GameObject.CreatePrimitive(PrimitiveType.Cube);
            path.transform.SetParent(map.transform);

            Vector3 botPos = new Vector3(mapBotLeftPos.x + squareWidth * botGridPos.x, mapBotLeftPos.y + 0.01f, mapBotLeftPos.z + squareWidth * botGridPos.y);
            path.transform.localPosition = botPos + new Vector3((k + 1) * dx * squareWidth / 2, 0, (k + 1) * dy * squareWidth / 2);
            path.transform.localScale = new Vector3(squareWidth / 2, mapSquareHeight, squareWidth / 2);
            path.GetComponent<Renderer>().material = gridMat;
            SetObjectColor(path, "_Color", Color.white);
            SetObjectColor(path, "_EmissionColor", Color.white);


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!moduleComplete) {
            if (Input.GetMouseButtonDown(0)) {
                if (IsMouseOver(submit)) {
                    CheckAnswer();
                }

                if ((IsMouseOver(commandButtons[delIndex])) && !((currentCommandGrid.x == 6) && (currentCommandGrid.y == -1))) {
                    commandGrid[(int)currentCommandGrid.x, (int)currentCommandGrid.y].GetComponent<MeshRenderer>().enabled = false;
                    currentCommandGrid.x--;
                    if (currentCommandGrid.x < 0) {
                        currentCommandGrid.y--;
                        currentCommandGrid.x = 6;
                    }
                }

                if (!(currentCommandGrid.x == 6 && currentCommandGrid.y == 2)) {
                    for (int i = 0; i < 6; i++) {
                        if ((IsMouseOver(commandButtons[i])) && (i != delIndex)) {
                            currentCommandGrid.x++;
                            if (currentCommandGrid.x > 6)
                            {
                                currentCommandGrid.x = 0;
                                currentCommandGrid.y++;
                            }
                            GameObject command = commandGrid[(int)currentCommandGrid.x, (int)currentCommandGrid.y];
                            SetObjectColor(command, "_Color", commandColors[i]);
                            SetObjectColor(command, "_EmissionColor", commandColors[i]);
                            command.GetComponent<MeshRenderer>().enabled = true;
                        }
                    }
                }
            }
        }
    }

    void GenerateGrid()
    {
        squareWidth = mapWidth / mapDimensions;
        mapBotLeftPos = new Vector3(mapPos.x - 2 * squareWidth, mapPos.y, mapPos.z - 2 * squareWidth);

        // Create backlight
        GameObject lines = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lines.transform.SetParent(map.transform);
        lines.transform.localPosition = mapPos;
        lines.transform.localScale = new Vector3(mapWidth, mapSquareHeight - 0.01f, mapWidth);
        lines.GetComponent<Renderer>().material = gridMat;
        SetObjectColor(lines, "_Color", gridBGColor);
        SetObjectColor(lines, "_EmissionColor", gridBGColor);
        lines.name = "GridLines";

        // Create squares
        for (int i = 0; i < mapDimensions; i++) {
            for (int j = 0; j < mapDimensions; j++) {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.SetParent(map.transform);
                go.transform.localPosition = new Vector3(mapBotLeftPos.x + squareWidth * i, mapBotLeftPos.y, mapBotLeftPos.z + squareWidth * j);
                go.transform.localScale = new Vector3(squareWidth * mapSquarePercentSize, mapSquareHeight, squareWidth * mapSquarePercentSize);
                go.GetComponent<Renderer>().material = gridMat;
                SetObjectColor(go, "_Color", gridColor);
                SetObjectColor(go, "_EmissionColor", gridColor);
                go.name = "Square" + (5 * i + j + 1);
                mapGrid[i, j] = go;
            }
        }
    }
}
