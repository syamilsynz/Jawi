using UnityEngine;
using System.Collections;
using SVGImporter;

public class FlashCard : MonoBehaviour 
{

    public SVGAsset[] svgAsset;
    public AudioClip[] audioAsset;
    public SVGImage svgimage;

    private AudioSource musicSource;

    private bool isPlaying;
    private int svgIndex = 0;

    public float offsetX;
    public float offsetY;
    public float moveOffsetX;
    public float moveSpeed = 2f;


    // Use this for initialization
    void Start () 
    {
        svgimage.vectorGraphics = svgAsset[svgIndex];

        for (int i = 0; i < svgAsset.Length; i++)
        {
            GameObject clone;
//            clone = Instantiate(player, new Vector2(transform.position, transform.rotation) as GameObject;

            clone = Instantiate(player, new Vector2(transform.position.x + (offsetX * i), transform.position.y), transform.rotation) as GameObject;
            clone.transform.parent = this.gameObject.transform;    
            clone.GetComponent<SVGImage>().vectorGraphics = svgAsset[i];
            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
//            Example(this.gameObject);
        }

        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
//            transform.GetChild(i).GetComponent<RectTransform>().position.x = transform.GetChild(i).GetComponent<RectTransform>().position.x + (100f * i);
            print("Child : " + i + " = x = " + transform.GetChild(i).GetComponent<RectTransform>().position.x);
        }

        //Get a component reference to the AudioSource attached to the UI game object
        musicSource = GetComponent<AudioSource> ();

    }

    // Update is called once per frame
    void Update () 
    {

    }

    public void NextFlashCard()
    {
        if (svgIndex < svgAsset.Length - 1)
        {
           
            if (!isPlaying)
            {
                svgIndex++;
                svgimage.vectorGraphics = svgAsset[svgIndex];

                MovingRight2();
            }
        }
            
    }

    public void PreviousFlashCard()
    {
        if (svgIndex > 0)
        {
           
            if (!isPlaying)
            {
                svgIndex--;
                svgimage.vectorGraphics = svgAsset[svgIndex];

                MovingLeft2();
            }
        }

    }

    public GameObject player;

    //Invoked when a button is pressed.
    public void Example(GameObject newParent)
    {
        //Makes the GameObject "newParent" the parent of the GameObject "player".
        player.transform.parent = newParent.transform;

        //Display the parent's name in the console.
        Debug.Log ("Player's Parent: " + player.transform.parent.name);

        // Check if the new parent has a parent GameObject.
        if(newParent.transform.parent != null)
        {
            //Display the name of the grand parent of the player.
            Debug.Log ("Player's Grand parent: " + player.transform.parent.parent.name);
        }
    }

    public void MovingRight2()
    {
        StartCoroutine(MovingRight());
    }

    public void MovingLeft2()
    {
        StartCoroutine(MovingLeft());
    }

    IEnumerator MovingRight()
    {
        isPlaying = true;

        Vector3 startPosition = this.transform.GetComponent<RectTransform>().localPosition;
        Vector3 endPosition;
        endPosition = this.transform.GetComponent<RectTransform>().localPosition;
        endPosition.x = endPosition.x - moveOffsetX;
       
        float t = 0;
        // animate letter
        while (t < 1f) 
        {
            t += Time.fixedDeltaTime * moveSpeed;
            this.transform.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        musicSource.clip = audioAsset[svgIndex];
        musicSource.Play ();

        isPlaying = false;

    }

    IEnumerator MovingLeft()
    {

        isPlaying = true;

        Vector3 startPosition = this.transform.GetComponent<RectTransform>().localPosition;
        Vector3 endPosition;
        endPosition = this.transform.GetComponent<RectTransform>().localPosition;
        endPosition.x = endPosition.x + moveOffsetX;

        float t = 0;
        // animate letter
        while (t < 1f) 
        {
            t += Time.smoothDeltaTime * moveSpeed;
            this.transform.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        musicSource.clip = audioAsset[svgIndex];
        musicSource.Play ();

        isPlaying = false;
    }

    public void PlayAudio()
    {
        if (!isPlaying)
        {
            musicSource.clip = audioAsset[svgIndex];
            musicSource.Play ();
        }
    }

}
