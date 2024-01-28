using Godot;
using System;

public partial class ItemUI : Panel
{
	[Export] private TextureRect textRect;
	[Export] private StyleBox enabledStyle;
	[Export] private StyleBox disabledStyle;

	public override void _Ready()
	{
		base._Ready();
		SetEnabled(false);
	}

	public void SetWeapon(Weapon wep)
	{
		textRect.Texture = wep.Resource.WeaponSprite as Texture2D;
	}

	public void SetEnabled(bool active)
	{
		if (active)
		{
			AddThemeStyleboxOverride("panel", enabledStyle);
		}
		else
		{
			AddThemeStyleboxOverride("panel", disabledStyle);
		}
	}
}
