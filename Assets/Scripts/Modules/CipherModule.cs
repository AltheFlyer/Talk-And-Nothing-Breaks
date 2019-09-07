using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipherModule : Module
{

    string[] answerWords = { "PUNCH", "KNOCK", "CHIPS", "JUICE", "TABLE", "CHAIR", "PLUSH", "LIGHT", "POPPY", "MONTH", "BRYAN", "ALLEN" };
    int[] answerNums = { 103, 157, 253, 415, 576, 613, 643, 734, 790, 804, 924, 997 };
    string[] keywords = { "was", "no", "the", "bomb", "is", "not", "safe", "its" };
    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        //set up encrypted word
        char[] answerWord = answerWords[1].ToCharArray();
        char[] keyword = keywords[1].ToCharArray();
        for (int i = 0; i < answerWord.Length; i++) {
            int cipherOffset = alphabet.IndexOf(keyword[i % keyword.Length]);
            int letterOffset = alphabet.IndexOf(answerWord[i]);
            int index = (cipherOffset + letterOffset) % 26;
            answerWord[i] = alphabet[index];
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
