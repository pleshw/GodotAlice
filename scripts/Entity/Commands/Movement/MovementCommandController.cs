using Godot;

namespace Entity.Commands;

public class MovementCommandController(Entity entityToMove)
{
  public readonly PlayerCommands.WalkTopCommand WalkTop = new(entityToMove);
  public readonly PlayerCommands.WalkRightCommand WalkRight = new(entityToMove);
  public readonly PlayerCommands.WalkBottomCommand WalkBottom = new(entityToMove);
  public readonly PlayerCommands.WalkLeftCommand WalkLeft = new(entityToMove);
  public readonly PlayerCommands.DashCommand Dash = new(entityToMove);
}