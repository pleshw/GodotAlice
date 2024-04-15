using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Godot;

namespace Entity;


public delegate void FrameChangedEvent(AnimatedSprite2D animationSprites, int currentFrame, int animationFrameCount);

public abstract partial class EntityActionAnimator(Entity entity) : AnimatorNode(entity)
{
  protected override Dictionary<string, AnimationData> Animations { get; set; } = [];

  public override abstract void OnReady();
  public override abstract void Play();



  public bool PerformingAction = false;
  /// <summary>
  /// 
  /// </summary>
  /// <param name="actionAnimationKey">e.g: DashingDefault</param>
  /// <param name="animationSpriteName">e.g: Dashing</param>
  /// <param name="onAnimationFinished"></param>
  public void PlayActionByName(string actionAnimationKey, string animationSpriteName, FrameChangedEvent onAnimationProgress, Action onAnimationFinished)
  {
    if (AnimationSprites == null || Entity.GameStates.HasFlag(GameStates.PERFORMING_ACTION))
    {
      return;
    }

    AnimatedSprite2D animationSprites = Animations[actionAnimationKey].Sprites;
    int animationFrameCount = AnimationSprites[animationSpriteName].SpriteFrames.GetFrameCount(Animations[actionAnimationKey].Name);
    Entity.GameStates |= GameStates.PERFORMING_ACTION;

    void _onAnimationProgress() => EachActionFrame(animationSprites, animationFrameCount, onAnimationProgress);

    void _onAnimationFinished()
    {
      animationSprites.Disconnect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(_onAnimationProgress));
      animationSprites.Disconnect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(_onAnimationFinished));

      Entity.GameStates &= ~GameStates.PERFORMING_ACTION;
      onAnimationFinished();
    };

    animationSprites.Connect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(_onAnimationProgress));
    animationSprites.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(_onAnimationFinished));

    PlayAnimation(Animations[actionAnimationKey]);
  }

  public static void EachActionFrame(AnimatedSprite2D animationSprites, int animationFrameCount, FrameChangedEvent onAnimationProgress)
  {
    int currentFrame = animationSprites.Frame;
    onAnimationProgress(animationSprites, currentFrame, animationFrameCount);
  }


  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.AnimationsByName)
    {
      anim.Value.Visible = false;
    }
  }
}
