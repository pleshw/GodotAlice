using Godot;

namespace Entity.Commands;

public class UICommandController(Player owner)
{
  public readonly PlayerCommands.TogglePlayerMenuCommand TogglePlayerMenu = new(owner);
}