using System.Collections.Generic;
using UnityEngine;

public class BrightModuleScript : Module
{
    //Prefab for generating command object
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

    //Used for map gen, traversal, and usage
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

    //Used for command gen, traversal, and usage
    int delIndex;
    float commandDist = 0.2f;
    Vector3 commandTopLeftPos = new Vector3(-0.6f, 0, 0.2f);
    Vector2 currentCommandGrid = new Vector2(6, -1);

    //Random
    static System.Random random = new System.Random();

    //Materials and colors
    public Material gridMat;
    public Color gridColor;
    public Color gridBGColor;
    public Color litColor;
    Color[] commandColors = { Color.blue, Color.yellow, Color.green, Color.red, Color.cyan, Color.magenta };

    void Start()
    {
        base.Start();

        //Initialize arrays and list
        commandButtons = new GameObject[6];
        mapGrid = new GameObject[mapDimensions, mapDimensions];
        commandGrid = new GameObject[7, 3];
        brightPoints = new List<Vector2>();

        //Randomize color array
        for (int i = 0; i < commandColors.Length; ++i)
        {
            int j = random.Next(i, commandColors.Length);

            //If statements keep delIndex on magenta
            if (commandColors[i] == Color.magenta)
            {
                delIndex = j;
            }
            if (commandColors[j] == Color.magenta)
            {
                delIndex = i;
            }

            //Swap colors with random index
            Color temp = commandColors[i];
            commandColors[i] = commandColors[j];
            commandColors[j] = temp;

        }

        //Get reference to game objects
        submit = transform.Find("Submit").gameObject;
        map = transform.Find("Map").gameObject;
        commandOutline = transform.Find("Display").Find("Outline").gameObject;
        commands = transform.Find("Display").Find("Commands").gameObject;
        //Also set colors to buttons
        for (int i = 0; i < 6; i++) {
            commandButtons[i] = transform.Find("Buttons").Find("Button" + (i + 1)).gameObject;
            SetObjectColor(commandButtons[i], "_Color", commandColors[i]);
        }

        //Instantiate all the commands and disable renderer
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

        //Colour the command outline properly
        SetObjectColor(commandOutline, "_EmissionColor", gridColor);
        SetObjectColor(commandOutline, "_Color", gridColor);

        //Set points bot needs to brighten
        //TODO: Make based off of ID and serial instead of constant
        brightPoints.Add(new Vector2(3f, 2f));
        brightPoints.Add(new Vector2(4f, 1f));
        brightPoints.Add(new Vector2(4f, 4f));

        //Generate the map
        GenerateMap();
    }

    void Update()
    {
        if (!moduleComplete) {
            if (Input.GetMouseButtonDown(0)) {
                //Submit button click
                if (IsMouseOver(submit)) {
                    if (CheckAnswer()) {
                        DeactivateModule();
                    } else {
                        bombSource.strikes++;
                    }
                }

                //Clicked delete command and there are commands
                if ((IsMouseOver(commandButtons[delIndex])) && !((currentCommandGrid.x == 6) && (currentCommandGrid.y == -1))) {
                    //Stop rendering command to show it's to be not read and not shown
                    commandGrid[(int)currentCommandGrid.x, (int)currentCommandGrid.y].GetComponent<MeshRenderer>().enabled = false;
                    currentCommandGrid.x--;

                    // move to previous row if x smaller than 0
                    if (currentCommandGrid.x < 0) {
                        currentCommandGrid.y--;
                        currentCommandGrid.x = 6;
                    }
                }

                //Clicked any other command and the display isn't full
                if (!(currentCommandGrid.x == 6 && currentCommandGrid.y == 2)) {
                    for (int i = 0; i < 6; ++i) {

                        //Check each button, making sure it isn't the delete button
                        if ((IsMouseOver(commandButtons[i])) && (i != delIndex)) {
                            currentCommandGrid.x++;

                            //Move to next row if x is larger than 6
                            if (currentCommandGrid.x > 6) {
                                currentCommandGrid.x = 0;
                                currentCommandGrid.y++;
                            }

                            //Re-color the command at the next position and enable renderer
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

    
    void GenerateMap()
    {
        //Nice info
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
        for (int i = 0; i < mapDimensions; ++i) {
            for (int j = 0; j < mapDimensions; ++j) {
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

    bool CheckAnswer()
    {
        //Try to delete previous answer if it exists
        try {
            Destroy(answer);
        } catch (System.NullReferenceException e) {
            //Empty catch, idk what to put here so left empty
        }

        //Create a new answer object to hold path
        answer = new GameObject();
        answer.transform.SetParent(map.transform);
        answer.transform.localPosition = new Vector3(0, 0, 0);
        answer.transform.localScale = new Vector3(1, 1, 1);
        answer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        answer.name = "answer";

        //Traverse path according to command colors
        List<Vector2> brightened = new List<Vector2>();
        botGridPos = new Vector2(0, 0);
        for (int i = 0; i < currentCommandGrid.y + 1; ++i) {
            for (int j = 0; j < 7; ++j) {

                //Checks if there is a command (command isn't rendered if it isn't filled)
                if (commandGrid[j, i].GetComponent<MeshRenderer>().enabled) {
                    Color color = commandGrid[j, i].GetComponent<Renderer>().material.GetColor("_Color");

                    //Move left
                    if (color == Color.blue && botGridPos.x > 0) {
                        MoveBot(-1, 0);
                        botGridPos.x--;
                    //Move right
                    } else if (color == Color.yellow && botGridPos.x < 4) {
                        MoveBot(1, 0);
                        botGridPos.x++;
                    //Move down
                    } else if (color == Color.green && botGridPos.y > 0) {
                        MoveBot(0, -1);
                        botGridPos.y--;
                    //Move up
                    } else if (color == Color.red && botGridPos.y < 4) {
                        MoveBot(0, 1);
                        botGridPos.y++;
                    } else if (color == Color.cyan) {
                        //Check if brightened point is a correct point and hasn't already been brightened
                        if (brightPoints.Contains(botGridPos) && !brightened.Contains(botGridPos)) {
                            brightened.Add(botGridPos);
                        }
                        Brighten(botGridPos);

                    }
                }
            }
        }

        // Correct if brightened all 3 points and bot position is the top right corner
        if (brightened.Count == 3 && botGridPos.x == 4 && botGridPos.y == 4) {
            return true;
        }
        return false;
    }

    void Brighten (Vector2 pos)
    {
        //Creates a green square at position given
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
        //delta x and y used to determine direction of movement
        for (int k = 0; k < 2; k++) {

            //Create a white square to where the bot moves
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

}
