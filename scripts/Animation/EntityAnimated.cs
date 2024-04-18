using Godot;
using Entity;
using System;
using GameManagers;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Entity;

public abstract partial class EntityAnimated(Vector2 initialPosition) : Entity(initialPosition)
{
  [Export]
  public EntityAnimatedBody AnimatedBody;

  public bool LockAnimations = false;

  public Dictionary<EntityGameState, string> AnimationNameByState = new() {
    {EntityGameState.IDLE,"Idle"},
    {EntityGameState.MOVING,"Moving"},
    {EntityGameState.DASHING, "Dashing" }
  };

  public override void Setup(Vector2 initialPosition = default, int gridCellWidth = 32)
  {
    base.Setup(initialPosition, gridCellWidth);
  }

  public override void _Ready()
  {
    base._Ready();

    AnimatedBody.Play("Idle");

    OnMovedEvent += (Vector2 from, Vector2 to) =>
    {
      FlipAnimationToFacingSide();
    };

    OnGameStateChangeEvent += (EntityGameState previousState, EntityGameState currentState) =>
    {
      bool animationPlaying = AnimatedBody.IsPlaying();
      if (LockAnimations && animationPlaying)
      {
        return;
      }

      if (previousState == currentState && animationPlaying)
      {
        return;
      }

      AnimatedBody.Stop();
      AnimatedBody.EmitSignal(AnimatedSprite2D.SignalName.AnimationFinished);

      switch (currentState)
      {
        case EntityGameState.IDLE:
          AnimatedBody.Play("Idle");
          return;
        case EntityGameState.MOVING:
          AnimatedBody.Play("Moving");
          return;
        case EntityGameState.DASHING:
          PlayDashAnimation();
          return;
      }
    };
  }

  public void FlipAnimationToFacingSide()
  {
    AnimatedBody.Parts.ForEach(p => p.FlipH = directionState.FacingSide == DIRECTIONS.LEFT);
  }

  public async Task PlayAnimationAsync(StringName animationName, OnFrameChangeEvent onFrameChange, OnAnimationFinishedEvent onFinished)
  {
    var animationFinishedTask = new TaskCompletionSource<bool>();
    LockAnimations = true;

    void _onFinished(AnimatedSprite2D animatedSprite, Transform2D initialTransform)
    {
      onFinished(animatedSprite, initialTransform);
      animationFinishedTask.SetResult(true);
      LockAnimations = false;
    }

    FlipAnimationToFacingSide();
    AnimatedBody.Play(animationName, onFrameChange, _onFinished);

    await animationFinishedTask.Task;
  }

  public void PlayAnimation(StringName animationName, OnFrameChangeEvent onFrameChange, OnAnimationFinishedEvent onFinished)
  {
    LockAnimations = true;

    void _onFinished(AnimatedSprite2D animatedSprite, Transform2D initialTransform)
    {
      onFinished(animatedSprite, initialTransform);
      LockAnimations = false;
    }

    AnimatedBody.Play(animationName, onFrameChange, _onFinished);
  }

  public void PlayAttackAnimation()
  {
    if (LockAnimations)
    {
      return;
    }

    MovementController.DisableMovement();

    LockGameState = true;
    LockAnimations = true;

    FlipAnimationToFacingSide();

    PlayAnimation("Attacking",
     (AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount) =>
     {

     },

     (AnimatedSprite2D animatedSprite, Transform2D initialTransform) =>
     {
       LockGameState = false;
       LockAnimations = false;
       GameState = EntityGameState.IDLE;
       MovementController.EnableMovement();
     });
  }

  public async Task PlayAttackAnimationAsync()
  {
    if (LockAnimations)
    {
      return;
    }

    MovementController.DisableMovement();

    LockGameState = true;
    LockAnimations = true;

    FlipAnimationToFacingSide();

    await PlayAnimationAsync("Attacking",
      (AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount) =>
      {

      },

      (AnimatedSprite2D animatedSprite, Transform2D initialTransform) =>
      {
        GameState = EntityGameState.IDLE;
      });

    LockGameState = false;
    LockAnimations = false;
    MovementController.EnableMovement();
  }

  public void PlayDashAnimation()
  {
    float initialSpeed = AnimatedBody.SpriteReference.SpeedScale;
    AnimatedBody.SetSpeedScale(initialSpeed * DashSpeedModifier);
    AnimatedBody.Play("Dashing", (AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount) =>
    {
      float reverseAnimationStage = Mathf.Remap(currentFrame, 0, animationFrameCount, .7f, 0);
      float reverseAnimationStateScaleFactor = Mathf.Remap(currentFrame, 0, animationFrameCount, .6f, 1);

      (animatedSprite.Material as ShaderMaterial).SetShaderParameter("blinkStage", reverseAnimationStage);

      animatedSprite.Scale = initialTransform.Scale with { Y = initialTransform.Scale.Y * reverseAnimationStateScaleFactor };
    },
    (AnimatedSprite2D animatedSprite, Transform2D initialTransform) =>
    {
      animatedSprite.Scale = initialTransform.Scale;
      (animatedSprite.Material as ShaderMaterial).SetShaderParameter("blinkStage", 0);
      LockAnimations = false;
      AnimatedBody.SetSpeedScale(initialSpeed);
    });
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
  }


  public void CancelCurrentInteraction()
  {
    if (!GameState.HasFlag(EntityGameState.INTERACTING))
    {
      return;
    }

    GameState = EntityGameState.IDLE;
  }
}