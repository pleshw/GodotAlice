using System.Linq;
using Godot;

namespace Entity;


public partial class EntityMovementAnimator(Entity entity)
{
  readonly Entity _entity = entity;

  private AnimatedSprite2D _lastPlayedAnimation;

  private AnimatedSprite2D _idleAnimations;

  private AnimatedSprite2D _walkingAnimations;

  public DIRECTIONS facingDirection = DIRECTIONS.BOTTOM;
  public DIRECTIONS lastFacingDirection = DIRECTIONS.BOTTOM;

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

    HideLastPlayedAnimation();
    _idleAnimations.Visible = true;
    _idleAnimations.Play("Default");
    _lastPlayedAnimation = _idleAnimations;
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

    HideLastPlayedAnimation();
    _walkingAnimations.Visible = true;

    switch (facingDirection)
    {
      case DIRECTIONS.TOP:
        _walkingAnimations.Play("Top");
        break;
      case DIRECTIONS.RIGHT:
        _walkingAnimations.Play("Right");
        break;
      case DIRECTIONS.BOTTOM:
        _walkingAnimations.Play("Bottom");
        break;
      case DIRECTIONS.LEFT:
        _walkingAnimations.Play("Left");
        break;
      default:
        _walkingAnimations.Play("Bottom");
        break;
    }

    _lastPlayedAnimation = _walkingAnimations;
  }

  private void HideAllAnimations()
  {
    foreach (var anim in _entity.MovementAnimations)
    {
      anim.Value.Visible = false;
    }
  }

  private void HideLastPlayedAnimation()
  {
    if (_lastPlayedAnimation == null)
    {
      return;
    }

    _lastPlayedAnimation.Visible = false;
  }
}
