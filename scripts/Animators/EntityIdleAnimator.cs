using System.Linq;
using Animation;
using Godot;

namespace Entity;

public partial class EntityIdleAnimator(Entity entity) : AnimatorNode(entity)
{
  public DIRECTIONS facingDirection = DIRECTIONS.BOTTOM;
  public DIRECTIONS lastFacingDirection = DIRECTIONS.BOTTOM;

  public override int Priority
  {
    get => 1;
    set { }
  }

  public AnimationData IdleAnimationData
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Idle"],
        Name = "Default"
      };
    }
  }

  public override void OnReady()
  {
    if (_entity.Animations.TryGetValue("Idle", out AnimatedSprite2D idleAnimations))
    {
      AnimationSprites.Add("Idle", idleAnimations);
    }

    HideAllAnimations();
  }

  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    TryPlayAnimation(IdleAnimationData);
  }

  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.Animations)
    {
      anim.Value.Visible = false;
    }
  }
}
