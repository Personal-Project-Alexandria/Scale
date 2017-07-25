﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PColor
{
	RED,
	GREEN,
	BLUE,
	WHITE,
	GRAY,
	YELLOW,
	BLACK,
	PURPLE,
	PINK,
	DARKYELLOW,
	DARKGREEN
}

public class Palette {

	public static int PColorSize()
	{
		return System.Enum.GetNames(typeof(PColor)).Length;
	}

	public static Color Translate(PColor color)
	{
		switch (color)
		{
		case PColor.RED: return new Color32(223, 107, 107, 255);
		case PColor.GREEN: return new Color32(74, 206, 108, 255);
		case PColor.GRAY: return new Color32(58, 58, 58, 255);
		case PColor.BLUE: return new Color32(55, 78, 188, 255);
		case PColor.BLACK: return new Color32(35, 35, 35, 255);
		case PColor.YELLOW: return new Color32(206, 186, 74, 255);
		case PColor.PURPLE: return new Color32(104, 55, 188, 255);
		case PColor.WHITE: return new Color32(255, 255, 255, 255);
		case PColor.PINK: return new Color32(206, 74, 148, 255);
		case PColor.DARKYELLOW: return new Color32(206, 164, 74, 255);
		case PColor.DARKGREEN: return new Color32(42, 116, 32, 255);
		default: return new Color32(0, 0, 0, 255);
		}
	}

	public static Color RandomColor()
	{
		return Palette.Translate((PColor)Random.Range(0, PColorSize()));
	}

	public static Color RandomColorExcept(PColor except)
	{
		int exceptNum = (int)except;
		int num;
		do
		{
			num = Random.Range(0, PColorSize() - 1);
		} while (num == exceptNum);
		return Palette.Translate((PColor)num);
	}

	public static Color RandomColorExcept(List<PColor> excepts)
	{
		int num;
		do
		{
			num = Random.Range(0, PColorSize() - 1);
		} while (excepts.IndexOf((PColor)num) > -1);

		return Palette.Translate((PColor)num);
	}
}