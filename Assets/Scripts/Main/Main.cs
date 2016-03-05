using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour 
{
    public Text playerCoin;

	// Use this for initialization
	void Start () 
	{
        // Load Playerpref data
        SaveManager.LoadData();
	}
	
	// Update is called once per frame
	void Update () 
	{
        playerCoin.text = "Anda ada " + SaveManager.coinAmount.ToString();
	}

    public void GoToScene(string name)
	{
//		Application.LoadLevel(1);

        SceneManager.LoadScene(name);
	}
        
}
