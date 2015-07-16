using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarsInApp : MonoBehaviour 
{
    public string[] Skus;

    #if UNITY_ANDROID

    void OnEnable()
    {
        GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
        GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
        GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
        GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
    }
    
    
    void OnDisable()
    {
        GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
        GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
        GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
    }

    void Awake()
    {
        foreach (string s in Skus)
            if (!PlayerPrefs.HasKey(s))
                PlayerPrefs.SetInt(s, 0);

        var key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkT3LCv0FCw3ly9GaePxmSyWgEPMZB3MakSl3ID3M/oVXPb9+HyEc1HUI9T2gPvIV72iJ08PaKjpcOmri0VkoJerVF/Ak/1Hz7p2nfZ16rVbQpzPxQg1LiT834tSQOyhZ1gvM1k6sXRIllTNrpp5ZlvoCMBvkuD62Y74YzK5GX6rcp/ft8pIAVTo1Qqz17a32/eC52iyqN90dShG0kLQBWiix+nWici+CQmciJtM4bwxyDTTgq+cmJUfeFCv2zUPKR/f3UWsPkMduvoft9HW6na8hqusuJh1LODwIlZmd+ysdykw0E7XGCJGBlT9tj/XAicmtWcQMqQMVAyVHc02mJwIDAQAB";
        GoogleIAB.init( key );
    }
    
    private void QueryInventory()
    {
        GoogleIAB.queryInventory( Skus );
    }
    
    void billingSupportedEvent()
    {
        QueryInventory();
    }
    
    
    void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
    {
        foreach (var data in purchases)
        {
            PlayerPrefs.SetInt(data.productId,1);
        }
        EventController.PostEvent("update.inapp", null);
    }
    
    void purchaseSucceededEvent( GooglePurchase purchase )
    {
        QueryInventory();
    }
    
    
    void purchaseFailedEvent( string error )
    {
        QueryInventory();
    }
    
    
    void consumePurchaseSucceededEvent( GooglePurchase purchase )
    {
        QueryInventory();
    }
    
    #endif
}
