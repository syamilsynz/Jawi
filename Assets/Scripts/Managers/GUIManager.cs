using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour 
{
    public Material targetBoxMaterial;
    public Material letterBoxMaterial;

    public Text coinText;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void UpdateCoinInformation()
    {
        // Update text for related coin
        coinText.text = "Coin : " + SaveManager.coinAmount;
    }

    public void BuyCoin(int value)
    {
        SaveManager.coinAmount = SaveManager.coinAmount + value;
        UpdateCoinInformation();
    }
        
}
