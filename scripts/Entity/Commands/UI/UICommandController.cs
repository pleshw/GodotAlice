using Godot;

namespace Entity.Commands.UI;

public class UICommandController(Entity owner)
{
  public readonly ToggleInventoryCommand ToggleInventory = new(owner);
}