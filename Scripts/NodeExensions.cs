using Godot;

namespace GGJ24;

public static class NodeExtensions
{
    public static T TryFindNodeOfTypeInChildren<T>(this Node node) where T : class
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T nodeAsT)
            {
                return nodeAsT;
            }
        }
        return null;
    }
}