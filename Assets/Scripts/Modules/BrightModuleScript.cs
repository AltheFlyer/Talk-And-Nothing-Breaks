using System.Collections.Generic;
using UnityEngine;
using System;

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
    [SerializeField]
    List<Vector2> brightPoints;

    //Used for command gen, traversal, and usage
    [SerializeField]
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
        //brightPoints.Add(new Vector2(3f, 2f));
        //brightPoints.Add(new Vector2(4f, 1f));
        //brightPoints.Add(new Vector2(4f, 4f));

        //Generate the map
        
        GenerateMap();

        GenerateLights();
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
                        AddStrike();
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
            print("Oh no");
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

        // Correct if brightened all 3 points
        if (brightened.Count == brightPoints.Count) {
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

    void GenerateLights() 
    {
        //Note lights are on a 5x5 grid, zero-indexed, with standard mathematical x/y axes

        //Reference Values
        string serial = bombSource.serialCode;
        int id = bombSource.id;
        string idString = bombSource.idAsBinary;
        bool containsVowel = bombSource.serialContainsVowel;
        //int delIndex
        string numbers = "1234567890";
        string letters = "QWERTYUIOPASDFGHJKLZXCVBNM";
        string vowels = "AEIOU";
        string consonants = "QWRTYPSDFGHJKLZXCVBNM";

        //First Light
        int row = 0;
        int col = 0;

        int redPos = GetLightPosition(Color.red);
        int bluePos = GetLightPosition(Color.blue);
        int yellowPos = GetLightPosition(Color.yellow);
        int greenPos = GetLightPosition(Color.green);
        int cyanPos = GetLightPosition(Color.cyan);
        Color helpMe = new Color(1, 1, 0, 0);

        
        //Is ID even? (Column)
        if (id % 2 == 0) {
            //Does serial number end with a number?
            if (numbers.Contains(serial.Substring(serial.Length - 1, 1))) {
                col = 0;
            } else {
                col = 1;
            }
            //Is the position of the light (cyan) button in the top row?
            if (cyanPos < 3) {
                row = 4;
            } else {
                row = 3;
            }
        } else {
            if (numbers.Contains(serial.Substring(0,  1))) {
                col = 3;
            } else {
                col = 4;
            }
            //Is the 'up' light in a higher column than the 'down' light?
            //Is the position of the light (cyan) button in the top row?
            if (redPos < 3 && greenPos >= 3) {
                row = 4;
            } else {
                row = 3;
            }
        }

        brightPoints.Add(new Vector2(col, row));
        

        //First Light RNG
        //What position is the delete button?
        if (delIndex == 0 || delIndex == 3) {
            //Are there vowels in the serial code? If so, what is it?
            row = (delIndex / 3) + 1;
            if (bombSource.serialContainsVowel) {
                for (int i = 0; i < serial.Length; i++) {
                    if (vowels.Contains(serial.Substring(i, 1))) {
                        col = vowels.IndexOf(serial.Substring(i, 1));
                    }
                }
            } else {
                col = 0;
            }
        } else if (delIndex == 1) {
            row = 2;
            int vowelCount = 0;
            for (int i = 0; i < serial.Length; i++) {
                if (vowels.Contains(serial.Substring(i, 1))) {
                    vowelCount++;
                }
            }
            if (vowelCount == 0) {
                col = 4;
            } else if (vowelCount == 1) {
                col = 3;
            } else {
                col = 0;
            }
        } else if (delIndex == 2) {
            col = 2;
            int consonantCount = 0;
            for (int i = 0; i < serial.Length; i++) {
                if (consonants.Contains(serial.Substring(i, 1))) {
                    consonantCount++;
                }
            }
            row = Math.Min(consonantCount, 4);
        } else if (delIndex == 4) {
            if (id >= 128) {
                col = 2;
                row = 4;
            } else {
                col = 1;
                row = 2;
            }
        } else if (delIndex == 5) {
            if (id % 7 == 0) {
                row = 1;
                col = 4;
            } else if (id % 5 == 0) {
                row = 1;
                col = 3;
            } else if (id % 3 == 0) {
                row = 2;
                col = 1;
            } else if (id % 2 == 0) {
                row = 1;
                col = 1;
            } else {
                row = 1;
                col = 2;
            }
        }

        brightPoints.Add(new Vector2(col, row));
        SetObjectColor(mapGrid[col, row], "_Color", helpMe);
        
        //2nd RNG light
        //Make the lookup table;
        int[,] generatedCols = {
            {0, 0, 0, 0, 0, 4, 4, 4, 4},
            {3, 3, 2, 2, 2, 1, 1, 1, 1},
            {1, 3, 2, 4, 0, 0, 1, 2, 3},
            {0, 1, 2, 3, 4, 0, 1, 2, 3}
        };

        //Get number of letters in serial code
        int letterCount = 0;
        for (int i = 0; i < serial.Length; i++) {
            if (letters.Contains(serial.Substring(i, 1))) {
                letterCount++;
            }
        }

        //Is the 'up' button higher than the 'down' button?
        if (redPos < 3 && greenPos >= 3) {
            col = generatedCols[0, letterCount];
        //Is the 'left' button to the left of the 'right' button?
        } else if (bluePos % 3 < yellowPos % 3) {
            col = generatedCols[1, letterCount];
        //Is the 'delete' button directly left of the 'light' button?
        } else if (delIndex != 2 && delIndex + 1 == cyanPos) {
            col = generatedCols[2, letterCount];
        } else {
            col = generatedCols[3, letterCount];
        }

        int oneCount = 0;
        for (int i = 0; i < idString.Length; i++) {
            if ("1" == idString.Substring(i, 1)) {
                oneCount++;
            }
        }

        //Is the up button directly above delete or light?
        if (redPos < 3 && (redPos + 3 == delIndex || redPos + 3 == cyanPos)) {
            row = 0;
        //'Right' directly below delete or light?
        } else if (yellowPos >= 3 && (yellowPos - 3 == delIndex || yellowPos - 3 == cyanPos)) {
            row = 4;
        //'Left' directly below delete or light?
        } else if (bluePos >= 3 && (bluePos - 3 == delIndex || bluePos - 3 == cyanPos)) {
            row = 3;
        //Are 'down', delete, or 'down', light in the same row?
        } else if (greenPos % 3 == delIndex % 3 ||  greenPos % 3 ==  cyanPos % 3) {
            row = 2;
        } else {
            row = 1;
        }

        if (oneCount <= 4) {
            row += oneCount;
        } else {
            row += (oneCount - 4);
        }
        
        row = row % 5;

        if ((row == 0 && col == 0) || brightPoints.Contains(new Vector2(col, row))) {
            if (!brightPoints.Contains(new Vector2(0, 2))) {
                col = 0;
                row = 2;
            } else if (!brightPoints.Contains(new Vector2(0, 4))) {
                col = 0;
                row = 4;
            } else {
                col = 4;
                row = 4;
            }
        }

        brightPoints.Add(new Vector2(col, row));
        SetObjectColor(mapGrid[col, row], "_Color", helpMe);
    }

    int GetLightPosition(Color c)
    {
        for (int i = 0; i < 6; i++) {
            if (commandColors[i] == c) {
                return i;
            }
        }
        //Bad bad bad
        print("GET LIGHT POSITION FAILED");
        return 0;
    }

}
