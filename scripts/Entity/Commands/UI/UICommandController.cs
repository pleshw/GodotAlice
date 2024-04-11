using Godot;

namespace Entity.Commands;

public class UICommandController(Entity owner)
{
  public readonly EntityCommands.TogglePlayerMenuCommand TogglePlayerMenu = new(owner);
}