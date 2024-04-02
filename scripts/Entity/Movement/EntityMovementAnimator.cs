using System.Linq;
using Animation;
using Godot;

namespace Entity;


public partial class EntityMovementAnimator(Entity entity) : Animator(entity)
{
  private AnimatedSprite2D _idleAnimations;
  private AnimatedSprite2D _walkingAnimations;
  public DIRECTIONS facingDirection = DIRECTIONS.BOTTOM;
  public DIRECTIONS lastFacingDirection = DIRECTIONS.BOTTOM;

  public override int Priority
  {
    get => 1;
    set { }
  }

  public void Init()
  {
    if (_entity.MovementAnimations.TryGetValue("Idle", out AnimatedSprite2D idleAnimations))
    {
      _idleAnimations = idleAnimations;
    }

    if (_entity.MovementAnimations.TryGetValue("Walking", out AnimatedSprite2D walkingAnimations))
    {
      _walkingAnimations = walkingAnimations;
    }

    HideAllAnimations();
  }

  public void PlayIdle()
  {
    if (_idleAnimations == null)
    {
      return;
    }

    TryPlayAnimation(_idleAnimations, "Default");
  }

  public void ChangeAnimationProcess()
  {
    switch (_entity.MovementState)
    {
      case MOVEMENT_STATE.IDLE:
        PlayIdle();
        break;
      case MOVEMENT_STATE.WALKING:
        PlayWalkingAnimation();
        break;
      default:
        return;
    }
  }

  public void PlayWalkingAnimation()
  {
    if (_walkingAnimations == null)
    {
      return;
    }

    switch (facingDirection)
    {
      case DIRECTIONS.TOP:
        TryPlayAnimation(_walkingAnimations, "Top");
        break;
      case DIRECTIONS.RIGHT:
        TryPlayAnimation(_walkingAnimations, "Right");
        break;
      case DIRECTIONS.BOTTOM:
        TryPlayAnimation(_walkingAnimations, "Bottom");
        break;
      case DIRECTIONS.LEFT:
        TryPlayAnimation(_walkingAnimations, "Left");
        break;
      default:
        TryPlayAnimation(_walkingAnimations, "Bottom");
        break;
    }
  }

  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.MovementAnimations)
    {
      anim.Value.Visible = false;
    }
  }
}
