using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using SVGImporter;

public class GameManager : MonoBehaviour 
{
	// from WordLibrary.cs
	// Question
    private int questionId;
	private QuestionLibrary questionLib;
	public GameObject questionGameObject;
	public string questionStr;
	public Text questionText;
	public Text questionRatioText;
    private int totalQuestion;
	public GameObject answerGameObject; 
	private AnswerLibrary answerLib;
    private string[] letterCollections;              // Set of correct letter and wrong letter to be used for choosing an answer
	public Text correctText;
	
	// From GameController.cs
	public GameObject targetParent;                 // Parent for all target box
    public GameObject sourceParent;                 // Parent for all source box (keyboard enter)
	public GameObject letterParent;                 // Parent for all letter available
    private Vector3[] tempPositions;                // holds the actual Start positions of the letters

	// answer
    private string[,] tempTargetPositions;          // Hold the position of sourcebox and its letter
    private string[] tempLetterID;                   // Hold the letter id for sourcebox
    private int totalBoxInSourceBox = 16;          // Total box for sourcebox
    private int totalBoxInTargetBox = 16;           // Total box in targetbox
    private int totalBoxInRowOne = 8;             // Total box of targetbox in row 1
    private int totalBoxInRowTwo = 8;             // Total box of targetbox in row 2
	
	private float offsetX1 = 0;                     // X Offset for row 1
	private float offsetX2 = 0;                     // X offset for row 2

	// Input checking
	private bool touching = false;
	private float moveSpeed = 16f;
	public static bool isCurrentlyMoving = false;
	
    private JawiManager jawiManager;
    public GameObject globalScript;
	
    public string[] playerAnswer;
    private int jawiCurrentIndex;

    private bool playerWin = false;

    public Button btnOpenOneLetter;

    private GUIManager guiManager;
    public GameObject canvasScript;
    public GameObject answerjawiSVG;

    public void ClearPlayerAnswer(int index)
    { 
//        Debug.Log("ClearJawiAnswer at " + index);
        playerAnswer[index] = null;
    }

    static class RandomStringArrayTool
    {
        // source : http://www.dotnetperls.com/shuffle

        static System.Random _random = new System.Random();

        public static string[] RandomizeStrings(string[] arr)
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            // Add all strings from array
            // Add new random int each time
            foreach (string s in arr)
            {
                list.Add(new KeyValuePair<int, string>(_random.Next(), s));
            }
            // Sort the list by the random number
            var sorted = from item in list
                orderby item.Key
                select item;
            // Allocate new string array
            string[] result = new string[arr.Length];
            // Copy values to array
            int index = 0;
            foreach (KeyValuePair<int, string> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            // Return copied array
            return result;
        }
    }

	// Use this for initialization
	void Start ()
	{
		answerLib = answerGameObject.GetComponent<AnswerLibrary>();
		questionLib = questionGameObject.GetComponent<QuestionLibrary>();
        jawiManager = globalScript.GetComponent<JawiManager>();
        guiManager = canvasScript.GetComponent<GUIManager>();
              
        SaveManager.LoadData();
        guiManager.UpdateCoinInformation();

		InitLevel();

	}
	
	// Update is called once per frame
	void Update () 
	{
//	    LetterClick ();

        // Enable and disable player can open letter 
        if (System.Array.Exists(playerAnswer, element => element == null) 
            ||  System.Array.Exists(playerAnswer, element => element == string.Empty))
        {
            btnOpenOneLetter.interactable = true;
        }
        else
        {
            btnOpenOneLetter.interactable = false;
        }
	}
	
