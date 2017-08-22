using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PColor
{
	RED,
	GREEN,
	BLUE,
	LIME,
	WHITE,
	GRAY,
	YELLOW,
	BLACK,
	PURPLE,
	PINK,
	DARKYELLOW,
	GOLD,
	SILVER,
	BRONZE
}

public class Palette {

	public static int PColorSize()
	{
		return System.Enum.GetNames(typeof(PColor)).Length;
	}

	public static Color Translate(PColor color, byte alpha = 255)
	{
		switch (color)
		{
		case PColor.RED: return new Color32(223, 107, 107, alpha);
		case PColor.GREEN: return new Color32(74, 206, 108, alpha);
		case PColor.GRAY: return new Color32(58, 58, 58, alpha);
		case PColor.LIME: return new Color32(65, 201, 156, alpha);
		case PColor.BLUE: return new Color32(55, 78, 188, alpha);
		case PColor.BLACK: return new Color32(35, 35, 35, alpha);
		case PColor.YELLOW: return new Color32(74, 206, 108, alpha);
		case PColor.PURPLE: return new Color32(104, 55, 188, alpha);
		case PColor.WHITE: return new Color32(255, 255, 255, alpha);
		case PColor.PINK: return new Color32(206, 74, 148, alpha);
		case PColor.DARKYELLOW: return new Color32(74, 206, 108, alpha);
		case PColor.GOLD: return new Color32(173, 164, 65, alpha);
		case PColor.SILVER: return new Color32(129, 131, 159, alpha);
		case PColor.BRONZE: return new Color32(120, 89, 49, alpha);
		default: return new Color32(0, 0, 0, alpha);
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
			num = Random.Range(0, PColorSize());
		} while (num == exceptNum);
		return Palette.Translate((PColor)num);
	}

	public static Color RandomColorExcept(List<PColor> excepts)
	{
		int num;
		do
		{
			num = Random.Range(0, PColorSize());
		} while (excepts.IndexOf((PColor)num) > -1);

		return Palette.Translate((PColor)num);
	}
}
