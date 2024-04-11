using Godot;

namespace Entity.Commands;

public partial class EntityCommands
{
  public class TogglePlayerMenuCommand : EntityBaseCommand
  {
    private readonly Entity owner;

    private Tween InventoryTween;

    private Tween CameraTween;

    public Vector2 CameraStartPosition;

    public Callable setGlobalCamera;
    public Callable setPlayerCamera;

    public TogglePlayerMenuCommand(Entity entity) : base(entity)
    {
      owner = entity;

      setGlobalCamera = Callable.From(() => Entity.GlobalCamera.MakeCurrent());
      setPlayerCamera = Callable.From(() =>
      {
        owner.Camera.GlobalPosition = Entity.GlobalCamera.GlobalPosition;
        owner.Camera.Zoom = Entity.GlobalCamera.Zoom;
        owner.Camera.MakeCurrent();
      });

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
      owner.MovementController.DisableMovement();
      owner.directionState.FacingDirectionVector = new Vector2 { X = 1, Y = 0 };

      if (owner.MenuWindow.Visible)
      {
        InventoryTween.TweenCallback(setGlobalCamera);

        CameraTween.TweenProperty(
           Entity.GlobalCamera,
           "zoom",
           new Vector2(1, 1),
            0.2f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.OutIn);

        CameraTween.TweenProperty(
           Entity.GlobalCamera,
          "position",
          Vector2.Zero,
          0.1f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.OutIn);

        InventoryTween.TweenProperty(
          owner.MenuWindow,
          "modulate:a",
          0,
          0.3f).SetTrans(Tween.TransitionType.Linear);

        InventoryTween.TweenCallback(Callable.From(() =>
        {
          owner.MenuWindow.Visible = false;
          owner.MovementController.EnableMovement();
        }));

      }
      else
      {
        float cameraOffsetX = 350f;
        float cameraOffsetY = 130f;
        owner.MenuWindow.Visible = true;

        CameraTween.TweenProperty(
          Entity.GlobalCamera,
          "position",
          owner.GlobalPosition + new Vector2(cameraOffsetX, cameraOffsetY),
          0.1f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.Out);

        CameraTween.TweenProperty(
          Entity.GlobalCamera,
           "zoom",
           new Vector2(4f, 4f),
            0.3f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.OutIn);


        var viewportSize = Entity.GlobalCamera.GetViewportRect().Size;
        owner.MenuWindow.Scale = new Vector2(1, 1);

        owner.MenuWindow.SetPosition(new()
        {
          X = -140,
          Y = -(owner.MenuWindow.Size.Y / 2) + cameraOffsetY - 15
        });

        InventoryTween.TweenProperty(
          owner.MenuWindow,
          "modulate:a",
          1,
          0.2f).SetTrans(Tween.TransitionType.Linear);

        InventoryTween.TweenCallback(setPlayerCamera);
      }
    }
  }
}
