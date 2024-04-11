using Godot;

namespace Entity.Commands;

public partial class EntityCommands
{
  public class TogglePlayerMenuCommand : EntityBaseCommand
  {
    private readonly Entity owner;

    private Tween InventoryTween;

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

      InventoryTween = owner.GetTree().CreateTween();
      owner.MovementController.DisableMovement();
      owner.directionState.FacingDirectionVector = new Vector2 { X = 1, Y = 0 };

      owner.MenuWindow.SetIndexed("modulate:a", 0.0f);

      if (owner.MenuWindow.Visible)
      {
        InventoryTween.TweenCallback(setGlobalCamera);

        InventoryTween.TweenProperty(
           Entity.GlobalCamera,
           "zoom",
           new Vector2(1, 1),
            0.2f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.OutIn);

        InventoryTween.TweenProperty(
           Entity.GlobalCamera,
          "position",
          Vector2.Zero,
          0.3f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.OutIn);

        InventoryTween.TweenCallback(Callable.From(() =>
        {
          owner.MenuWindow.Visible = false;
          owner.MovementController.EnableMovement();
        }));
      }
      else
      {
        float cameraOffsetX = 370f;
        float cameraOffsetY = 50f;
        owner.MenuWindow.Visible = true;

        InventoryTween.TweenProperty(
          Entity.GlobalCamera,
          "position",
          owner.GlobalPosition + new Vector2(cameraOffsetX, cameraOffsetY),
          0.2f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.Out);

        InventoryTween.TweenProperty(
          Entity.GlobalCamera,
           "zoom",
           new Vector2(2f, 2f),
            0.3f).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.OutIn);


        var viewportSize = Entity.GlobalCamera.GetViewportRect().Size;
        owner.MenuWindow.Scale = new Vector2(1, 1);

        owner.MenuWindow.SetPosition(new()
        {
          X = -120,
          Y = -(owner.MenuWindow.Size.Y / 2) + cameraOffsetY - 15
        });

        InventoryTween.TweenProperty(
          owner.MenuWindow,
          "modulate:a",
          1,
          0.5f).SetTrans(Tween.TransitionType.Linear);

        InventoryTween.TweenCallback(setPlayerCamera);
      }
    }
  }
}
