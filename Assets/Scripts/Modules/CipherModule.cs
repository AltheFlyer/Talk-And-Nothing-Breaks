using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CipherModule : Module
{

    GameObject encrypted;
    GameObject answer;
    GameObject left;
    GameObject right;
    GameObject submit;

    static System.Random random = new System.Random();

    string[] answerWords = { "PUNCH", "BRYAN", "TRICK", "ALBUM", "TABLE", "ALLOW", "PLUSH", "ALONE", "BRAIN", "TRAIN","BREAD", "PAINS", "ALLEN", "BRICK", "TALON", "PRICK" };
    int[] answerNums = { 103, 157, 223, 253, 364, 415, 420, 576, 613, 643, 666, 734, 790, 804, 924, 997 };
    string[] keywords = { "HA", "WAIT", "NO", "THE", "BOMB", "IS", "NOT", "SAFE", "ITS" };
    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string vowels = "AEIOU";
    string answerWord;
    int answerNumIndex;
    int currentNumIndex;
    int numVowels;
    String decrypted;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        //Find game objects
        encrypted = transform.Find("Encrypted Message").gameObject;
        answer = transform.Find("Answer Number").gameObject;
        left = transform.Find("Left Arrow").gameObject;
        right = transform.Find("Right Arrow").gameObject;
        submit = transform.Find("Submit").gameObject;

        //set up encrypted word
        currentNumIndex = 0;
        answerNumIndex = random.Next(0, answerWords.Length);
        decrypted = answerWords[answerNumIndex];
        print(decrypted);

        //Find number of vowels in serial code
        numVowels = 0;
        foreach (char letter in bombSource.serialCode) {
            if (vowels.Contains(letter.ToString())) {
                numVowels++;
            }
        }
        print(decrypted);
        generateAnswer();
        print(decrypted);
        answer.GetComponent<TMP_Text>().text = answerNums[currentNumIndex].ToString();
        print(decrypted);
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
                    AddStrike("Caesar Cipher");
                }
            }

            if (IsMouseOver(left) && currentNumIndex > 0) {
                currentNumIndex--;
                answer.GetComponent<TMP_Text>().text = answerNums[currentNumIndex].ToString();
            }
            if (IsMouseOver(right) && currentNumIndex < answerNums.Length - 1) {
                currentNumIndex++;
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
        /* Hardcore version
        char[] word = decrypted.ToCharArray();
        int keywordIndex = 0;
        //Generate keyword based on number of strikes and number of vowels in serial code
        if (bombSource.strikes == 2) {
            if (numVowels < 1) {
                keywordIndex = 0;
            }
            else if (numVowels < 3)  {
                keywordIndex = 6;
            }
            else {
                keywordIndex = 4;
            }
        }
        else if (bombSource.strikes == 1) {
            if (numVowels < 1) {
                keywordIndex = 3;
            }
            else if (numVowels < 3) {
                keywordIndex = 7;
            }
            else {
                keywordIndex = 2;
            }
        }
        else {
            if (numVowels < 1) {
                keywordIndex = 1;
            }
            else if (numVowels < 3)  {
                keywordIndex = 5;
            }
            else {
                keywordIndex = 8;
            }
        }
        char[] keyword = keywords[keywordIndex].ToCharArray();

        for (int i = 0; i < word.Length; ++i) {
            int cipherOffset = alphabet.IndexOf(keyword[i % keyword.Length]);
            int letterOffset = alphabet.IndexOf(word[i]);
            int index = (cipherOffset + letterOffset) % 26;
            word[i] = alphabet[index];
        }
        answerWord = new String(word);
        encrypted.GetComponent<TMP_Text>().text = answerWord;
        */
        int shift = 0;
        if (bombSource.strikes == 2) {
            if (numVowels < 1) {
                shift = 17;
            } else if (numVowels < 3) {
                shift = 14;
            } else {
                shift = 9;
            }
        } else if (bombSource.strikes == 1) {
            if (numVowels < 1) {
                shift = 23;
            } else if (numVowels < 3) {
                shift = 1;
            } else {
                shift = 20;
            }
        } else {
            if (numVowels < 1) {
                shift = 11;
            } else if (numVowels < 3) {
                shift = 6;
            } else  {
                shift = 16;
            }
        }
        char[] word = decrypted.ToCharArray();
        for (int i = 0; i < word.Length; i++) {
            int index = alphabet.IndexOf(word[i]) - shift;
            if (index < 0) {
                index += 26;
            }
            word[i] = alphabet[index];
        }
        answerWord = new String(word);
        encrypted.GetComponent<TMP_Text>().text = answerWord;
    }
}
