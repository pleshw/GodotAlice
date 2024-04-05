using System;
using Entity;
using Godot;

namespace Animation;

public class EntityAnimationController
{
  public Callable CallOnMainAnimationFinish;

  public readonly EntityAnimationInfo AnimationInfo;

  public EntityAnimationController(EntityAnimationInfo animationInfo)
  {
    AnimationInfo = animationInfo;
    CallOnMainAnimationFinish = Callable.From(OnMainAnimationFinished);
  }

  public static void StopAnimation(AnimatedSprite2D animation)
  {
    animation.Stop();
    animation.Visible = false;
  }

  public static void PlayAnimation(AnimatedSprite2D animation)
  {
    animation.Visible = true;
    animation.Play(animation.Name);
  }

  public void PlayMainAnimation()
  {
    AnimationInfo.MainAnimationData.BeforeAnimationStart();
    AnimationInfo.MainAnimationData.Sprites.Visible = true;
    AnimationInfo.MainAnimationData.Sprites.Play(AnimationInfo.MainAnimationData.Name);
  }

  public void StopMainAnimation()
  {
    if (AnimationInfo.MainAnimationData != null)
    {
      StopAnimation(AnimationInfo.MainAnimationData.Sprites);
    }
  }

  public void PlayAnimation(AnimationData animation)
  {
    if (animation.CanPlayConcurrently)
    {
      AnimationInfo.AnimationSet.Add(animation);
    }
    else
    {
      TryPlayAsMainAnimation(animation);
    }
  }

  private void TryPlayAsMainAnimation(AnimationData animationData)
  {
    if (EntityAnimationInfo.HaveLessPriority(animationData, AnimationInfo.MainAnimationData))
    {
      return;
    }

    if (EntityAnimationInfo.HaveHigherPriority(animationData, AnimationInfo.MainAnimationData))
    {
      if (AnimationInfo.MainAnimationData != null && !AnimationInfo.MainAnimationData.CanBeInterrupted)
      {
        return;
      }

      StopMainAnimation();
      AnimationInfo.MainAnimationData = animationData;
      ConnectOnFinishedToMain();

      PlayMainAnimation();
      return;
    }

    if (EntityAnimationInfo.HaveSamePriority(animationData, AnimationInfo.MainAnimationData))
    {
      if (AnimationInfo.MainAnimationData != null)
      {
        bool isMainAnimationPlaying = AnimationInfo.MainAnimationData.Sprites.IsPlaying();
        bool canAnimationBeInterrupted = AnimationInfo.MainAnimationData.CanBeInterrupted;

        if ((animationData.Name == AnimationInfo.MainAnimationData.Name && isMainAnimationPlaying) || (isMainAnimationPlaying && !canAnimationBeInterrupted))
        {
          return;
        }
      }

      StopMainAnimation();
      AnimationInfo.MainAnimationData = animationData;
      ConnectOnFinishedToMain();

      PlayMainAnimation();
    }
  }

  public void ConnectOnFinishedToMain()
  {
    if (!AnimationInfo.MainAnimationData.Sprites.IsConnected(AnimatedSprite2D.SignalName.AnimationFinished, CallOnMainAnimationFinish))
    {
      AnimationInfo.MainAnimationData.Sprites.Connect(
       AnimatedSprite2D.SignalName.AnimationFinished,
       CallOnMainAnimationFinish
     );
    }
  }

  public void DisconnectOnFinishedFromMain()
  {
    if (AnimationInfo.MainAnimationData.Sprites.IsConnected(AnimatedSprite2D.SignalName.AnimationFinished, CallOnMainAnimationFinish))
    {
      AnimationInfo.MainAnimationData.Sprites.Disconnect(
       AnimatedSprite2D.SignalName.AnimationFinished,
       CallOnMainAnimationFinish
     );
    }
  }

  public void OnMainAnimationFinished()
  {
    DisconnectOnFinishedFromMain();
    StopMainAnimation();

    if (AnimationInfo.MainAnimationData.Sequel == null)
    {
      AnimationInfo.MainAnimationData = AnimationInfo.DefaultAnimationData;
    }
    else
    {
      AnimationInfo.MainAnimationData = AnimationInfo.MainAnimationData.Sequel;
      AnimationInfo.MainAnimationData.CanBeInterrupted = false;
      ConnectOnFinishedToMain();
    }

    PlayMainAnimation();
  }

  public static void RequestPlayAnimation(PlayAnimationRequest request, out EntityAnimationInfo animationInfo)
  {
    animationInfo = EntityAnimationInfo.GetInfoFrom(request.Entity);
    animationInfo.Controller.PlayAnimation(request.AnimationData);
  }

  public void SwitchAnimation(AnimationData animationToPlay)
  {
    StopMainAnimation();
    PlayAnimation(animationToPlay);
  }
}