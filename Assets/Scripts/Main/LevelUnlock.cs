﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LevelUnlock : MonoBehaviour 
{
    public string levelId;

	// Use this for initialization
	void Start () 
    {
        PlayerPrefs.SetString("1","1");

        CheckLevelUnlock(levelId);

        CheckAchievement();
	}
	
    public void CheckLevelUnlock(string levelId)
    {
        this.transform.FindChild("Text").gameObject.GetComponent<Text>().text = levelId.ToString();

        if (PlayerPrefs.HasKey(levelId))
        {
            this.GetComponent<Button>().interactable = true;

            // Change the color of the button
            this.transform.FindChild("ImageUnlock").gameObject.SetActive(true);
            this.transform.FindChild("ImageLock").gameObject.SetActive(false);

            // Check next level is unlock?
            if (PlayerPrefs.HasKey((int.Parse(levelId) + 1).ToString()))
                this.GetComponent<Animator>().enabled = false; // Disable Animation if next level is unlock
            else
                this.GetComponent<Animator>().enabled = true;   // Enable Animation if no other higher level than this level
        }
        else
        {
            this.GetComponent<Button>().interactable = false;

            // Change the color of the button
            this.transform.FindChild("ImageUnlock").gameObject.SetActive(false);
            this.transform.FindChild("ImageLock").gameObject.SetActive(true);

            // Disable Animation
            this.GetComponent<Animator>().enabled = false;
        }
    }
        
    public void CheckAchievement()
    {
        // Achievement new adventure start
//        if (PlayerPrefs.HasKey("2"))
//        {
//            GPGSManager.Instance.Achievement_New_Adventure();
//        }
//
//        // Achievement landing kategori
//        if (PlayerPrefs.HasKey("10"))
//        {
//            GPGSManager.Instance.Achievement_Landing_Time();
//        }
//
//        // Achievement awan awangan punya kategori
//        if (PlayerPrefs.HasKey("20"))
//        {
//            GPGSManager.Instance.Achievement_Awan_Awangan();
//        }
    }
        


        

}
