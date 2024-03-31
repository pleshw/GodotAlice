using Godot;

namespace Entity.Commands.Movement;

public class MovementCommandController(Entity entityToMove)
{
  public readonly WalkTopCommand WalkTop = new(entityToMove);
  public readonly WalkRightCommand WalkRight = new(entityToMove);
  public readonly WalkBottomCommand WalkBottom = new(entityToMove);
  public readonly WalkLeftCommand WalkLeft = new(entityToMove);
}