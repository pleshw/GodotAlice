using Godot;

namespace Entity.Commands;

public partial class PlayerCommands
{
  public class TogglePlayerMenuCommand : EntityCommandBase
  {
    private readonly Player owner;

    private Tween InventoryTween;

    public Vector2 CameraStartPosition;

    public Callable setGlobalCamera;
    public Callable setPlayerCamera;

    public Vector2 playerAnimPosition;

    public TogglePlayerMenuCommand(Player entity) : base(entity)
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

      InventoryTween = owner.GetTree().CreateTween();

      owner.MovementController.DisableMovement();
      owner.directionState.FacingDirectionVector = new Vector2 { X = 1, Y = 0 };

      owner.UIMenu.SetIndexed("modulate:a", 0.0f);

      if (owner.UIMenu.Visible)
      {
        InventoryTween.TweenCallback(Callable.From(() =>
        {
          owner.UIMenu.Visible = false;
          owner.MovementController.EnableMovement();
        }));
      }
      else
      {
        owner.UIMenu.Visible = true;

        var viewportSize = owner.GlobalCamera.GetViewport().GetVisibleRect().Size;
        owner.UIMenu.SetSize(viewportSize);
        owner.UIMenu.Position = Vector2.Zero;
        owner.UIMenu.Scale = Vector2.One;

        InventoryTween.TweenProperty(
          owner.UIMenu,
          "modulate:a",
          1,
          0.4f).SetTrans(Tween.TransitionType.Linear);
      }
    }
  }
}
