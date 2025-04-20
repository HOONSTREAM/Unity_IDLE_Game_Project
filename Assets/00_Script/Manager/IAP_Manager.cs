using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP_Manager : IStoreListener
{

    public readonly string Removd_ADS = "remove_ads";
    public readonly string Steel_1000 = "steel_1000";
    public readonly string Dia_19000 = "dia_19000";
    public readonly string Dia_1400 = "dia_1400";
    public readonly string Dia_4900 = "dia_4900";
    public readonly string Dungeon_Dia = "dungeon_dia_20";
    public readonly string Dungeon_Gold = "gold_30";

    private IStoreController Store_Controller; // ���� ������ �����ϴ� �Լ� ������
    private IExtensionProvider Store_Extension_Provider; // ���� �÷����� ���� Ȯ�� ó�� ������

    public void Init_Unity_IAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance()); // IAP �۾� �ʱ�ȭ

        builder.AddProduct(Steel_1000, ProductType.Consumable, new IDs() { { Steel_1000, GooglePlay.Name } });
        builder.AddProduct(Removd_ADS, ProductType.NonConsumable, new IDs() { { Removd_ADS, GooglePlay.Name } });
        builder.AddProduct(Dia_19000, ProductType.Consumable, new IDs() { { Dia_19000, GooglePlay.Name } });
        builder.AddProduct(Dia_1400, ProductType.Consumable, new IDs() { { Dia_1400, GooglePlay.Name } });
        builder.AddProduct(Dia_4900, ProductType.Consumable, new IDs() { { Dia_4900, GooglePlay.Name } });
        builder.AddProduct(Dungeon_Dia, ProductType.Consumable, new IDs() { { Dungeon_Dia, GooglePlay.Name } });
        builder.AddProduct(Dungeon_Gold, ProductType.Consumable, new IDs() { { Dungeon_Gold, GooglePlay.Name } });


        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP �ʱ�ȭ�� �����Ͽ����ϴ�.");

        Store_Controller = controller;
        Store_Extension_Provider = extensions;
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"IAP �ʱ�ȭ�� �����Ͽ����ϴ�. : {message}");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"IAP �ʱ�ȭ�� �����Ͽ����ϴ�. : {error.ToString()}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log(purchaseEvent.purchasedProduct.transactionID); // ������ ������ ID
        Debug.Log(purchaseEvent.purchasedProduct.definition.id); // ������ ������ ID

        string purchase_EventName = purchaseEvent.purchasedProduct.definition.id;

        //Enum.Parse�� Type������ �ʿ���Ѵ�. typeof(IAP_Holder),�� �Ľ��ҰŴ�. �� �˷������.
        //IAP_Holder : Ÿ�� �̸� �� ��ü(���� ����, ����ȯ, �� �� � ���)
        //typeof(IAP_Holder) : ���÷���, enum �Ľ�, Ÿ�Ժ� � ���

        IAP_Holder holder = (IAP_Holder)Enum.Parse(typeof(IAP_Holder), purchase_EventName);

        Base_Canvas.instance.Get_UI("UI_Reward");
        Utils.UI_Holder.Peek().GetComponent<UI_Reward>().GetIAPReward(holder);

        _ = Base_Manager.BACKEND.WriteData();

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"���ſ� �����Ͽ����ϴ�. : {failureReason.ToString()} ");

    }

    /// <summary>
    /// ���� ���� ����
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
            Debug.Log($"��ǰ�� ���ų�, ���� ���Ű� �Ұ����մϴ�.");
        }
    }

}
