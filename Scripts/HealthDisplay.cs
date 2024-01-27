using System.Globalization;
using Godot;

namespace GGJ24.Scripts;

public partial class HealthDisplay : Control
{
	[Export] private RichTextLabel Label;
	private PlayerHealth Player => GetNode(CSharpGlobals.pathToPlayer).FindChild("PlayerHealth") as PlayerHealth;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Player == null)	
		{
			return;
		}

		Label.Text = $"[center]{Mathf.CeilToInt(Player.CurrentHealth).ToString(CultureInfo.InvariantCulture)}[/center]";
	}
}