	void InitLevel()
	{
        totalQuestion = answerLib.setQuestion.Length;
        letterCollections = new string[totalBoxInSourceBox];
		
		if (PlayerPrefs.GetInt("Question ID") == 0 || PlayerPrefs.GetInt("Question ID") > totalQuestion)
		{
			Debug.Log("Question Id " + PlayerPrefs.GetInt("Question ID") + " total question : " + totalQuestion);
            SaveManager.questionID = 1;
            SaveManager.SaveData();
		}
		
		questionId = PlayerPrefs.GetInt("Question ID");
        Debug.Log("Question Id " + PlayerPrefs.GetInt("Question ID") + " total question : " + totalQuestion);
	    	
        questionStr = answerLib.setQuestion[questionId - 1].question;
		questionText.text = questionStr;
		questionRatioText.text = "Question : " + questionId + " / " + totalQuestion;
//		Debug.Log(questionStr);
		
        // Fill in lettercollection with all answer letter
        for (int i = 0; i < answerLib.setQuestion[questionId - 1].answerLength; i++)
        {
            letterCollections[i] = answerLib.setQuestion[questionId - 1].answerJawi[i];
        }

        // Fill in letter collection with unique random letter that is not in the answer
        for (int i = answerLib.setQuestion[questionId - 1].answerLength; i < letterCollections.Length; i++)
        {
            bool match = true;

            while (match)
            {
                string let = GetLetterJawi().ToString();

                if (System.Array.Exists(letterCollections, element => element == let))
                {
                    match = true;
                }
                else
                {
                    letterCollections[i] = let;
                    match = false;
                }

               
            }
        }

        // Randomize the letter
        letterCollections = RandomStringArrayTool.RandomizeStrings(letterCollections);
				
		tempTargetPositions = new string[totalBoxInSourceBox,2];
        tempLetterID = new string[totalBoxInSourceBox];
		
		StartCoroutine (SetupLevel());
		
		
	}
	

