using System.Globalization;
using Godot;

namespace GGJ24.Scripts.Projectiles;

public partial class ArmorDisplay : Control
{
    [Export] private RichTextLabel Label;
    private PlayerHealth Player =>  GetNode(CSharpGlobals.pathToPlayer).FindChild("PlayerHealth") as PlayerHealth;
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Player == null)	
        {
            return;
        }

        Label.Text = $"[center]{Mathf.CeilToInt(Player.Armor).ToString(CultureInfo.InvariantCulture)}[/center]";
    }
}