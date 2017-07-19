using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAPManager : MonoSingleton<IAPManager>, IStoreListener
{
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	//private static string PRODUCT_30_DIAMOND = "com.quoclv.bestgame.jexy.diamond_0";
	//private static string PRODUCT_60_DIAMOND = "com.quoclv.bestgame.jexy.diamond_1";
	//private static string PRODUCT_120_DIAMOND = "com.quoclv.bestgame.jexy.diamond_2";
	//private static string PRODUCT_240_DIAMOND = "com.quoclv.bestgame.jexy.diamond_3";
	//private static string PRODUCT_360_DIAMOND = "com.quoclv.bestgame.jexy.diamond_4";
	//private static string PRODUCT_480_DIAMOND = "com.quoclv.bestgame.jexy.diamond_5";
	//private static string PRODUCT_600_DIAMOND = "com.quoclv.bestgame.jexy.diamond_6";
	//private static string PRODUCT_720_DIAMOND = "com.quoclv.bestgame.jexy.diamond_7";
	//private static string PRODUCT_840_DIAMOND = "com.quoclv.bestgame.jexy.diamond_8";
	//private static string PRODUCT_960_DIAMOND = "com.quoclv.bestgame.jexy.diamond_9";
	//private static string PRODUCT_1080_DIAMOND = "com.quoclv.bestgame.jexy.diamond_10";
	//private static string PRODUCT_2000_DIAMOND = "com.quoclv.bestgame.jexy.diamond_full";
	//private static string PRODUCT_NOADS = "com.quoclv.bestgame.jexy.noads";

	void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing()
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


		//builder.AddProduct(PRODUCT_30_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_60_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_120_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_240_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_360_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_480_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_600_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_720_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_840_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_960_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_1080_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_2000_DIAMOND, ProductType.Consumable);
		//builder.AddProduct(PRODUCT_NOADS, ProductType.NonConsumable);

		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize(this, builder);
	}

	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	void BuyProductID(string productId)
	{
		// If Purchasing has been initialized ...
		if (IsInitialized())
		{
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else
			{
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	/* ------------------------------------------ BUY HERE ------------------------------------------ */

	//public void Buy30Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_30_DIAMOND);
	//}

	//public void Buy60Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_60_DIAMOND);
	//}

	//public void Buy120Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_120_DIAMOND);
	//}

	//public void Buy240Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_240_DIAMOND);
	//}

	//public void Buy360Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_360_DIAMOND);
	//}

	//public void Buy480Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_480_DIAMOND);
	//}

	//public void Buy600Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_600_DIAMOND);
	//}

	//public void Buy720Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_720_DIAMOND);
	//}

	//public void Buy840Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_840_DIAMOND);
	//}

	//public void Buy960Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_960_DIAMOND);
	//}

	//public void Buy1080Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_1080_DIAMOND);
	//}

	//public void Buy2000Diamonds()
	//{
	//	this.BuyProductID(PRODUCT_2000_DIAMOND);
	//}

	//public void BuyNoAds()
	//{
	//	this.BuyProductID(PRODUCT_NOADS);
	//}


	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		//if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_30_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(30, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_60_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(60, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_120_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(120, true);
		//}
		//if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_240_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(240, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_360_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(360, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_480_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(480, true);
		//}
		//if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_600_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(600, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_720_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(720, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_840_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(840, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_960_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(960, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_1080_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(1080, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_2000_DIAMOND, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.AddDiamond(2000, true);
		//}
		//else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_NOADS, StringComparison.Ordinal))
		//{
		//	UserProfile.Instance.RemoveAds();
		//	Analytics.Instance.a.LogEvent(new EventHitBuilder().SetEventCategory("Buy").SetEventAction("Buy Remove Ads"));
		//}
		//else
		//{
		//	Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		//}

		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}
