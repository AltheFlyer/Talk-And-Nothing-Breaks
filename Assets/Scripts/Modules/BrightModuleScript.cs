using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrightModuleScript : Module
{
    [SerializeField]
    GameObject commandPrefab;

    GameObject submit;
    GameObject commandOutline;
    GameObject commands;
    GameObject map;
    GameObject answer;
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
    List<Vector2> brightPoints;

    int delIndex;
    float commandDist = 0.2f;
    Vector3 commandTopLeftPos = new Vector3(-0.6f, 0, 0.2f);
    Vector2 currentCommandGrid = new Vector2(6, -1);

    static System.Random random = new System.Random();
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
        for (int i = 0; i < commandColors.Length; ++i)
        {
            int j = random.Next(i, commandColors.Length);
            if (commandColors[i] == Color.magenta) {
                delIndex = j;
            }
            if (commandColors[j] == Color.magenta) {
                delIndex = i;
            }
            Color temp = commandColors[i];
            commandColors[i] = commandColors[j];
            commandColors[j] = temp;
            
        }

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

        brightPoints = new List<Vector2>();
        brightPoints.Add(new Vector2(3f, 2f));
        brightPoints.Add(new Vector2(4f, 1f));
        brightPoints.Add(new Vector2(4f, 4f));

        

        

        GenerateGrid();
    }


    bool CheckAnswer()
    {
        try
        {
            Destroy(answer);
        } catch (System.NullReferenceException e)
        {
            
        }

        answer = new GameObject();
        answer.transform.SetParent(map.transform);
        answer.transform.localPosition = new Vector3(0, 0, 0);
        answer.transform.localScale = new Vector3(1, 1, 1);
        answer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        answer.name = "answer";
        List<Vector2> brightened = new List<Vector2>();
        botGridPos = new Vector2(0, 0);
        for (int i = 0; i < currentCommandGrid.y + 1; i++) {
            for (int j = 0; j < 7; j++) {
                if (commandGrid[j, i].GetComponent<MeshRenderer>().enabled) {
                    Color color = commandGrid[j, i].GetComponent<Renderer>().material.GetColor("_Color");
                    if (color == Color.blue && botGridPos.x > 0) {
                        MoveBot(-1, 0);
                        botGridPos.x--;
                    } else if (color == Color.yellow && botGridPos.x < 4) {
                        MoveBot(1, 0);
                        botGridPos.x++;
                    } else if (color == Color.green && botGridPos.y > 0) {
                        MoveBot(0, -1);
                        botGridPos.y--;
                    } else if (color == Color.red && botGridPos.y < 4) {
                        MoveBot(0, 1);
                        botGridPos.y++;
                    } else if (color == Color.cyan) {
                        print(j + " " + i);
                        if (brightPoints.Contains(botGridPos) && !brightened.Contains(botGridPos))
                        {
                            brightened.Add(botGridPos);
                        }
                        Brighten(botGridPos);

                    }
                }
            }
        }
        print(brightened.Count);
        if (brightened.Count == 3 && botGridPos.x == 4 && botGridPos.y == 4) {
            return true;
        }
        return false;
    }

    void Brighten (Vector2 pos)
    {
        GameObject bright = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bright.transform.SetParent(answer.transform);

        bright.transform.localPosition = new Vector3(mapBotLeftPos.x + squareWidth * botGridPos.x, mapBotLeftPos.y + 0.002f, mapBotLeftPos.z + squareWidth * botGridPos.y);
        bright.transform.localScale = new Vector3(squareWidth * 0.75f, mapSquareHeight, squareWidth * 0.75f);
        bright.transform.localRotation = Quaternion.Euler(0, 0, 0);

        bright.GetComponent<Renderer>().material = gridMat;
        SetObjectColor(bright, "_Color", Color.green);
        SetObjectColor(bright, "_EmissionColor", Color.green);
    }

    void MoveBot(float dx, float dy)
    {
        for (int k = 0; k < 2; k++) {
            GameObject path = GameObject.CreatePrimitive(PrimitiveType.Cube);
            path.transform.SetParent(answer.transform);

            Vector3 botPos = new Vector3(mapBotLeftPos.x + squareWidth * botGridPos.x, mapBotLeftPos.y + 0.001f, mapBotLeftPos.z + squareWidth * botGridPos.y);
            path.transform.localPosition = botPos + new Vector3((k + 1) * dx * squareWidth / 2, 0, (k + 1) * dy * squareWidth / 2);
            path.transform.localScale = new Vector3(squareWidth / 2, mapSquareHeight, squareWidth / 2);
            path.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
                    if (CheckAnswer())
                    {
                        DeactivateModule();
                    } else
                    {
                        bombSource.strikes++;
                    }
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

        // Initial white square
        GameObject initialBot = GameObject.CreatePrimitive(PrimitiveType.Cube);
        initialBot.transform.SetParent(map.transform);
        initialBot.transform.localPosition = new Vector3(mapBotLeftPos.x, mapBotLeftPos.y + 0.001f, mapBotLeftPos.z);
        initialBot.transform.localScale = new Vector3(squareWidth / 2, mapSquareHeight, squareWidth / 2);
        initialBot.transform.localRotation = Quaternion.Euler(0, 0, 0);
        initialBot.GetComponent<Renderer>().material = gridMat;
        SetObjectColor(initialBot, "_Color", Color.white);
        SetObjectColor(initialBot, "_EmissionColor", Color.white);
        initialBot.name = "bot";

        // Create backlight
        GameObject lines = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lines.transform.SetParent(map.transform);
        lines.transform.localPosition = mapPos;
        lines.transform.localScale = new Vector3(mapWidth, mapSquareHeight - 0.001f, mapWidth);
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