    // Setup level based on question
	IEnumerator SetupLevel()
	{
        // Init total jawi letter
        playerAnswer = new string[answerLib.setQuestion[questionId - 1].answerLength];
		
        tempPositions = new Vector3[totalBoxInSourceBox];
		
		for (int i = 0; i < tempTargetPositions.GetLength(0); i++)
		{
			for (int j = 0; j < tempTargetPositions.GetLength(1); j++)
			{
				tempTargetPositions[i,j] = string.Empty;
			}
		}
		
		// uncentered related targetboxs in row 1
        for (int i = 0; i < totalBoxInRowOne; i++)
		{
            float pos = targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition.x - offsetX1;     
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition = new Vector2 (pos, targetParent.GetComponent<RectTransform>().localPosition.y);
		}
        // uncentered related targetboxs in row 2
        for (int i = totalBoxInRowOne; i < (totalBoxInRowOne + totalBoxInRowTwo); i++)
		{
            float pos = targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition.x - offsetX2;
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition = new Vector2 (pos, targetParent.GetComponent<RectTransform>().localPosition.y - 100.5f);

		}
		
		// init center
		offsetX1 = 0;
		offsetX2 = 0;

        // Disable targetbox
        for(int i = 0; i < targetParent.transform.childCount; i++)
        {
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.SetActive(false);
        }
        // Disable sourcebox
        for(int i = 0; i < sourceParent.transform.childCount; i++)
        {
            sourceParent.transform.FindChild("sourceBox"+(i + 1)).gameObject.SetActive(false);
        }
        // Disable letter
        for(int i = 0; i < letterParent.transform.childCount; i++)
        {
            letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<SVGImage>().gameObject.SetActive(false);
        }
            
        // Enable/Disable related and unrelate sourcebox/letter
		for(int i = 0; i < totalBoxInSourceBox; i++)
		{				
            letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<SVGImage>().gameObject.SetActive(true);
           
            int jawiIndex = jawiManager.GetJawiIndex(letterCollections[i]);

            letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<SVGImage>().vectorGraphics = jawiManager.jawiCharacterSVG[jawiIndex];
			// enable box collider to be able to click on the letter
//            letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<Collider2D>().enabled = true;

            sourceParent.transform.FindChild("sourceBox"+(i + 1)).gameObject.SetActive(true);  
//			tempPositions[i] = sourceParent.transform.FindChild("sourceBox" + (i + 1)).position;
            tempPositions[i] = sourceParent.transform.FindChild("sourceBox" + (i + 1)).GetComponent<RectTransform>().localPosition;
			
		}
      
            
        // set active related box/letter for row 1
        for(int i = 0; i < answerLib.setQuestion[questionId - 1].answerLength1; i++)
        {
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.SetActive(true);
            offsetX1 = offsetX1 - targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition.x;

//            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.FindChild("Text").GetComponent<Text>().text = string.Empty;
//            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.FindChild("TextHint").GetComponent<Text>().text = string.Empty;

        }

        // set active related box/letter for row 2
        for (int i = totalBoxInRowOne; i < totalBoxInRowOne + answerLib.setQuestion[questionId - 1].answerLength2; i++)
        {
//            Debug.Log("length 2 = " +  answerLib.jawi[questionId - 1].answerLength2 );
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.SetActive(true);
            offsetX2 = offsetX2 - targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition.x;

//            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.FindChild("Text").GetComponent<Text>().text = string.Empty;
//            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.FindChild("TextHint").GetComponent<Text>().text = string.Empty;

        }

        // Offset for row 1
        offsetX1 = offsetX1 / answerLib.setQuestion[questionId - 1].answerLength1;
        // Offset for row 2
        if (answerLib.setQuestion[questionId - 1].answerLength2 != 0)
            offsetX2 = offsetX2 / ( answerLib.setQuestion[questionId - 1].answerLength2);
		
		// offset to centered related targetboxs
        // centered row 1
        for (int i = 0; i < totalBoxInRowOne; i++)
		{
            float pos = targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition.x + offsetX1;
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition = new Vector2 (pos, targetParent.GetComponent<RectTransform>().localPosition.y);
		}
        // centered row 2
        for (int i = totalBoxInRowOne; i < totalBoxInRowOne + totalBoxInRowTwo; i++)
		{
            float pos = targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition.x + offsetX2;
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.GetComponent<RectTransform>().localPosition = new Vector2 (pos, targetParent.GetComponent<RectTransform>().localPosition.y - 100.5f);
		}


        /*
        // Swap box Position from right to left for jawi for row 1
        for (int i = 0; i < answerLib.setQuestion[questionId - 1].answerLength1/2; i++)
        {
            Vector2 tempPos = targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.position;
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.position = targetParent.transform.FindChild("targetBox"+((answerLib.setQuestion[questionId - 1].answerLength1) - i)).gameObject.transform.position;
            targetParent.transform.FindChild("targetBox"+((answerLib.setQuestion[questionId - 1].answerLength1) - i)).gameObject.transform.position = tempPos;
        }

        // Swap box Position from right to left for jawi for row 2
        for (int i = 10; i < totalBoxInRowOne + (answerLib.setQuestion[questionId - 1].answerLength2/2); i++)
        {
            Vector2 tempPos = targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.position;
            targetParent.transform.FindChild("targetBox"+(i + 1)).gameObject.transform.position = targetParent.transform.FindChild("targetBox"+((totalBoxInTargetBox + answerLib.setQuestion[questionId - 1].answerLength2) - i)).gameObject.transform.position;
            targetParent.transform.FindChild("targetBox"+((totalBoxInTargetBox + answerLib.setQuestion[questionId - 1].answerLength2) - i)).gameObject.transform.position = tempPos;
        }
        */
		


        StartCoroutine(ResetLetters());

        guiManager.targetBoxMaterial.color = new Color32(99, 99, 99, 255);

		yield return 0;

       
	}
	
	
	public void SubmitWord()
	{
        // If there is still available space letter
        if (System.Array.Exists(playerAnswer, element => element == null) 
            ||  System.Array.Exists(playerAnswer, element => element == string.Empty))
        {
            
            guiManager.targetBoxMaterial.color = new Color32(99, 99, 99, 255);
//            guiManager.letterBoxMaterial.color = new Color32(99, 99, 99, 255);
            return;
        }
            
        if (Enumerable.SequenceEqual(playerAnswer, answerLib.setQuestion[questionId - 1].answerJawi))
        {
            guiManager.targetBoxMaterial.color = new Color32(39, 174, 96, 255);
//            guiManager.letterBoxMaterial.color = new Color32(39, 174, 96, 255);
            StartCoroutine( CorrectAnswer());

            SaveManager.coinAmount = SaveManager.coinAmount + 10;
            SaveManager.SaveData();

            guiManager.coinText.text = "Coin : " + SaveManager.coinAmount.ToString();
        }
        else
        {
            guiManager.targetBoxMaterial.color = new Color32(231, 76, 60, 255);
//            guiManager.letterBoxMaterial.color = new Color32(231, 76, 60, 255);
            Debug.Log("U get wrong answer");
        }

	}
        
