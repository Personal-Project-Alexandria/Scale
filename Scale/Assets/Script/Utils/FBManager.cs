using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

public class FBManager : MonoSingleton<FBManager> {

	private List<string> Perms = new List<string> { "public_profile", "email", "user_friends" };
	private string fbname = "";

	public ScreenRecorder screenRecorder;

	protected void Awake()
	{
		FB.Init(InitCompleteCallback, UnityCallbackDelegate);
	}

	public void Login()
	{
		if (!FB.IsLoggedIn)
		{
			FB.LogInWithReadPermissions(Perms, LogInCallback);
		}
		else
		{
			ShareLink();
		}
	}

	public void ShareLink()
	{
#if UNITY_ANDROID
		FB.ShareLink(new Uri("https://play.google.com/store/apps/details?id=com.topfreepuzzle.tapshrink"), UserProfile.Instance.gameName, "Join us to play this awesome game", null, ShareLinkCallback);
#elif UNITY_IOS
		FB.ShareLink(new Uri("https://itunes.apple.com/us/app/id1264453259"), UserProfile.Instance.gameName, "Join us to play this awesome game", null, ShareLinkCallback);
#endif
	}

	private bool sharing = true;

	private Uri screenshotUri;

	public void SetupUserName()
	{
		if (!fbname.Equals(""))
		{
			return;
		}

		if (FB.IsLoggedIn)
		{ 
			FB.API("/me?fields=name", HttpMethod.GET, GetUserCallback);
		}	
	}

	public string GetUserName()
	{
		return this.fbname;
	}


	private void ShareScreenCallback(IGraphResult reusult)
	{

	}
#region Callback
	private void LogInCallback(IResult result)
	{
		if (result.Cancelled)
		{
			Debug.Log("User cancelled");
		}
		else
		{
			Debug.Log("Login successfully");
			SetupUserName();
		}
	}
	private void ShareLinkCallback(IShareResult result) 
	{
		if (result.Cancelled)
		{
			Debug.Log("Share cancelled");
		}
		else
		{
			Debug.Log("Share successfully");
		}
	}
	private void InitCompleteCallback()
	{
		if (FB.IsInitialized)
		{
			Debug.Log("Succesfully initialized");
			SetupUserName();
		}
		else
		{
			Debug.Log("Init failed");
		}
	}
	private void UnityCallbackDelegate(bool isUnity)
	{
		if (isUnity)
		{
			Time.timeScale = 1f;
		}
		else
		{
			Time.timeScale = 0f;
		}
	}
	private void GetUserCallback(IGraphResult result)
	{
		if (FB.IsLoggedIn)
		{
			IDictionary<string, object> dict = result.ResultDictionary;
			fbname = dict["name"].ToString();
		}
	}
#endregion
}
