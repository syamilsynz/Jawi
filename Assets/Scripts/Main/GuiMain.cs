using UnityEngine;
using System.Collections;

public class GuiMain : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void BuyCoin(int value)
    {
        SaveManager.coinAmount = SaveManager.coinAmount + value;
        SaveManager.SaveData();
    }
}
