using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour 
{
    public static int coin;
		
    public void AddCoin(int value)
	{
        SaveManager.coinAmount = SaveManager.coinAmount + value;
        SaveManager.SaveData();
	}
	
    public void SubtractCoin(int value)
	{
        SaveManager.coinAmount = SaveManager.coinAmount - value;
        SaveManager.SaveData();
	}
	
	public int GetCoin()
	{
		return coin;
	}
	
	public void SetCoin(int value)
	{
        coin = value;
	}
	
    public void UpdateCoin(int value)
    {
        SaveManager.coinAmount = SaveManager.coinAmount + value;
        SaveManager.SaveData();
    }
	
}
