using Godot;

namespace Entity.Commands;

public class MovementCommandController(Entity entityToMove)
{
  public readonly EntityCommands.WalkTopCommand WalkTop = new(entityToMove);
  public readonly EntityCommands.WalkRightCommand WalkRight = new(entityToMove);
  public readonly EntityCommands.WalkBottomCommand WalkBottom = new(entityToMove);
  public readonly EntityCommands.WalkLeftCommand WalkLeft = new(entityToMove);
  public readonly EntityCommands.DashCommand Dash = new(entityToMove);
}