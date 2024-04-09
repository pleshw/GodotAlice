using Godot;

namespace Entity.Commands.UI;

public class ToggleInventoryCommand : EntityBaseCommand
{
  private readonly Entity owner;

  private Tween InventoryTween;

  private Tween CameraTween;

  public Vector2 CameraStartPosition;

  public ToggleInventoryCommand(Entity entity) : base(entity)
  {
    owner = entity;

    entity.Ready += () =>
    {
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
          0.1f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.Out);

      InventoryTween.TweenProperty(
        owner.InventoryWindow,
        "modulate:a",
        0,
        0.2f).SetTrans(Tween.TransitionType.Linear);

      CameraTween.TweenProperty(
        owner.Camera,
        "position",
        CameraStartPosition,
        0.2f).SetTrans(Tween.TransitionType.Linear);

      InventoryTween.TweenCallback(Callable.From(() => owner.InventoryWindow.Visible = false));
    }
    else
    {
      owner.InventoryWindow.Visible = true;

      CameraTween.TweenProperty(
        owner.Camera,
        "position",
        CameraStartPosition + new Vector2(1250f, -100f),
        0.1f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.In);

      CameraTween.TweenProperty(
        owner.Camera,
         "zoom",
         new Vector2(1.5f, 1.5f),
          0.1f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.In);

      InventoryTween.TweenProperty(
        owner.InventoryWindow,
        "modulate:a",
        1,
        0.2f).SetTrans(Tween.TransitionType.Linear);
    }
  }
}