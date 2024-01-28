using Godot;
using GGJ24;
using GGJ24.Scripts;

public partial class AmmoUI : RichTextLabel
{
	private Inventory playerInventory;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerInventory = GetNode(CSharpGlobals.pathToPlayer).TryFindNodeOfTypeInChildren<Inventory>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int maxAmmo = playerInventory.CurrentWeapon.Resource.MaximumAmmo;
		int currentAmmo = playerInventory.CurrentWeapon.CurrentAmmo;

		string text;
		if (currentAmmo == -1)
		{
			 text = $"[center]{currentAmmo}/\n{maxAmmo}[/center]";
		}
		else
		{
			text = "PUNCH";
		}
		Text = text;
	}
}
