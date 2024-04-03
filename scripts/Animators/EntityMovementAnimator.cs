using Animation;
using Godot;

namespace Entity;

public partial class EntityMovementAnimator(Entity entity) : AnimatorNode(entity)
{
  public bool IsUpdated { get; set; }

  public override int Priority
  {
    get => 2;
    set { }
  }

  public override void OnReady()
  {
    if (_entity.Animations.TryGetValue("Walking", out AnimatedSprite2D walkingAnimations))
    {
      AnimationSprites.Add("Walking", walkingAnimations);
    }

    if (_entity.Animations.TryGetValue("Running", out AnimatedSprite2D runningAnimations))
    {
      AnimationSprites.Add("Running", runningAnimations);
    }

    HideAllAnimations();
  }

  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    switch (Entity.facingDirection)
    {
      case DIRECTIONS.TOP:
        PlayWalkAnimationByName("Top");
        break;
      case DIRECTIONS.RIGHT:
        AnimationSprites["Walking"].FlipH = false;
        PlayWalkAnimationByName("Sides");
        break;
      case DIRECTIONS.BOTTOM:
        PlayWalkAnimationByName("Bottom");
        break;
      case DIRECTIONS.LEFT:
        AnimationSprites["Walking"].FlipH = true;
        PlayWalkAnimationByName("Sides");
        break;
      default:
        PlayWalkAnimationByName("Bottom");
        break;
    }
  }

  public void PlayWalkAnimationByName(StringName animationName)
  {
    TryPlayAnimation(new AnimationData
    {
      Animator = this,
      Animation = AnimationSprites["Walking"],
      Name = animationName,
      WaitToFinish = true
    });
  }

  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.Animations)
    {
      anim.Value.Visible = false;
    }
  }
}
