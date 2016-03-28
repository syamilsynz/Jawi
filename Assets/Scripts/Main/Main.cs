using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour 
{
    public Text playerCoin;

    public ScrollRect myScrollRect;

	// Use this for initialization
	void Start () 
	{
//        PlayerPrefs.DeleteAll();
        // Load Playerpref data
        SaveManager.LoadData();

        myScrollRect.verticalNormalizedPosition =  PlayerPrefs.GetFloat("mapVerticalValue");;

	}
	
	// Update is called once per frame
	void Update () 
	{
        playerCoin.text = SaveManager.coinAmount.ToString();

	}

    public void GoToScene(string name)
	{
//		Application.LoadLevel(1);

        SceneManager.LoadScene(name);
	}

    public void SetMapPosition()
    {
//        Debug.Log("Vertical value : " + myScrollRect.verticalNormalizedPosition);
        PlayerPrefs.SetFloat("mapVerticalValue", myScrollRect.verticalNormalizedPosition);
    }
        
}