	IEnumerator CorrectAnswer()
	{
        playerWin = true;

		correctText.gameObject.SetActive(true);
        answerjawiSVG.SetActive(true);


        answerjawiSVG.GetComponent<SVGImage>().vectorGraphics = answerLib.setQuestion[questionId - 1].answerSVG;
              
		yield return new WaitForSeconds(2f);

		correctText.gameObject.SetActive(false);
        answerjawiSVG.SetActive(false);
		UpdateQuestion();
	}
	
    public void LetterClick2(GameObject go)
    {
        // Check if there is empty slot in jawianswer
        for (int i = 0; i < playerAnswer.Length; i++)
        {
            if (playerAnswer[i] == null || playerAnswer[i] == string.Empty)
            {
                jawiCurrentIndex = i;
                //                        Debug.Log("jawiCurrentIndex : " + jawiCurrentIndex);
                break;
            }
        }

        // Get letter Id
        int letterId = System.Int32.Parse(go.name.Substring("letter".Length));

        Vector3 startPosition = go.transform.GetComponent<RectTransform>().localPosition;
        // Move the letter
        if (System.Array.Exists(playerAnswer, element => element == null) 
            ||  System.Array.Exists(playerAnswer, element => element == string.Empty)
            || startPosition.y == targetParent.transform.FindChild("targetBox"+1).GetComponent<RectTransform>().localPosition.y 
            || startPosition.y == targetParent.transform.FindChild("targetBox"+(totalBoxInRowOne + 1)).GetComponent<RectTransform>().localPosition.y)
        {
            //                    Debug.Log("Have null and string empty");
            StartCoroutine(MoveLetter (go.transform, letterId - 1));

        }
        else
        {
            // Full word in targetbox
            Debug.Log("full word already!");

        }
    }
	void LetterClick()
	{

        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && touching == false && !isCurrentlyMoving)
		{
//            RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), transform.forward, Mathf.Infinity);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
			//ray shooting out of the camera from where the mouse is
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//            if (Physics.Raycast(ray, out hit) && hit.collider.name.Contains ("letter"))
            if (hit.collider != null)
            {
                if (hit.collider.name.Contains ("letter"))
                {
                    // Check if there is empty slot in jawianswer
                    for (int i = 0; i < playerAnswer.Length; i++)
                    {
                        if (playerAnswer[i] == null || playerAnswer[i] == string.Empty)
                        {
                            jawiCurrentIndex = i;
    //                        Debug.Log("jawiCurrentIndex : " + jawiCurrentIndex);
                            break;
                        }
                    }
                        

                    // Get letter Id
                    int letterId = System.Int32.Parse(hit.collider.name.Substring("letter".Length));

                    Vector3 startPosition = hit.transform.position;

                    // Move the letter
                    if (System.Array.Exists(playerAnswer, element => element == null) 
                        ||  System.Array.Exists(playerAnswer, element => element == string.Empty)
                        || startPosition.y == targetParent.transform.FindChild("targetBox"+1).position.y 
                        || startPosition.y == targetParent.transform.FindChild("targetBox"+(totalBoxInRowOne + 1)).position.y)
                    {
    //                    Debug.Log("Have null and string empty");
                        StartCoroutine(MoveLetter (hit.transform, letterId - 1));

                    }
                    else
                    {
                        // Full word in targetbox
                        Debug.Log("full word already!");

                    }

                }
//                touching = true;
            }
			touching = true;
		}
		else
		{
			touching = false;
		}
	}
	
	/// <summary>
	/// Moves the letter to a target/source slot 
	/// </summary>
	IEnumerator MoveLetter(Transform transform, int letterIndex)
	{
		isCurrentlyMoving = true;
		float t = 0;
		
		// find the first '\0' char and count
		int target = 0;
		
		bool flag = false;
		
        // For upper target box (box 0-10) in row 1
        for (int i = 0; i < answerLib.setQuestion[questionId - 1].answerLength1; i++)
		{
            if (playerAnswer[i] == string.Empty || playerAnswer[i] == null)
            {
                target = i + 1;
                flag = true;
                break;
            }
                
		}
		
        // For targetbox (box 11-20) in row 2
        for (int i = totalBoxInRowOne; i < totalBoxInRowOne + answerLib.setQuestion[questionId - 1].answerLength2; i++)
		{
            if ((playerAnswer[i - totalBoxInRowOne + answerLib.setQuestion[questionId - 1].answerLength1] == string.Empty 
                || playerAnswer[i - totalBoxInRowOne + answerLib.setQuestion[questionId - 1].answerLength1] == null) 
                && flag == false)
            {
                target = i + 1;
                flag = true;
                break;
            }
		}

				
		// enable the collider for clicking
//        transform.GetComponent<Collider2D>().enabled = false;
		
        Vector3 startPosition = transform.GetComponent<RectTransform>().localPosition;
		Vector3 endPosition;
		
		// store and clear letter from targetBox/sourceBox
        if (startPosition.y == targetParent.transform.FindChild("targetBox"+1).GetComponent<RectTransform>().localPosition.y 
            || startPosition.y == targetParent.transform.FindChild("targetBox"+(totalBoxInRowOne + 1)).GetComponent<RectTransform>().localPosition.y)
		{
            Debug.Log("from targetbox : " + startPosition.y + " = " + targetParent.transform.FindChild("targetBox"+1).GetComponent<RectTransform>().localPosition.y);
			endPosition = tempPositions[letterIndex];
			endPosition.z = -0.1f;

            // If letter from first row
			int emptyTarget = System.Int32.Parse(tempTargetPositions[letterIndex, 1]);

            // If letter from second row
            if (startPosition.y == targetParent.transform.FindChild("targetBox"+(totalBoxInRowOne + 1)).GetComponent<RectTransform>().localPosition.y)
                emptyTarget = emptyTarget - totalBoxInRowOne + answerLib.setQuestion[questionId - 1].answerLength1;
            
            Debug.Log("[MoveLetter] = Clear jawi answer at index : " + emptyTarget);
            ClearPlayerAnswer(emptyTarget);

            jawiCurrentIndex = emptyTarget;

            tempLetterID[ System.Int32.Parse(tempTargetPositions[letterIndex, 1])] = string.Empty;


		}
		else
		{
            
            endPosition = targetParent.transform.FindChild("targetBox"+target).GetComponent<RectTransform>().localPosition;
            endPosition.z = -0.1f;

            Debug.Log("from sourceBox : jawiindex : "  + jawiCurrentIndex + " endposition = " + endPosition);

            string b = transform.GetComponent<SVGImage>().vectorGraphics.name;

            playerAnswer[jawiCurrentIndex] = transform.GetComponent<SVGImage>().vectorGraphics.name;
            // store letter and its temp position in targetBox
            tempTargetPositions[letterIndex, 0] = transform.GetComponent<SVGImage>().vectorGraphics.name;
            tempTargetPositions[letterIndex, 1] = (target - 1).ToString();


            tempLetterID[target - 1] = (letterIndex + 1).ToString();
		}
		
		// animate letter
		while (t < 1f) 
		{
			t += Time.deltaTime * moveSpeed;
            transform.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, t);
			yield return null;
		}

		SubmitWord();
		
		isCurrentlyMoving = false;
