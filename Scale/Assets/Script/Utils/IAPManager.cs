using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAPManager : MonoSingleton<IAPManager>, IStoreListener
{
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.


#if UNITY_ANDROID
	private static string PRODUCT_1000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_0";
	private static string PRODUCT_3500_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_1";
	private static string PRODUCT_7000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_2";
	private static string PRODUCT_10000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_3";
	private static string PRODUCT_15000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_4";
	private static string PRODUCT_20000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_5";
	private static string PRODUCT_30000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_6";
	private static string PRODUCT_40000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_7";
	private static string PRODUCT_50000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_8";
	private static string PRODUCT_70000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_9";
	private static string PRODUCT_150000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_10";
	private static string PRODUCT_NOADS = "com.topfreepuzzle.tapshrink.noads";
#elif UNITY_IOS
	private static string PRODUCT_1000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_0";
	private static string PRODUCT_3500_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_1";
	private static string PRODUCT_7000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_2";
	private static string PRODUCT_10000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_3";
	private static string PRODUCT_15000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_4";
	private static string PRODUCT_20000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_5";
	private static string PRODUCT_30000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_6";
	private static string PRODUCT_40000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_7";
	private static string PRODUCT_50000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_8";
	private static string PRODUCT_70000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_9";
	private static string PRODUCT_150000_DIAMOND = "com.topfreepuzzle.tapshrink.diamond_10";
	private static string PRODUCT_NOADS = "com.topfreepuzzle.tapshrink.noads";
#else
	private static string PRODUCT_1000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_0";
	private static string PRODUCT_3500_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_1";
	private static string PRODUCT_7000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_2";
	private static string PRODUCT_10000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_3";
	private static string PRODUCT_15000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_4";
	private static string PRODUCT_20000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_5";
	private static string PRODUCT_30000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_6";
	private static string PRODUCT_40000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_7";
	private static string PRODUCT_50000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_8";
	private static string PRODUCT_70000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_9";
	private static string PRODUCT_150000_DIAMOND = "com.quoclv.bestgame.scalecolor.diamond_10";
	private static string PRODUCT_NOADS = "com.quoclv.bestgame.scalecolor.noads";
#endif

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


		builder.AddProduct(PRODUCT_1000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_3500_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_7000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_10000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_15000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_20000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_30000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_40000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_50000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_70000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_150000_DIAMOND, ProductType.Consumable);
		builder.AddProduct(PRODUCT_NOADS, ProductType.NonConsumable);

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
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.RESTORE_PURCHASE, 1);
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
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.RESTORE_PURCHASE, 0);
		}
		// Otherwise ...
		else
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.RESTORE_PURCHASE, 2);
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	/* ------------------------------------------ BUY HERE ------------------------------------------ */

	public void Buy1000Diamonds()
	{
		this.BuyProductID(PRODUCT_1000_DIAMOND);
	}

	public void Buy3500Diamonds()
	{
		this.BuyProductID(PRODUCT_3500_DIAMOND);
	}

	public void Buy7000Diamonds()
	{
		this.BuyProductID(PRODUCT_7000_DIAMOND);
	}

	public void Buy10000Diamonds()
	{
		this.BuyProductID(PRODUCT_10000_DIAMOND);
	}

	public void Buy15000Diamonds()
	{
		this.BuyProductID(PRODUCT_15000_DIAMOND);
	}

	public void Buy20000Diamonds()
	{
		this.BuyProductID(PRODUCT_20000_DIAMOND);
	}

	public void Buy30000Diamonds()
	{
		this.BuyProductID(PRODUCT_30000_DIAMOND);
	}

	public void Buy40000Diamonds()
	{
		this.BuyProductID(PRODUCT_40000_DIAMOND);
	}

	public void Buy50000Diamonds()
	{
		this.BuyProductID(PRODUCT_50000_DIAMOND);
	}

	public void Buy70000Diamonds()
	{
		this.BuyProductID(PRODUCT_70000_DIAMOND);
	}

	public void Buy150000Diamonds()
	{
		this.BuyProductID(PRODUCT_150000_DIAMOND);
	}

	public void BuyNoAds()
	{
		this.BuyProductID(PRODUCT_NOADS);
	}


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
		if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_1000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(1000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_3500_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(3500);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_7000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(7000);
		}
		if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_10000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(10000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_15000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(15000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_20000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(20000);
		}
		if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_30000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(30000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_40000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(40000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_50000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(50000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_70000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(70000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_150000_DIAMOND, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.AddDiamond(150000);
		}
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_NOADS, StringComparison.Ordinal))
		{
			NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_SUCCESS);
			UserProfile.Instance.RemoveAds();
		}
		else
		{
			//NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_FAIL);
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}

		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_FAIL);
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}
