﻿using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Application.Quit();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}
