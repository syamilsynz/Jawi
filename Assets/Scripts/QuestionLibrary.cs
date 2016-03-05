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

    [Space(5)] [Header("Timer")] [Space(5)] 
    public Category[] timer;

    void Awake()
    {
        // Initialize set of question for timer mode
        timer = animal1.Concat(family).ToArray();
        timer = timer.Concat(food).ToArray();

        // Randomize the question
        timer = RandomCategoryArrayTool.RandomizeCategory(timer);
    }


    static class RandomCategoryArrayTool
    {
        // source : http://www.dotnetperls.com/shuffle

        static System.Random _random = new System.Random();

        public static Category[] RandomizeCategory(Category[] arr)
        {
            List<KeyValuePair<int, Category>> list = new List<KeyValuePair<int, Category>>();
            // Add all strings from array
            // Add new random int each time
            foreach (Category s in arr)
            {
                list.Add(new KeyValuePair<int, Category>(_random.Next(), s));
            }
            // Sort the list by the random number
            var sorted = from item in list
                orderby item.Key
                select item;
            // Allocate new string array
            Category[] result = new Category[arr.Length];
            // Copy values to array
            int index = 0;
            foreach (KeyValuePair<int, Category> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            // Return copied array
            return result;
        }
    }



}



