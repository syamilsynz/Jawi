using UnityEngine;
using System.Collections;
using SVGImporter;
using UnityEngine.UI;
using System.Linq;

public class FlashCard : MonoBehaviour 
{

    [System.Serializable]
    public class FlashCardTemplate
    {
        public SVGAsset svgFlashImage;      // Flash Card Image
        public SVGAsset svgFlashName;       // Name of Flash Card using image
        public AudioClip audioAsset;        // Audio of flash card

        public bool flashNameIsTextBased;   // text or svg image

        public Material templateColor;      // Template Color
     
        [HideInInspector] public bool unlock;
        public int price;                  // Coin price - 0 is default (free)

        // Unlock Condition
        public bool isFree;                 // Free of cas
        public bool isCoinDependency;       // Need coin to unlock
        public bool isIAP;                  // Need to buy in app purchase
        public bool isWatchRewardedAd;      // Need to watch rewarded ad


    }



    [Space(5)] [Header("Flash Card Details")] [Space(5)] 
    public FlashCardTemplate[] flashcardTemplate;       // Set of question/answer for animal

    public SVGImage svgimage;

    private AudioSource musicSource;

    private bool isPlaying;
    private int svgIndex = 0;

    public float offsetX;
    public float offsetY;
    public float moveOffsetX;
    public float moveSpeed = 2f;

    private string flashCardName;

    public Button btnWatchAds;
    public Button btnCoin;
    public Button btnIAP;


    static FlashCard instance;
    public static FlashCard Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        for (int i = 0; i < flashcardTemplate.Length; i++)
        {
            GameObject clone;

            clone = Instantiate(player, new Vector2(transform.position.x + (offsetX * i), transform.position.y +  offsetY), transform.rotation) as GameObject;
            clone.transform.parent = this.gameObject.transform;    
            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            clone.transform.FindChild("flashcardImage").GetComponent<SVGImage>().vectorGraphics = flashcardTemplate[i].svgFlashImage;

            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            // Name of flash card
            if (flashcardTemplate[i].flashNameIsTextBased)
            {
                clone.transform.FindChild("TextFlashName").GetComponent<Text>().text = flashcardTemplate[i].svgFlashImage.name.ToUpper();
            }
            else
            {
                clone.transform.FindChild("SvgFlashName").GetComponent<SVGImage>().vectorGraphics = flashcardTemplate[i].svgFlashName;
            }
                
            // Set Color
            clone.transform.FindChild("TemplateCard").GetComponent<SVGImage>().material = flashcardTemplate[i].templateColor;

            // Lock or Unlock
            if (flashcardTemplate[i].unlock == true)
            {
                clone.transform.FindChild("flashcardImage").GetComponent<SVGImage>().color = new Color32(255, 255, 255, 255);
        
            }
            else
            {
//                clone.transform.FindChild("flashcardImage").GetComponent<SVGImage>().color = new Color32(100, 100, 1001, 255);
                clone.transform.FindChild("flashcardImage").GetComponent<SVGImage>().color = new Color32(0, 0, 0, 255);
            }
        }

