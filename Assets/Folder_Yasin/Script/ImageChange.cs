using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageChange : MonoBehaviour {

	public GameObject imageObject;
	public Sprite image1st;
	public Sprite image2nd;
	public float duration;
	public bool ignoreTimeScale;
	public bool useAlpha;

	// Use this for initialization
	void Start () {
		StartCoroutine (FadeToBlack ());
	}


	IEnumerator FadeToBlack ()
	{
		
		yield return new WaitForSeconds(duration);

		imageObject.GetComponent<Image> ().sprite = image1st;
		imageObject.GetComponent<Image> ().CrossFadeColor (Color.black, duration, ignoreTimeScale, useAlpha);

		yield return new WaitForSeconds(duration);

		imageObject.GetComponent<Image> ().sprite = image2nd;
		imageObject.GetComponent<Image> ().CrossFadeColor (Color.white, duration, ignoreTimeScale, useAlpha);
	}

}