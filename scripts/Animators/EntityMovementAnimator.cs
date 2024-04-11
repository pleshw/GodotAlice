using System.Collections.Generic;
using System.Linq;
using Animation;
using Godot;

namespace Entity;

public partial class EntityMovementAnimator(Entity entity) : AnimatorNode(entity)
{

  /// The name of the AnimatedSprite2D + the name of the Animation
  protected override Dictionary<string, AnimationData> Animations { get; set; } = [];

  public override void OnReady()
  {
    AddMovementAnimation("Walking");
    AddMovementAnimation("Dashing", 3, false);

    ConfirmAnimations();
    HideAllAnimations();
  }

  public void AddMovementAnimation(string animationName, int animationPriority = 1, bool animationCanBeInterrupted = true)
  {
    if (_entity.AnimationsByName.TryGetValue(animationName, out AnimatedSprite2D dashingAnimations))
    {
      AnimationSprites.Add(animationName, dashingAnimations);
      SpriteFrames animations = dashingAnimations.SpriteFrames;
      string[] animationNames = animations.GetAnimationNames();
      foreach (string name in animationNames)
      {
        Animations.Add(animationName + name, GetMovementAnimationData(animationName, name, animationPriority, animationCanBeInterrupted));
      }
    }
  }

  private AnimationData GetMovementAnimationData(string spritesKey, string animationName, int priority = 1, bool canBeInterrupted = true)
  {
    return new AnimationData
    {
      Sprites = AnimationSprites[spritesKey],
      Name = animationName,
      Animator = this,
      Priority = priority,
      Entity = Entity,
      CanPlayConcurrently = false,
      CanBeInterrupted = canBeInterrupted,
      BeforeAnimationStart = () =>
      {
        AnimationSprites[spritesKey].FlipH = Entity.directionState.FacingSide == DIRECTIONS.LEFT;
      }
    };
  }

  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    MOVEMENT_STATE movementState = Entity.MovementController.States.Max;
    switch (movementState)
    {
      case MOVEMENT_STATE.WALKING:
        PlayWalkAnimation();
        return;
      case MOVEMENT_STATE.DASHING:
        PlayDashAnimation();
        return;
    }
  }

  private bool AlreadyDashing = false;
  private void PlayDashAnimation()
  {
    if (AlreadyDashing)
    {
      return;
    }

    AnimatedSprite2D animationSprites = Animations["DashingDefault"].Sprites;
    int animationFrameCount = AnimationSprites["Dashing"].SpriteFrames.GetFrameCount(Animations["DashingDefault"].Name);
    animationSprites.SpeedScale = Entity.DashSpeedModifier / 2;
    Vector2 initialScale = animationSprites.Scale;
    AlreadyDashing = true;
    Entity.Stats.IsInvulnerable = true;

    void onDashProgress() => OnDashProgress(animationSprites, animationFrameCount, initialScale);

    void onAnimationFinished()
    {
      animationSprites.Disconnect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(onDashProgress));

      animationSprites.Disconnect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(onAnimationFinished));

      animationSprites.Scale = initialScale;
      AlreadyDashing = false;
      Entity.Stats.IsInvulnerable = false;
    };

    animationSprites.Connect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(onDashProgress));
    animationSprites.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(onAnimationFinished));

    PlayAnimation(Animations["DashingDefault"]);
  }

  public static void OnDashProgress(AnimatedSprite2D animationSprites, int animationFrameCount, Vector2 initialScale)
  {
    int currentFrame = animationSprites.Frame;
    float reverseAnimationStage = Mathf.Remap(currentFrame, 0, animationFrameCount, .7f, 0);
    float reverseAnimationStateScaleFactor = Mathf.Remap(currentFrame, 0, animationFrameCount, .7f, 1);

    (animationSprites.Material as ShaderMaterial).SetShaderParameter("blinkStage", reverseAnimationStage);

    animationSprites.Scale = initialScale with { Y = initialScale.Y * reverseAnimationStateScaleFactor };
  }

  private void PlayWalkAnimation()
  {
    AnimationSprites["Walking"].SpeedScale = Mathf.Pow(Entity.MovementController.MoveSpeed, 2) / (Entity.MovementController.MoveSpeed * 10);
    switch (Entity.directionState.LastCommandDirection)
    {
      case DIRECTIONS.TOP:
        PlayAnimation(Animations["WalkingTop"]);
        break;
      case DIRECTIONS.RIGHT:
        PlayAnimation(Animations["WalkingRight"]);
        break;
      case DIRECTIONS.BOTTOM:
        PlayAnimation(Animations["WalkingBottom"]);
        break;
      case DIRECTIONS.LEFT:
        PlayAnimation(Animations["WalkingLeft"]);
        break;
      default:
        PlayAnimation(Entity.idleAnimator.IdleAnimationData);
        break;
    }
  }


  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.AnimationsByName)
    {
      anim.Value.Visible = false;
    }
  }
}
