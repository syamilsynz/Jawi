using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using SVGImporter;

public class QuestionLibrary : MonoBehaviour 
{	
    [System.Serializable]
    public class Category
    {
        public SVGAsset answerSVG;
        public string answerRumi;
        [Space(5)] 
        [Header("Enter Word1 and Word2 only")]
        [Space(5)] 
        public string[] answerJawi;
        public string[] word1;
        public string[] word2;
        [HideInInspector]
        public int answerLength;
        [HideInInspector]
        public int answerLength1;
        [HideInInspector]
        public int answerLength2;

        public string question;

        public void InitData()
        {
            // Initialize all the data related to word1 and word2
            answerJawi = word1.Concat(word2).ToArray();
            answerLength = answerJawi.Length;
            answerLength1 = word1.Length;
            answerLength2 = word2.Length;

        }

    }
        
    [Space(5)] [Header("Category Animal")] [Space(5)] 
    public Category[] animal1;       // Set of question/answer for animal
    public Category[] animal2;       // Set of question/answer for animal
    public Category[] animal3;       // Set of question/answer for animal
    [Space(5)] [Header("Category Family")] [Space(5)] 
    public Category[] family;       // Set of question/answer for family
    [Space(5)] [Header("Category Food")] [Space(5)] 
    public Category[] food;         // Set of question/answer for food
    [Space(5)] [Header("Category Ibadah")] [Space(5)] 
    public Category[] ibadah;       // Set of question/answer for ibadah
    [Space(5)] [Header("Category Kata-Kata Hikmah")] [Space(5)] 
    public Category[] kataKataHikmah;   // Set of kata kata hikmah





}



