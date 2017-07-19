using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
	Button, Buy, Popup, Touch
}

public class SoundManager : MonoSingleton<SoundManager> {

	protected bool music = true;
	protected bool sfx = true;

	public AudioSource backgroundMusic;
	public AudioSource button;
	public AudioSource buy_success;
	public AudioSource popup;
	public AudioSource touch;

	public bool IsBackgroundPlaying()
	{
		return this.music;
	}

	public bool IsSfxPlaying()
	{
		return this.sfx;
	}

	public void ToggleSfx(bool isOn)
	{
		this.sfx = isOn;

		if (this.sfx)
		{
			button.volume = 0.5f;
			buy_success.volume = 1f;
			popup.volume = 1f;
			touch.volume = 1f;
		}
		else
		{
			button.volume = 0f;
			buy_success.volume = 0f;
			popup.volume = 0f;
			touch.volume = 0f;
		}
	}

	public void ToggleMusic(bool isOn)
	{
		this.music = isOn;

		if (this.music)
		{
			backgroundMusic.volume = 0.8f;
		}
		else
		{
			backgroundMusic.volume = 0f;
		}
	}

	public void PlaySfx(SFX type)
	{
		switch (type)
		{
		case SFX.Button:
			if (button.isPlaying)
			{
				button.Stop();
			}
			button.Play();
			break;
		case SFX.Buy:
			if (buy_success.isPlaying)
			{
				buy_success.Stop();
			}
			buy_success.Play();
			break;
		case SFX.Popup:
			if (popup.isPlaying)
			{
				popup.Stop();
			}
			popup.Play();
			break;
		case SFX.Touch:
			if (touch.isPlaying)
			{
				touch.Stop();
			}
			touch.Play();
			break;
		}
	}

	public void Start()
	{
		backgroundMusic.Play();
	}
}
