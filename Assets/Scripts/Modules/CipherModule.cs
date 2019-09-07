using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CipherModule : Module
{

    GameObject encryted;
    GameObject answer;
    GameObject left;
    GameObject right;
    GameObject submit;

    static System.Random random = new System.Random();

    string[] answerWords = { "PUNCH", "KNOCK", "CHIPS", "JUICE", "TABLE", "CHAIR", "PLUSH", "LIGHT", "POPPY", "MONTH", "BRYAN", "ALLEN" };
    int[] answerNums = { 103, 157, 253, 415, 576, 613, 643, 734, 790, 804, 924, 997 };
    string[] keywords = { "ha", "wait", "no", "the", "bomb", "is", "not", "safe", "its" };
    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string vowels = "AEIOU";
    string answerWord;
    int answerNumIndex;
    int currentNumIndex;
    int numVowels;
    char[] decrypted;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        //Find game objects
        encryted = transform.Find("Encrypted Message").gameObject;
        answer = transform.Find("Answer Number").gameObject;
        left = transform.Find("Left Arrow").gameObject;
        right = transform.Find("Right Arrow").gameObject;
        submit = transform.Find("Submit").gameObject;

        //set up encrypted word
        currentNumIndex = 0;
        answerNumIndex = random.Next(0, answerWords.Length);
        decrypted = answerWords[answerNumIndex].ToCharArray();

        //Find number of vowels in serial code
        numVowels = 0;
        foreach (char letter in bombSource.serialCode) {
            if (vowels.Contains(letter.ToString())) {
                numVowels++;
            }
        }

        generateAnswer();

        answer.GetComponent<TMP_Text>().text = answerNums[currentNumIndex].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        generateAnswer();
        if (!moduleComplete && Input.GetMouseButtonDown(0)) {
            //Submit button click
            if (IsMouseOver(submit)) {
                if (CheckAnswer()) {
                    DeactivateModule();
                }
                else {
                    AddStrike();
                }
            }

            if (IsMouseOver(left) && currentNumIndex > 0) {
                answerNumIndex--;
                answer.GetComponent<TMP_Text>().text = answerNums[currentNumIndex].ToString();
            }
            if (IsMouseOver(right) && answerNumIndex < answerNums.Length) {
                answerNumIndex++;
                answer.GetComponent<TMP_Text>().text = answerNums[currentNumIndex].ToString();
            }
        }
    }

    bool CheckAnswer()
    {
        if (currentNumIndex == answerNumIndex) {
            return true;
        }
        return false;
    }

    void generateAnswer ()
    {
        int keywordIndex = 0;
        //Generate keyword based on number of strikes and number of vowels in serial code
        if (bombSource.strikes == 2) {
            if (numVowels < 1) {
                keywordIndex = 0;
            }
            else if (numVowels < 3)  {
                keywordIndex = 2;
            }
            else {
                keywordIndex = 5;
            }
        }
        else if (bombSource.strikes == 1) {
            if (numVowels < 1) {
                keywordIndex = 3;
            }
            else if (numVowels < 3) {
                keywordIndex = 6;
            }
            else {
                keywordIndex = 8;
            }
        }
        else {
            if (numVowels < 1) {
                keywordIndex = 1;
            }
            else if (numVowels < 3)  {
                keywordIndex = 4;
            }
            else {
                keywordIndex = 7;
            }
        }
        char[] keyword = keywords[keywordIndex].ToCharArray();

        for (int i = 0; i < decrypted.Length; ++i) {
            int cipherOffset = alphabet.IndexOf(keyword[i % keyword.Length]);
            int letterOffset = alphabet.IndexOf(decrypted[i]);
            int index = (cipherOffset + letterOffset) % 26;
            decrypted[i] = alphabet[index];
        }
        answerWord = decrypted.ToString();
        encryted.GetComponent<TMP_Text>().text = answerWord;
    }
}