//        transform.GetComponent<Collider2D>().enabled = true;
		yield return 0;
	}
	
	IEnumerator ResetLetters()
	{       
		isCurrentlyMoving = true;
		
		for(int i = 0; i < totalBoxInSourceBox; i++)
		{
            Vector3 startPosition = letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<RectTransform>().localPosition;
			Vector3 endPosition = tempPositions[i];
			endPosition.z = -0.1f;
			
			/*while (t < 1f) 
			{
				t += Time.deltaTime * moveSpeed;
				letterParent.transform.FindChild("letter"+(i+1)).position = Vector3.Lerp(startPosition, endPosition, t);
				yield return null;
			}*/
                        letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<RectTransform>().localPosition = endPosition;
//            letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<BoxCollider2D>().enabled = true;
			yield return null;
		}
		
		tempPositions = new Vector3[totalBoxInSourceBox];

        // Clear all jawi in answer box
        for (int i = 0; i < playerAnswer.Length; i++)
        {
            ClearPlayerAnswer(i);
        }
            
        // Clear all temporary target position
		for (int i = 0; i < tempTargetPositions.GetLength(0); i++)
		{
			for (int j = 0; j < tempTargetPositions.GetLength(1); j++)
			{
				tempTargetPositions[i,j] = string.Empty;
			}
		}
		// Reset letter to its previous position and clear temporary letter ID
		for(int i = 0; i < totalBoxInSourceBox; i++)
		{

			// tempPositions holds the actual Start positions of the letters
            tempPositions[i] = sourceParent.transform.FindChild("sourceBox" + (i + 1)).GetComponent<RectTransform>().localPosition;
            tempLetterID[i] = string.Empty;
						
			yield return null;
		}
		
		isCurrentlyMoving = false;
		yield return 0;
	}
	
	IEnumerator ResetLetters(int letterIndex)
	{
		isCurrentlyMoving = true;

		float t = 0;
		
        Vector3 startPosition = letterParent.transform.FindChild("letter" + (letterIndex + 1)).position;
		Vector3 endPosition = tempPositions[letterIndex];
		endPosition.z = -0.1f;
		
		while (t < 1f) 
		{
			t += Time.deltaTime * moveSpeed;
            letterParent.transform.FindChild("letter" + (letterIndex + 1)).position = Vector3.Lerp(startPosition, endPosition, t);
			yield return null;
		}
		yield return null;
		
		
		StartCoroutine (SetupLevel());
		//InitLevel();
		
		isCurrentlyMoving = false;
		yield return 0;
	}
	
	public void ResetAllLetter()
	{
		StartCoroutine(ResetLetters());
	}
	
	public void Shuffle()
	{
		StartCoroutine (SetupLevel());
	}
	
	public void ShuffleAnswer()
	{
		StartCoroutine (SetupLevel());
	}
	public void UpdateQuestion()
	{
        SaveManager.questionID++;
        SaveManager.SaveData();
        Debug.Log("player pref : " + PlayerPrefs.GetInt("Question ID"));

        if (SaveManager.questionID <= totalQuestion)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            // level complete
            // Unlock Next Level
            PlayerPrefs.SetInt((PlayerPrefs.GetInt("Level ID") + 1).ToString(), (PlayerPrefs.GetInt("Level ID") + 1));
            // Pop Up level Complete Panel

            SceneManager.LoadScene("main");
        }

	}
	
	public void Restart()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}

    public string GetLetterJawi()
    {
        // This method returns a random lowercase letter.
        // ... Between 'a' and 'z' inclusize.
        int num = Random.Range(0, jawiManager.jawiStr.Length);
//        Debug.Log("num : " + num);
        string let = jawiManager.jawiStr[num];
        return let;
    }
	
	public void RemoveUnwantedLetters()
	{
        if (SaveManager.coinAmount >= 50)
        {
            // Reduce Coin
            SaveManager.coinAmount = SaveManager.coinAmount - 50;
            SaveManager.SaveData();
            guiManager.UpdateCoinInformation();

    		for (int i = 0; i < totalBoxInSourceBox; i++)
    		{
                string let = letterCollections[i];

                // hide letter that does not contain in answer letter
                if (!System.Array.Exists(answerLib.setQuestion[questionId - 1].answerJawi, element => element == let))
                {                
                    letterParent.transform.FindChild("letter" + (i + 1)).gameObject.SetActive(false);
                }

    		}

            // Clear unwanted letter inside jawiAnswer
            for (int i = 0; i < playerAnswer.Length; i++)
            {
                string let = playerAnswer[i];

                if (!System.Array.Exists(answerLib.setQuestion[questionId - 1].answerJawi, element => element == let))
                {
                    Debug.Log("[RemoveUnwantedLetters] = Clear jawi answer at index : " + i);   
                    ClearPlayerAnswer(i);

                }
            }
        }
        else
        {
            // Prompt player to buy coin or cancel
        }

            
 

	}
	
	IEnumerator OpenRandomLetter()
	{
        
		isCurrentlyMoving = true;
		float t = 0;
		int target = 0;
		int target2 = 0;
        int target3 = 0;
        bool flag = false;

        // Get correct random letter to be open
        int letterIndex = 0;
       
        // Get correct random letter to be open from sourcebox
        for (int i = 0; i < totalBoxInSourceBox; i++)
        {
            Vector3 startPosition3 = letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<RectTransform>().localPosition;

            // If the letter position is in targetbox, then do nothing, else : check whether the letter is correct or not
            if (startPosition3.y == targetParent.transform.FindChild("targetBox"+1).GetComponent<RectTransform>().localPosition.y 
                || startPosition3.y == targetParent.transform.FindChild("targetBox"+(totalBoxInRowOne + 1)).GetComponent<RectTransform>().localPosition.y)
            {
                //                Debug.Log("sama bai untuk first");
            }
            else
            {
                string let = letterParent.transform.FindChild("letter" + (i + 1)).GetComponent<SVGImage>().vectorGraphics.name;
//                Debug.Log("let : sama bai untuk first = " + let) ;
                // Check if the letter exist in the answer
                if (System.Array.Exists(answerLib.setQuestion[questionId - 1].answerJawi, element1 => element1 == let))
                {
                    // Get the index of the letter
                    for (int j = 0; j < answerLib.setQuestion[questionId - 1].answerLength; j++)
                    {
                        if (answerLib.setQuestion[questionId - 1].answerJawi[j] == let)
                        {
                            letterIndex = j;
                            // Check if the letter exist in the targetbox. If exist, find another letter. If not exist, that letter can be used to be open
                            if (playerAnswer[j] != let)
                            {
                                flag = true;
                                target = i + 1;
                                break;  // Stop for-loop answerLib.jawi[questionId - 1].answerLength
                            }
                        }
                    }

                    if (flag)
                        break;  // stop for-loop totalBoxInSourceBox
                }
            }
        }

        if (letterIndex < answerLib.setQuestion[questionId - 1].answerLength1)
        {
            target2 = letterIndex + 1;
            target3 = letterIndex + 1;
        }
        else
        {
            target2 = letterIndex + 1 + (totalBoxInRowTwo - answerLib.setQuestion[questionId - 1].answerLength1) ;
            target3 = letterIndex + 1;

        }
            
//        Debug.Log("target = " + target + " , target2 = " + target2 + " , target3 = " + target3);
        // Reposition letter in targetbox to its sourcebox and replace it with the correct letter
        if (!(tempLetterID[target2 - 1].Equals(string.Empty) || tempLetterID[target2 - 1].Equals(null)))
        {
            int letterId = System.Int32.Parse(tempLetterID[target2 - 1]);
//            Debug.Log("Letter Id = " + letterId + " -- target2 = " + (target2 - 1));

            // Move wrong letter to sourcebox
            Vector3 startPosition2 = letterParent.transform.FindChild("letter" + (letterId)).GetComponent<RectTransform>().localPosition;
            Vector3 endPosition2;
            endPosition2 = tempPositions[letterId - 1];
            endPosition2.z = -0.1f;
        
            // animate letter
            while (t < 1f) 
            {
                t += Time.deltaTime * moveSpeed;
                letterParent.transform.FindChild("letter"+(letterId)).GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition2, endPosition2, t);
                yield return null;
            }
        }

        // Clear player answer at selected index before replace with correct letter
        ClearPlayerAnswer(target3 - 1);

        t = 0;

        // Move correct letter from sourcebox to targetbox          
        Debug.Log("letter target " + target);
        Vector3 startPosition = letterParent.transform.FindChild("letter" + (target)).GetComponent<RectTransform>().localPosition;
		Vector3 endPosition;
        endPosition = targetParent.transform.FindChild("targetBox"+target2).GetComponent<RectTransform>().localPosition;
		endPosition.z = -0.1f;
		
        // Update player answer
        playerAnswer[target3 - 1] = letterParent.transform.FindChild("letter"+(target)).GetComponent<SVGImage>().vectorGraphics.name;
		// store letter and its temp position in targetBox
        tempTargetPositions[target- 1, 0] =  letterParent.transform.FindChild("letter"+(target)).GetComponent<SVGImage>().vectorGraphics.name;
        tempTargetPositions[target - 1, 1] = (target2 - 1).ToString();
        // Disable collider
