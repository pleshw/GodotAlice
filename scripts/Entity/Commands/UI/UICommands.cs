using Godot;

namespace Entity.Commands.UI;

public class ToggleInventoryCommand : EntityBaseCommand
{
  private readonly Entity owner;

  private Tween InventoryTween;

  private Tween CameraTween;

  public Vector2 InventoryWindowStartPosition;
  public Vector2 CameraStartPosition;

  public ToggleInventoryCommand(Entity entity) : base(entity)
  {
    owner = entity;

    entity.Ready += () =>
    {
      InventoryWindowStartPosition = owner.InventoryWindow.Position;
      CameraStartPosition = owner.Camera.Position;
    };
  }

  public override void Execute(bool repeating)
  {
    if (repeating)
    {
      return;
    }

    InventoryTween?.Kill();
    CameraTween?.Kill();

    CameraTween = owner.GetTree().CreateTween();
    InventoryTween = owner.GetTree().CreateTween();

    if (owner.InventoryWindow.Visible)
    {
      CameraTween.TweenProperty(
        owner.Camera,
         "zoom",
         new Vector2(1, 1),
          0.1f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);

      InventoryTween.TweenProperty(
        owner.InventoryWindow,
         "position",
          InventoryWindowStartPosition + new Vector2(50, 0),
          0.1f).SetTrans(Tween.TransitionType.Linear);

      InventoryTween.TweenProperty(
        owner.InventoryWindow,
        "modulate:a",
        0,
        0.2f).SetTrans(Tween.TransitionType.Linear);

      CameraTween.TweenProperty(
        owner.Camera,
        "position",
        CameraStartPosition,
        0.1f).SetTrans(Tween.TransitionType.Linear);

      InventoryTween.TweenCallback(Callable.From(() => owner.InventoryWindow.Visible = false));
    }
    else
    {
      owner.InventoryWindow.Visible = true;

      CameraTween.TweenProperty(
        owner.Camera,
        "position",
        CameraStartPosition + new Vector2(400f, 0),
        0f).SetTrans(Tween.TransitionType.Linear);

      CameraTween.TweenProperty(
        owner.Camera,
         "zoom",
         new Vector2(2f, 2f),
          0.1f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);

      InventoryTween.TweenProperty(
        owner.InventoryWindow,
         "position",
          InventoryWindowStartPosition,
          0.1f).SetTrans(Tween.TransitionType.Linear);

      InventoryTween.TweenProperty(
        owner.InventoryWindow,
        "modulate:a",
        1,
        0.2f).SetTrans(Tween.TransitionType.Linear);
    }
  }
}