using Godot;
using System;
using GGJ24;
using GGJ24.Scripts;

public partial class InventoryUIContainer : GridContainer
{
	[Export] private PackedScene uiElement;
	[Export] private Node uiParent;
	[Export] private int neededSlots;
	private Inventory playerInventory;
	private ItemUI[] uiElements;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerInventory = GetNode(CSharpGlobals.pathToPlayer).TryFindNodeOfTypeInChildren<Inventory>();
		uiElements = new ItemUI[neededSlots];
		
		for (int i = 0; i < neededSlots; i++)
		{
			ItemUI spawnedElement = uiElement.Instantiate() as ItemUI;
			uiParent.AddChild(spawnedElement);
			uiElements[i] = spawnedElement;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for (var i = 0; i < playerInventory.Weapons.Count; i++)
		{
			var item = playerInventory.Weapons[i];
			var itemUi = uiElements[i];
			itemUi.SetWeapon(item);
			itemUi.SetEnabled(item == playerInventory.CurrentWeapon);
		}
	}
}
