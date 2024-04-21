using Godot;
using Entity;
using System;
using GameManager;
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

      if (AnimatedBody.Freeze)
      {
        return;
      }

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

  public bool FlipH
  {
    get
    {
      return directionState.FacingSide == DIRECTIONS.LEFT;
    }
  }

  public void FlipAnimationToFacingSide()
  {
    AnimatedBody.Parts.ForEach(p => p.FlipH = FlipH);
  }

  public async Task PlayAnimationAsync(AnimationRequestInput animationRequest)
  {
    var animationFinishedTask = new TaskCompletionSource<bool>();
    LockAnimations = true;

    void _onFrameChange(AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount)
    {
      if (animationRequest.OnFrameChange is not null)
      {
        animationRequest.OnFrameChange(animatedSprite, initialTransform, currentFrame, animationFrameCount);
      }
    }

    void _onFinished(AnimatedSprite2D animatedSprite, Transform2D initialTransform)
    {
      if (animationRequest.OnFinished is not null)
      {
        animationRequest.OnFinished(animatedSprite, initialTransform);
      }
      animationFinishedTask.SetResult(true);
      LockAnimations = false;
    }

    FlipAnimationToFacingSide();
    AnimatedBody.Play(animationRequest with
    {
      OnFrameChange = _onFrameChange,
      OnFinished = _onFinished
    });

    await animationFinishedTask.Task;
  }

  public void PlayAnimation(AnimationRequestInput animationRequest)
  {
    LockAnimations = true;

    void _onFrameChange(AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount)
    {
      if (animationRequest.OnFrameChange is not null)
      {
        animationRequest.OnFrameChange(animatedSprite, initialTransform, currentFrame, animationFrameCount);
      }
    }

    void _onFinished(AnimatedSprite2D animatedSprite, Transform2D initialTransform)
    {
      if (animationRequest.OnFinished is not null)
      {
        animationRequest.OnFinished(animatedSprite, initialTransform);
      }
      LockAnimations = false;
    }

    AnimatedBody.Play(animationRequest with
    {
      OnFrameChange = _onFrameChange,
      OnFinished = _onFinished
    });
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

    BeforeAttackAnimationEvent();
    PlayAnimation(new()
    {
      Name = "Attacking",
      OnFrameChange = (AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount) =>
      {
        AttackAnimationFrameChangeEvent(currentFrame, animationFrameCount);
      },
      OnFinished = (AnimatedSprite2D animatedSprite, Transform2D initialTransform) =>
      {
        LockGameState = false;
        LockAnimations = false;
        GameState = EntityGameState.IDLE;
        MovementController.EnableMovement();
        AfterAttackAnimationEvent();
      },
      ForceDuration = 1 / Stats.AttacksPerSecond,
    });
  }

  public void PlayDashAnimation()
  {
    AnimatedBody.Play(new AnimationRequestInput()
    {
      Name = "Dashing",
      OnFrameChange = (AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount) =>
      {
        float reverseAnimationStage = Mathf.Remap(currentFrame, 0, animationFrameCount, .6f, .3f);
        float animationStage = Mathf.Remap(currentFrame, 0, animationFrameCount, .3f, .9f);

        animatedSprite.Rotate(10 * reverseAnimationStage);
        animatedSprite.Scale = initialTransform.Scale with
        {
          X = initialTransform.Scale.X * animationStage,
          Y = initialTransform.Scale.Y * animationStage
        };
      },
      OnFinished = (AnimatedSprite2D animatedSprite, Transform2D initialTransform) =>
      {
        animatedSprite.Rotation = initialTransform.Rotation;
        animatedSprite.Scale = initialTransform.Scale;
        LockAnimations = false;
      },
      ForceDuration = .6f
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