using UnityEngine;
using System.Collections;

public class InitUM : MonoBehaviour 
{

    void Awake()
    {
        Debug.Log("Enter " + this.name);

        //------- Game Services -----------
        UM_GameServiceManager.Instance.Connect();

        Debug.Log("Connecting To Game Service");

        UM_GameServiceManager.OnPlayerConnected += OnPlayerConnected;
        UM_GameServiceManager.OnPlayerDisconnected += OnPlayerDisconnected;


        if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) 
        {
            OnPlayerConnected();

        }

        // ------- IAB ---------
        UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction += OnPurchaseFlowFinishedAction;
        UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnConnectFinished;
    }

    // Use this for initialization
    void Start () 
    {

        // IAB - subscribign on intit fisigh action
        UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnBillingConnectFinishedAction;
        UM_InAppPurchaseManager.instance.Init();

        // IAB - Set text price based on localized price
        if(UM_InAppPurchaseManager.instance.IsInited)
        {
            //            buttonBuyCoin500000.transform.FindChild("Text").GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(COIN_500000_PRODUCT_ID).LocalizedPrice.ToString();
            //            buttonBuyNagaIAP.transform.FindChild("Text").GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(NAGAGAMI_PRODUCT_ID).LocalizedPrice.ToString();
            Debug.Log("Localize price");
        }
        else
        {
            //            buttonBuyCoin500000.transform.FindChild("Text").gameObject.GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(COIN_500000_PRODUCT_ID).ActualPrice.ToString();
            //            buttonBuyNagaIAP.transform.FindChild("Text").gameObject.GetComponent<Text>().text = UM_InAppPurchaseManager.Instance.GetProductById(NAGAGAMI_PRODUCT_ID).ActualPrice.ToString();
            Debug.Log("Unlocalize price");

        }

        // Load Main Scene
        //StartCoroutine(GoToScene(1));


    }

    // Update is called once per frame
    void Update () 
    {

    }

    IEnumerator GoToScene(int index)
    {
        yield return new WaitForSeconds(3f);

        Application.LoadLevel(index);
    }

    // ------- Game Service -------------
    private void OnPlayerConnected() 
    {
        Debug.Log( "Player Connected to Game Services");
    }


    private void OnPlayerDisconnected() 
    {
        Debug.Log("Player Disconnected to Game Services");
    }

    // --------- IAB -----------
    private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) {
        UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction -= OnPurchaseFlowFinishedAction;
        if(result.isSuccess) {
            Debug.Log( "Product " + result.product.id + " purchase Success");
        } else  {
            Debug.Log( "Product " + result.product.id + " purchase Failed");
        }
    }

    private void OnBillingConnectFinishedAction (UM_BillingConnectionResult result) {
        UM_InAppPurchaseManager.OnBillingConnectFinishedAction -= OnBillingConnectFinishedAction;
        if(result.isSuccess) {
            Debug.Log("OnBillingConnectFinishedAction - Connected");
        } else {
            Debug.Log("OnBillingConnectFinishedAction - Failed to connect");
        }
    }

    private void OnConnectFinished(UM_BillingConnectionResult result) {

        if(result.isSuccess) {
            print ( "Billing init Success");
        } else  {
            print("Billing init Failed");
        }
    }

}
