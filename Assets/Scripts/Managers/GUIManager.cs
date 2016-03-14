using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SVGImporter;

public class GUIManager : MonoBehaviour 
{
    public GameObject gameManagerGameObject; 
    private GameManager gameManager;

    public Material targetBoxMaterial;
    public Material letterBoxMaterial;

    public Text coinText;

    // Complete level
    public GameObject completeLevelParent;
    public GameObject panelWin;             // Win current question

    public GameObject panelBuyHint;         // Hold all child object buy hint

    public GameObject panelBuyCoin;         // Hold all child object buy coin

    public GameObject panelGameplay;        // Hold all child object for gameplay

    public GameObject backgroundCover;      // Background to hide gameplay

    // Button Hint
    public Button btnOpenOneLetterWatchAd;
    public Button btnOpenOneLetter;
    public Button btnRemoveLetter;
    public Button btnSolveQuestion;


	// Use this for initialization
	void Start () 
    {
        gameManager = gameManagerGameObject.GetComponent<GameManager>();

        btnOpenOneLetter.transform.FindChild("TextPrice").GetComponent<Text>().text = gameManager.hintPriceOpenOneLetter.ToString();
        btnRemoveLetter.transform.FindChild("TextPrice").GetComponent<Text>().text = gameManager.hintPriceRemoveLetter.ToString();
        btnSolveQuestion.transform.FindChild("TextPrice").GetComponent<Text>().text = gameManager.hintPriceSolveQuestion.ToString();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void UpdateCoinInformation()
    {
        // Update text for related coin
        coinText.text = "x " + SaveManager.coinAmount.ToString();
    }

    //-------------
    // COIN
    //-------------
    public void BuyCoin(int value)
    {



    }

    public void BuyMiniPouch(int value)
    {
        IABManager.Instance.miniPouch = value;
        IABManager.Instance.BuyMiniPouch();
    }


    public void LevelComplete()
    {
        completeLevelParent.SetActive(true);
        // Display jawi answer
//        completeLevelParent.transform.FindChild("ImageJawi").GetComponent<SVGImage>().vectorGraphics = gameManager.answerLib.setQuestion[PlayerPrefs.GetInt("Question ID") - 1].answerSVG;
        // Display rumi answer
//        completeLevelParent.transform.FindChild("TextAnswer").GetComponent<Text>().text = gameManager.answerLib.setQuestion[PlayerPrefs.GetInt("Question ID") - 1].answerRumi;
        // Set the value of slider (progress)
//        completeLevelParent.transform.FindChild("SliderProgress").GetComponent<Slider>().maxValue = gameManager.answerLib.setQuestion.Length;
//        completeLevelParent.transform.FindChild("SliderProgress").GetComponent<Slider>().value = PlayerPrefs.GetInt("Question ID");
//        completeLevelParent.transform.FindChild("SliderProgress").FindChild("Text").GetComponent<Text>().text = PlayerPrefs.GetInt("Question ID") + "/" + gameManager.answerLib.setQuestion.Length;

        completeLevelParent.transform.FindChild("Pouch Receive").transform.FindChild("Text").GetComponent<Text>().text = gameManager.coinReceive.ToString();

        // Achievement Harta Qarun
//        GPGSManager.Instance.Achievement_Harta_Qarun(gameManager.coinReceive);

        if (gameManager.coinReceive >= GPGSManager.Instance.hartaQarunValue)
            GPGSManager.Instance.Achievement_Harta_Qarun();
    }
        
    public void Win()
    {
        panelGameplay.SetActive(false);
        panelWin.SetActive(true);

        // Display jawi answer
        panelWin.transform.FindChild("ImageJawi").GetComponent<SVGImage>().vectorGraphics = gameManager.answerLib.setQuestion[PlayerPrefs.GetInt("Question ID") - 1].answerSVG;

    }

    //----------
    // HINT
    //----------
    public void OpenBuyHint()
    {
        panelBuyHint.SetActive(true);
    }

    public void CloseBuyHint()
    {
        panelBuyHint.SetActive(false);
    }

    // Facebook Sharing
    public void FacebookShare()
    {
        StartCoroutine(PostFBScreenshot());
    }

    private IEnumerator PostFBScreenshot() {


        yield return new WaitForEndOfFrame();
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
        // Read screen contents into the texture
        tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 ); 
        tex.Apply();


        UM_ShareUtility.FacebookShare("Tak faham, apakah ini?", tex);

        Destroy(tex);

    }
     
    // Whatsapp Sharing
    public void WhatappsShare()
    {
        StartCoroutine(PostScreenshot());
    }
        
    private IEnumerator PostScreenshot() 
    {

        yield return new WaitForEndOfFrame();
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
//        Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
        Texture2D tex = new Texture2D( height, height, TextureFormat.RGB24, false );
        // Read screen contents into the texture
//        tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
        tex.ReadPixels( new Rect(0, 0, width, height), height/2 - width/2, 0 );
        tex.Apply();


        UM_ShareUtility.ShareMedia("Hint", "Tolong, apa jawapan jawi ni? \n download game JAWI di sini https://play.google.com/store/apps/details?id=com.ingeniworks.jawi", tex);

        Destroy(tex);

    }

    // ------------------
    // TIMER MODE REGION
    //--------------------

    public GameObject timerLevelCompleteParent;

    public void TimerLevelComplete()
    {
        timerLevelCompleteParent.SetActive(true);
        // Display rumi answer
        timerLevelCompleteParent.transform.FindChild("TextScore").GetComponent<Text>().text = "Markah : " + gameManager.scoreTimer;
        timerLevelCompleteParent.transform.FindChild("TextHighScore").GetComponent<Text>().text = "Markah Tertinggi : " + SaveManager.timerHighScore;

        timerLevelCompleteParent.transform.FindChild("Pouch Receive").transform.FindChild("Text").GetComponent<Text>().text = gameManager.coinReceive.ToString();
    }


        
}
