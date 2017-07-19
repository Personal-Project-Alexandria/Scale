using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemPair
{
	public int id;
	public Button button;
	public Toggle toggle;
	
	public ItemPair(int id, Button button, Toggle toggle)
	{
		this.id = id;
		this.button = button;
		this.toggle = toggle;
	}
}

public class StoreDialog : BaseDialog {

	public int price = 2000;
	public ToggleGroup group;
	public Text diamond;
	public List<GameObject> balls;
	private List<ItemPair> pairs = new List<ItemPair>();

	public override void OnShow(Transform transf, object data)
	{
		base.OnShow(transf, data);
		ExtractItemPair();
	}

	protected void Update()
	{
		diamond.text = UserProfile.Instance.GetDiamond().ToString();
	}

	public void OnClickIAP()
	{
		iAPDialog iap = GUIManager.Instance.OnShowDialog<iAPDialog>("iAP");
	}

	public void ExtractItemPair()
	{
		foreach (GameObject ball in balls)
		{
			pairs.Add(new ItemPair(pairs.Count, ball.GetComponentInChildren<Button>(), ball.GetComponentInChildren<Toggle>()));
		}

		List<bool> ballList = UserProfile.Instance.GetBallList();
		List<Sprite> ballSprites = UserProfile.Instance.ballSprites;

		for (int i = 0; i < ballList.Count; i++)
		{
			((Image)pairs[i].toggle.targetGraphic).sprite = ballSprites[i];
			pairs[i].button.GetComponentInChildren<Text>().text = price.ToString();

			if (ballList[i])
			{
				pairs[i].button.gameObject.SetActive(false);
				pairs[i].toggle.interactable = true;
				pairs[i].toggle.isOn = false;
			}
			else
			{
				pairs[i].button.gameObject.SetActive(true);
				pairs[i].toggle.interactable = false;
				pairs[i].toggle.isOn = false;
			}
		}

		foreach (ItemPair p in pairs)
		{
			p.button.onClick.AddListener(delegate { OnClickBuy(p.id); });
			p.toggle.onValueChanged.AddListener(delegate { OnToggleChange(p.id); });
			p.toggle.group = group;
		}

		pairs[UserProfile.Instance.GetActiveBall()].toggle.isOn = true;
	}

	public void UpdateSprite(int id)
	{
		UserProfile.Instance.SetBallSprite(id);
	}
	
	public void OnClickBuy(int id)
	{
		if (UserProfile.Instance.ReduceDiamond(price))
		{
			pairs[id].button.gameObject.SetActive(false);
			pairs[id].toggle.interactable = true;
			UserProfile.Instance.BuyBall(id);
		}
	}

	public void OnToggleChange(int id)
	{
		if (pairs[id].toggle.isOn)
		{
			UpdateSprite(id);
		}
	}
}
