using Godot;

namespace Entity.Commands.UI;

public class ToggleInventoryCommand(Entity owner) : EntityMovementCommand(owner)
{
  public override void Execute(bool repeating)
  {
    if (repeating)
    {
      return;
    }


  }
}