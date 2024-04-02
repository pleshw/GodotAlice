using Godot;

namespace Entity.Commands.Movement;

public class MovementCommandKeybind(Entity entity) : EntityCommandKeybind(entity)
{
  public MovementCommandController movementController = new(entity);

  public override void BindDefaults()
  {
    BindKey(Key.W, movementController.WalkTop);
    BindKey(Key.D, movementController.WalkRight);
    BindKey(Key.S, movementController.WalkBottom);
    BindKey(Key.A, movementController.WalkLeft);
  }

  public void BindArrows()
  {
    BindKey(Key.Up, movementController.WalkTop);
    BindKey(Key.Right, movementController.WalkRight);
    BindKey(Key.Down, movementController.WalkBottom);
    BindKey(Key.Left, movementController.WalkLeft);
  }

  public void BindKeyWalkTop(Key key)
  {
    BindKey(key, movementController.WalkTop);
  }

  public void BindKeyWalkRight(Key key)
  {
    BindKey(key, movementController.WalkRight);
  }

  public void BindKeyWalkBottom(Key key)
  {
    BindKey(key, movementController.WalkBottom);
  }

  public void BindKeyWalkLeft(Key key)
  {
    BindKey(key, movementController.WalkLeft);
  }
}