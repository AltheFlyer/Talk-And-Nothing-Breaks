using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTimerScript : NeutralModule
{

    GameObject[] digits;
    GameObject[] strikes;

    [SerializeField]
    Color lightColor;
    [SerializeField]
    Color blankColor;

    IEnumerator countDown;
    public float secondsLeft = 100;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        digits = new GameObject[4];

        Transform digitHolder = transform.Find("Timer");

        for (int i = 0; i < 4; i++) {
            digits[i] = digitHolder.Find("Digit " + (i + 1).ToString()).gameObject;
            for (int j = 0; j < 7; j++) {
                SetObjectColor(digits[i].transform.Find(GetSegmentName(j)).gameObject, "_EmissionColor", lightColor);
                digits[i].transform.Find(GetSegmentName(j)).gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }
        }

        //Enable center colon
        SetObjectColor(digitHolder.Find("Colon").Find("Bot").gameObject, "_Color", lightColor);
        SetObjectColor(digitHolder.Find("Colon").Find("Bot").gameObject, "_EmissionColor", lightColor);

        SetObjectColor(digitHolder.Find("Colon").Find("Top").gameObject, "_Color", lightColor);
        SetObjectColor(digitHolder.Find("Colon").Find("Top").gameObject, "_EmissionColor", lightColor);
                
        countDown = CountDown();
        StartCoroutine(countDown);


        strikes = new GameObject[3];
        Transform strikeHolder = transform.Find("Strikes");

        //Initialize strikes
        for (int i = 0; i < 3; i++) {
            strikes[i] = strikeHolder.Find("Strike " + (i + 1).ToString()).gameObject;
            SetObjectColor(strikes[i].transform.Find("Fractured Cross").gameObject, "_Color", blankColor);
            SetObjectColor(strikes[i].transform.Find("Main Cross").gameObject, "_Color", blankColor);

            strikes[i].transform.Find("Fractured Cross").gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            strikes[i].transform.Find("Main Cross").gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }
    }

    void SetNumber(int digit, int number) 
    {   
        //HARDCODING!!!
        if (number == 1) {
            EnableSegments(digit, new int[] {2, 5});
        } else if (number == 2) {
            EnableSegments(digit, new int[] {0, 2, 3, 4, 6});
        } else if (number == 3) {
            EnableSegments(digit, new int[] {0, 2, 3, 5, 6});
        } else if (number == 4) {
            EnableSegments(digit, new int[] {1, 2, 3, 5});
        } else if (number == 5) {
            EnableSegments(digit, new int[] {0, 1, 3, 5, 6});
        } else if (number == 6) {
            EnableSegments(digit, new int[] {0, 1, 3, 4, 5, 6});
        } else if (number == 7) {
            EnableSegments(digit, new int[] {0, 2, 5});
        } else if (number == 8) {
            EnableSegments(digit, new int[] {0, 1, 2, 3, 4, 5, 6});
        } else if (number == 9) {
            EnableSegments(digit, new int[] {0, 1, 2, 3, 5});
        } else if (number == 0) {
            EnableSegments(digit, new int[] {0, 1, 2, 4, 5, 6});
        }
    }

    ///Enables segments in a digit, and disables the rest
    void EnableSegments(int digit, int[] segments) {
        int arrIndex = 0; //Used for the segments array
        //Loop through each segment
        for (int i = 0; i < 7; i++) {
            GameObject segment = digits[digit].transform.Find(GetSegmentName(i)).gameObject;
            
            if (arrIndex < segments.Length && segments[arrIndex] == i) {
                SetObjectColor(segment, "_Color", lightColor);
                segment.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                arrIndex++;
            } else {
                SetObjectColor(segment, "_Color", blankColor);
                segment.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }
        }
    }

    string GetSegmentName(int seg) {
        if (seg == 0) {
            return "Segment U";
        } else if (seg == 1) {
            return "Segment LU";
        } else if (seg == 2) {
            return "Segment RU";
        } else if (seg == 3) {
            return "Segment M";
        } else if (seg == 4) {
            return "Segment LB";
        } else if (seg == 5) {
            return "Segment RB";
        } else /*(seg == 6) */{
            return "Segment B";
        }
    }

    IEnumerator CountDown() {
        while (secondsLeft > 0) {
            secondsLeft -= Time.deltaTime;
            //Minutes displayed or not
            if (secondsLeft > 60) {
                //4 digits needed (>9:59)
                SetNumber(0, (int) (secondsLeft / 600));
                SetNumber(1, (int) ((secondsLeft / 60) % 10));
                SetNumber(2, (int) ((secondsLeft % 60) / 10));
                SetNumber(3, (int) ((secondsLeft % 10)));
            } else {
                //SECONDS ONLY OMG
                SetNumber(0, (int) (secondsLeft / 10));
                SetNumber(1, (int) (secondsLeft % 10));
                SetNumber(2, (int) ((secondsLeft * 10) % 10));
                SetNumber(3, (int) ((secondsLeft * 100) % 10));
            }
            yield return null;
        }
        //Oh no... (I'll make this code less crappy later)
        bombSource.strikes += 99;

        StopCoroutine(countDown);
    }

    public void AddStrike(int numStrikes) {
        SetObjectColor(strikes[numStrikes - 1].transform.Find("Fractured Cross").gameObject, "_Color", lightColor);
        SetObjectColor(strikes[numStrikes - 1].transform.Find("Main Cross").gameObject, "_Color", lightColor);

        SetObjectColor(strikes[numStrikes - 1].transform.Find("Fractured Cross").gameObject, "_EmissionColor", lightColor);
        SetObjectColor(strikes[numStrikes - 1].transform.Find("Main Cross").gameObject, "_EmissionColor", lightColor);

        strikes[numStrikes - 1].transform.Find("Fractured Cross").gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        strikes[numStrikes - 1].transform.Find("Main Cross").gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
    }
}