        //Get a component reference to the AudioSource attached to the UI game object
        musicSource = GetComponent<AudioSource> ();

//        flashCardName = flashcardTemplate[svgIndex].svgFlashImage.name;
        CheckFeatureUnlock();

    }

    // Update is called once per frame
    void Update ()
    {
//        if (flashcardTemplate[svgIndex].unlock)
//        {
//            this.transform.GetChild(2).transform.FindChild("flashcardImage").GetComponent<SVGImage>().color = new Color32(255, 255, 255, 255);   
//        }
//        else
//        {
//            this.transform.GetChild(2).transform.FindChild("flashcardImage").GetComponent<SVGImage>().color = new Color32(0, 0, 0, 255);
//        }
    }

    public void NextFlashCard()
    {
        if (svgIndex < flashcardTemplate.Length - 1)
        {
           
            if (!isPlaying)
            {
                svgIndex++;
                svgimage.vectorGraphics = flashcardTemplate[svgIndex].svgFlashImage;

//                flashCardName = flashcardTemplate[svgIndex].svgFlashImage.name;

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
                svgimage.vectorGraphics = flashcardTemplate[svgIndex].svgFlashImage;

//                flashCardName = flashcardTemplate[svgIndex].svgFlashImage.name;

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
        CheckFeatureUnlock();

        isPlaying = true;

        Vector3 startPosition = this.transform.GetComponent<RectTransform>().localPosition;
        Vector3 endPosition;
        endPosition = this.transform.GetComponent<RectTransform>().localPosition;
        endPosition.x = endPosition.x - moveOffsetX;
       
        float t = 0;
        // animate letter
        while (t < 1f) 
        {
            t += Time.smoothDeltaTime * moveSpeed;
            this.transform.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        musicSource.clip = flashcardTemplate[svgIndex].audioAsset;
        musicSource.Play ();    

        isPlaying = false;

    }

    IEnumerator MovingLeft()
    {
        CheckFeatureUnlock();

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

        musicSource.clip = flashcardTemplate[svgIndex].audioAsset;
        musicSource.Play ();

     

        isPlaying = false;
    }

    public void PlayAudio()
    {
        if (!isPlaying)
        {
            musicSource.clip = flashcardTemplate[svgIndex].audioAsset;
            musicSource.Play ();
        }
    }

    // ---- Unlock Flash Card -----

    public void CheckFeatureUnlock()
    {
        flashCardName = flashcardTemplate[svgIndex].svgFlashImage.name;

        if (PlayerPrefs.HasKey(flashCardName))
        {
            flashcardTemplate[svgIndex].unlock = true;

            btnCoin.gameObject.SetActive(false);
            btnWatchAds.gameObject.SetActive(false);
            btnIAP.gameObject.SetActive(false);
        }
        else
        {
            // Do something about the condition to unlock flash card

            if (flashcardTemplate[svgIndex].isFree)
            {
                flashcardTemplate[svgIndex].unlock = true;
                // Unlock flash card
//                PlayerPrefs.SetString(flashCardName, flashCardName);
                btnCoin.gameObject.SetActive(false);
                btnWatchAds.gameObject.SetActive(false);
                btnIAP.gameObject.SetActive(false);
            }
            else if (flashcardTemplate[svgIndex].isCoinDependency)
            {
                // Open related to coin
                flashcardTemplate[svgIndex].unlock = false;

                btnCoin.gameObject.SetActive(true);
                btnWatchAds.gameObject.SetActive(false);
                btnIAP.gameObject.SetActive(false);
            }
            else if (flashcardTemplate[svgIndex].isWatchRewardedAd)
            {
                // Open related to ads
                flashcardTemplate[svgIndex].unlock = false;

                btnCoin.gameObject.SetActive(false);
                btnWatchAds.gameObject.SetActive(true);
                btnIAP.gameObject.SetActive(false);
            }
            else if (flashcardTemplate[svgIndex].isIAP)
            {
                // Open related to in app purchase
                flashcardTemplate[svgIndex].unlock = false;

                btnCoin.gameObject.SetActive(false);
                btnWatchAds.gameObject.SetActive(false);
                btnIAP.gameObject.SetActive(true);
            }
        }

        if (flashcardTemplate[svgIndex].unlock)
        {
            this.transform.GetChild(svgIndex).transform.FindChild("flashcardImage").GetComponent<SVGImage>().color = new Color32(255, 255, 255, 255);   
        }
        else
        {
            this.transform.GetChild(svgIndex).transform.FindChild("flashcardImage").GetComponent<SVGImage>().color = new Color32(0, 0, 0, 255);
        }
    }

    public void UnlockFlashCard()
    {
        // If already unlock
        if (PlayerPrefs.HasKey(flashCardName))
        {
            flashcardTemplate[svgIndex].unlock = true;

            // Hide all button related to buying
        }
        else
        {
            
        }
    }

    public void UnlockFlashCardUsingIAP()
    {
        if (PlayerPrefs.HasKey(flashCardName))
        {
            flashcardTemplate[svgIndex].unlock = true;

            // Hide all button related to buying
        }
        else
        {
            // Unlock this feature if payment successfull

            IABManager.Instance.BuyNonConsumableProduct(flashCardName);


        }
    }

    public void UnlockFlashCardUsingCoin()
    {
        if (PlayerPrefs.HasKey(flashCardName))
        {
            flashcardTemplate[svgIndex].unlock = true;
          
            // Hide all button related to buying
            CheckFeatureUnlock();
        }
        else
        {
            // If need coin/price/iap to unlock
            if (flashcardTemplate[svgIndex].isCoinDependency == true)
            {
                if (SaveManager.coinAmount >= flashcardTemplate[svgIndex].price)
                {
                    // Unlock flash card
                    PlayerPrefs.SetString(flashCardName, flashCardName);

                    SaveManager.coinAmount = SaveManager.coinAmount - flashcardTemplate[svgIndex].price;
                    SaveManager.SaveData();

                    Debug.Log("feature " + flashCardName + " unlocked!");

                    CheckFeatureUnlock();

                }
                else
                {
                    // Don't have enough coin
                    // pop up coin panel
                    Debug.Log("Not enough coin " + SaveManager.coinAmount );
                    UM_NotificationController.instance.ShowNotificationPoup("Notification", "Not enough coin " + SaveManager.coinAmount);
                }
            }



        }
    }

    public void UnlockFlashCardUsingRewardedAd()
    {
        if (PlayerPrefs.HasKey(flashCardName))
        {
            flashcardTemplate[svgIndex].unlock = true;
            CheckFeatureUnlock();
            // Hide all button related to buying
        }
        else
        {

            UnityAdsManager.Instance.ShowFlashCardRewardedAd(flashCardName);

        }
    }



}
