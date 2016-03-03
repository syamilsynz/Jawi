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

	// Use this for initialization
	void Start () 
    {
        gameManager = gameManagerGameObject.GetComponent<GameManager>();
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
        SaveManager.coinAmount = SaveManager.coinAmount + value;
        UpdateCoinInformation();
    }


    public void LevelComplete()
    {
        completeLevelParent.SetActive(true);
        // Display jawi answer
        completeLevelParent.transform.FindChild("ImageJawi").GetComponent<SVGImage>().vectorGraphics = gameManager.answerLib.setQuestion[PlayerPrefs.GetInt("Question ID") - 1].answerSVG;
        // Display rumi answer
        completeLevelParent.transform.FindChild("TextAnswer").GetComponent<Text>().text = gameManager.answerLib.setQuestion[PlayerPrefs.GetInt("Question ID") - 1].answerRumi;
        // Set the value of slider (progress)
        completeLevelParent.transform.FindChild("SliderProgress").GetComponent<Slider>().maxValue = gameManager.answerLib.setQuestion.Length;
        completeLevelParent.transform.FindChild("SliderProgress").GetComponent<Slider>().value = PlayerPrefs.GetInt("Question ID");
        completeLevelParent.transform.FindChild("SliderProgress").FindChild("Text").GetComponent<Text>().text = PlayerPrefs.GetInt("Question ID") + "/" + gameManager.answerLib.setQuestion.Length;
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
        
        
}
