using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP_Manager : IStoreListener
{

    public readonly string Removd_ADS = "remove_ads";
    public readonly string Steel_1000 = "steel_1000";

    private IStoreController Store_Controller; // 구매 과정을 제어하는 함수 제공자
    private IExtensionProvider Store_Extension_Provider; // 여러 플랫폼을 위한 확정 처리 제공자

    public void Init_Unity_IAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance()); // IAP 작업 초기화

        builder.AddProduct(Steel_1000, ProductType.Consumable, new IDs() { { Steel_1000, GooglePlay.Name } });
        builder.AddProduct(Removd_ADS, ProductType.NonConsumable, new IDs() { { Removd_ADS, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP 초기화에 성공하였습니다.");

        Store_Controller = controller;
        Store_Extension_Provider = extensions;
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"IAP 초기화에 실패하였습니다. : {message}");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"IAP 초기화에 실패하였습니다. : {error.ToString()}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log(purchaseEvent.purchasedProduct.transactionID); // 고유한 영수증 ID
        Debug.Log(purchaseEvent.purchasedProduct.definition.id); // 구매한 아이템 ID

        _ = Base_Manager.BACKEND.WriteData();

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"구매에 실패하였습니다. : {failureReason.ToString()} ");

    }

    /// <summary>
    /// 구매 과정 진행
    /// </summary>
    /// <param name="_productID"></param>
    /// <returns></returns>
    public void Purchase(string _productID)
    {
        Product product = Store_Controller.products.WithID(_productID);

        if(product != null && product.availableToPurchase)
        {
            Store_Controller.InitiatePurchase(product);
        }

        else
        {
            Debug.Log($"상품이 없거나, 현재 구매가 불가능합니다.");
        }
    }

}