//        letterParent.transform.FindChild("letter"+(target)).GetComponent<BoxCollider2D>().enabled = false;
		
		// animate letter
		while (t < 1f) 
		{
			t += Time.deltaTime * moveSpeed;
            letterParent.transform.FindChild("letter"+(target)).GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, t);
			yield return null;
		}


		SubmitWord();


        isCurrentlyMoving = false;
	}
	
	public void OpenOneLetter()
	{
        // if there is no empty string in answer box, then player can't use open correct letter
        if (!(System.Array.Exists(playerAnswer, element => element == string.Empty) || System.Array.Exists(playerAnswer, element =>element == null)))
        {
            // Sepatutnya tak payah return, tetapi bila dah full, button open one letter kena disable kot. sebab banyak sangat buggggggggg... zddfzdfzdfzdfsd
            // buttonopenoneletter.enable = false;
            return;
        }

        if (SaveManager.coinAmount >= 10)
        {
            // Reduce Coin
            SaveManager.coinAmount = SaveManager.coinAmount - 10;
            SaveManager.SaveData();
            guiManager.UpdateCoinInformation();

		    StartCoroutine(OpenRandomLetter());
        }
        else
        {
            // Prompt player to buy coin or cancel
        }
	}

    public void SolveQuestion()
    {
        if (SaveManager.coinAmount >= 100)
        {
            // Reduce Coin
            SaveManager.coinAmount = SaveManager.coinAmount - 100;
            SaveManager.SaveData();
            guiManager.UpdateCoinInformation();
            // Solve the question
            ResetAllLetter();
            StartCoroutine(SolveQuestionIEnumerator());
        }
        else
        {
            // Prompt player to buy coin or cancel
        }
    }

    public void SolveQuestionOpenOneLetter()
    {
       
        StartCoroutine(OpenRandomLetter());
    }

    IEnumerator SolveQuestionIEnumerator()
    {

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < answerLib.setQuestion[questionId - 1].answerLength; i++)
        {
            yield return new WaitForSeconds(0.1f);
            SolveQuestionOpenOneLetter();
        }
    }
				
}
