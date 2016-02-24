using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MulaOption : MonoBehaviour {


	public int sceneToStart = 1;										//Index number in build settings of scene to load if changeScenes is true
	public bool changeScenes;											//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart;										//Choose whether to continue playing menu music or start a new music clip


	public bool inMainMenu = true;					//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	public Animator animColorFade; 					//Reference to animator which will fade to and from black when starting game.
	//public Animator animMenuAlpha;				//Reference to animator that will fade out alpha of MenuPanel canvas group
	public AnimationClip fadeColorAnimationClip;	//Animation clip fading to color (black default) when changing scenes
	//public AnimationClip fadeAlphaAnimationClip;	//Animation clip fading out UI elements alpha


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartButtonClicked()
	{		
			//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke ("LoadDelayed", fadeColorAnimationClip.length * .1f);

			//Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
			animColorFade.SetTrigger ("fade");
				
	}


}
