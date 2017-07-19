using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iAPDialog : BaseDialog {

	public delegate void Transaction(); 

	public void Buy1000Diamonds()
	{
		//NotifyDialog notify = GUIManager.Instance.OnShowNotiFyDialog("Notify", NotifyType.TRANSACTION_CONFIRM,
		//	(Transaction)IAPManager.Instance.Buy1000Diamonds);
		IAPManager.Instance.Buy1000Diamonds();
	}

	public void Buy3500Diamonds()
	{
		IAPManager.Instance.Buy3500Diamonds();
	}

	public void Buy7000Diamonds()
	{
		IAPManager.Instance.Buy7000Diamonds();
	}

	public void Buy10000Diamonds()
	{
		IAPManager.Instance.Buy10000Diamonds();
	}

	public void Buy15000Diamonds()
	{
		IAPManager.Instance.Buy15000Diamonds();
	}

	public void Buy20000Diamonds()
	{
		IAPManager.Instance.Buy20000Diamonds();
	}

	public void Buy30000Diamonds()
	{
		IAPManager.Instance.Buy30000Diamonds();
	}

	public void Buy40000Diamonds()
	{
		IAPManager.Instance.Buy40000Diamonds();
	}

	public void Buy50000Diamonds()
	{
		IAPManager.Instance.Buy50000Diamonds();
	}

	public void Buy70000Diamonds()
	{
		IAPManager.Instance.Buy70000Diamonds();
	}

	public void Buy150000Diamonds()
	{
		IAPManager.Instance.Buy150000Diamonds();
	}

	public void RestorePurchase()
	{
		IAPManager.Instance.RestorePurchases();
	}
}
