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

  public static void StopAnimation(AnimatedBody animation)
  {
    animation.Stop();
    animation.Visible = false;
  }

  public static void PlayAnimation(AnimatedBody animation)
  {
    animation.Visible = true;
    animation.Play(animation.Name);
  }

  public void PlayMainAnimation()
  {
    AnimationInfo.MainAnimationData.BeforeAnimationStart();
    AnimationInfo.MainAnimationData.BodySprites.Visible = true;
    AnimationInfo.MainAnimationData.BodySprites.Play(AnimationInfo.MainAnimationData.Name);
  }

  public void StopMainAnimation()
  {
    if (AnimationInfo.MainAnimationData != null)
    {
      StopAnimation(AnimationInfo.MainAnimationData.BodySprites);
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
        bool isMainAnimationPlaying = AnimationInfo.MainAnimationData.BodySprites.IsPlaying();
        bool canAnimationBeInterrupted = AnimationInfo.MainAnimationData.CanBeInterrupted;

        if ((animationData.Name == AnimationInfo.MainAnimationData.Name && isMainAnimationPlaying) || (isMainAnimationPlaying && !canAnimationBeInterrupted))
        {
          AnimationInfo.Buffer = animationData;
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
    if (!AnimationInfo.MainAnimationData.BodySprites.IsConnected(AnimatedSprite2D.SignalName.AnimationFinished, CallOnMainAnimationFinish))
    {
      AnimationInfo.MainAnimationData.BodySprites.Connect(
       AnimatedSprite2D.SignalName.AnimationFinished,
       CallOnMainAnimationFinish
     );
    }
  }

  public void DisconnectOnFinishedFromMain()
  {
    if (AnimationInfo.MainAnimationData.BodySprites.IsConnected(AnimatedSprite2D.SignalName.AnimationFinished, CallOnMainAnimationFinish))
    {
      AnimationInfo.MainAnimationData.BodySprites.Disconnect(
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
      if (AnimationInfo.Buffer != null)
      {
        AnimationInfo.MainAnimationData = AnimationInfo.Buffer;
        AnimationInfo.Buffer = null;
      }
      else
      {
        AnimationInfo.MainAnimationData = AnimationInfo.DefaultAnimationData;
      }
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