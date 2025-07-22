using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class IAP_Manager : IStoreListener
{
    private Action onPurchaseSuccessCallback;

    public readonly string Removd_ADS = "remove_ads";
    public readonly string Steel_1000 = "steel_1000";
    public readonly string Dia_19000 = "dia_19000";
    public readonly string Dia_1400 = "dia_1400";
    public readonly string Dia_4900 = "dia_4900";
    public readonly string Dungeon_Dia = "dungeon_dia_20";
    public readonly string Dungeon_Gold = "gold_30";
    public readonly string Today_Package_01 = "package_1";
    public readonly string Strong_Package_02 = "package_2";
    public readonly string Today_Package_03 = "package_3";
    public readonly string Dia_68000 = "dia_68000";
    public readonly string Start_Package = "start";
    public readonly string Hondon_2000 = "hondon_2000";
    public readonly string Dia_Gacha = "dia_gacha";
    public readonly string Enhancement = "enhancement";
    public readonly string DEF_Enhancement = "def_enhancement";
    public readonly string DIA_PASS = "dia_pass";
    public readonly string Enhancement_Package = "enhancement_package";
    public readonly string RESEARCH_TICKET = "ticket";
    public readonly string RESEARCH_BOOK = "research_book";
    public readonly string RESEARCH_PACKAGE = "research_package";

    private IStoreController Store_Controller; // 구매 과정을 제어하는 함수 제공자
    private IExtensionProvider Store_Extension_Provider; // 여러 플랫폼을 위한 확정 처리 제공자

    public void Init_Unity_IAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance()); // IAP 작업 초기화

        builder.AddProduct(Steel_1000, ProductType.Consumable, new IDs() { { Steel_1000, GooglePlay.Name } });
        builder.AddProduct(Removd_ADS, ProductType.NonConsumable, new IDs() { { Removd_ADS, GooglePlay.Name } });
        builder.AddProduct(Dia_19000, ProductType.Consumable, new IDs() { { Dia_19000, GooglePlay.Name } });
        builder.AddProduct(Dia_1400, ProductType.Consumable, new IDs() { { Dia_1400, GooglePlay.Name } });
        builder.AddProduct(Dia_4900, ProductType.Consumable, new IDs() { { Dia_4900, GooglePlay.Name } });
        builder.AddProduct(Dungeon_Dia, ProductType.Consumable, new IDs() { { Dungeon_Dia, GooglePlay.Name } });
        builder.AddProduct(Dungeon_Gold, ProductType.Consumable, new IDs() { { Dungeon_Gold, GooglePlay.Name } });
        builder.AddProduct(Today_Package_01, ProductType.Consumable, new IDs() { { Today_Package_01, GooglePlay.Name } });
        builder.AddProduct(Strong_Package_02, ProductType.Consumable, new IDs() { { Strong_Package_02, GooglePlay.Name } });
        builder.AddProduct(Dia_68000, ProductType.Consumable, new IDs() { { Dia_68000, GooglePlay.Name } });
        builder.AddProduct(Start_Package, ProductType.Consumable, new IDs() { { Start_Package, GooglePlay.Name } });
        builder.AddProduct(Today_Package_03, ProductType.Consumable, new IDs() { { Today_Package_03, GooglePlay.Name } });
        builder.AddProduct(Hondon_2000, ProductType.Consumable, new IDs() { { Hondon_2000, GooglePlay.Name } });
        builder.AddProduct(Dia_Gacha, ProductType.Consumable, new IDs() { { Dia_Gacha, GooglePlay.Name } });
        builder.AddProduct(Enhancement, ProductType.Consumable, new IDs() { { Enhancement, GooglePlay.Name } });
        builder.AddProduct(DEF_Enhancement, ProductType.Consumable, new IDs() { { DEF_Enhancement, GooglePlay.Name } });
        builder.AddProduct(DIA_PASS, ProductType.Consumable, new IDs() { { DIA_PASS, GooglePlay.Name } });
        builder.AddProduct(Enhancement_Package, ProductType.Consumable, new IDs() { { Enhancement_Package, GooglePlay.Name } });
        builder.AddProduct(RESEARCH_TICKET, ProductType.Consumable, new IDs() { { RESEARCH_TICKET, GooglePlay.Name } });
        builder.AddProduct(RESEARCH_BOOK, ProductType.Consumable, new IDs() { { RESEARCH_BOOK, GooglePlay.Name } });
        builder.AddProduct(RESEARCH_PACKAGE, ProductType.Consumable, new IDs() { { RESEARCH_PACKAGE, GooglePlay.Name } });

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

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productId = args.purchasedProduct.definition.id;
        string receipt_Json = args.purchasedProduct.receipt;

        decimal price = args.purchasedProduct.metadata.localizedPrice;
        string currency = args.purchasedProduct.metadata.isoCurrencyCode;

        // 영수증 검증
        decimal iapPrice = args.purchasedProduct.metadata.localizedPrice;
        string iapCurrency = args.purchasedProduct.metadata.isoCurrencyCode;

        BackendReturnObject validation = Backend.Receipt.IsValidateGooglePurchase(receipt_Json, "receiptDescription", iapPrice, iapCurrency);


        if (validation.IsSuccess())
        {

            Success_Purchase(args, productId);

        }

        else
        {
            Debug.LogError($"[결제 검증 실패] - {productId}, Error: {validation.GetMessage()}");
            Base_Manager.BACKEND.Log_Try_Crack_IAP(productId, validation.GetMessage());
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"결제 검증에 실패하였습니다 : {validation.GetMessage()}");
        }
       
        return PurchaseProcessingResult.Complete;
    }

    private void Success_Purchase(PurchaseEventArgs args, string productId)
    {      
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

        Debug.Log($"[결제 검증 성공] - {productId}");
        Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"결제 검증 성공");

        if (Enum.TryParse(productId, out IAP_Holder holder))
        {
            Base_Canvas.instance.Get_UI("UI_Reward");
            Utils.UI_Holder.Peek().GetComponent<UI_Reward>().GetIAPReward(holder);

            _ = Base_Manager.BACKEND.WriteData();

            onPurchaseSuccessCallback?.Invoke();
            onPurchaseSuccessCallback = null;
        }
        else
        {
            Debug.LogError($"[ERROR] IAP_Holder Enum에 없는 productId: {productId}");
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"구매에 실패하였습니다. : {failureReason.ToString()} ");

    }
    /// <summary>
    /// 뒤끝 콘솔에 영수증 검증을 진행하는 메서드 입니다.
    /// </summary>
    /// <param name="purchaseEvent"></param>
    private void GetCheckGoogleReceiptWithPrice(PurchaseEventArgs purchaseEvent)
    {
        string receiptToken = purchaseEvent.purchasedProduct.receipt;
        var bro = Backend.Receipt.IsValidateGooglePurchase(
                json: receiptToken,
                receiptDescription: "구매 했습니다",
                isSubscription: false,
                iapPrice: purchaseEvent.purchasedProduct.metadata.localizedPrice,
                iapCurrency: purchaseEvent.purchasedProduct.metadata.isoCurrencyCode);
    }

    /// <summary>
    /// 구매 과정 진행
    /// </summary>
    /// <param name="_productID"></param>
    /// <returns></returns>
    public void Purchase(string _productID, Action onSuccess = null)
    {
        Product product = Store_Controller.products.WithID(_productID);

        if(product != null && product.availableToPurchase)
        {
            onPurchaseSuccessCallback = onSuccess;
            Store_Controller.InitiatePurchase(product);
        }

        else
        {
            Debug.Log($"상품이 없거나, 현재 구매가 불가능합니다.");
        }
    }

}
