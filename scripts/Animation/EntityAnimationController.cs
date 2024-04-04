using System;
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
    animation.Play();
  }

  public void PlayMainAnimation()
  {
    AnimationInfo.MainAnimationData.BeforeAnimationStart();
    AnimationInfo.MainAnimationData.Animation.Visible = true;
    AnimationInfo.MainAnimationData.Animation.Play();
  }

  public void StopMainAnimation()
  {
    if (AnimationInfo.MainAnimationData != null)
    {
      StopAnimation(AnimationInfo.MainAnimationData.Animation);
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
      StopMainAnimation();
      AnimationInfo.MainAnimationData = animationData;
      PlayMainAnimation();
      return;
    }

    if (EntityAnimationInfo.HaveSamePriority(animationData, AnimationInfo.MainAnimationData))
    {
      if (animationData.Animation == AnimationInfo.MainAnimationData.Animation)
      {
        return;
      }

      StopMainAnimation();
      AnimationInfo.MainAnimationData = animationData;
      ConnectOnFinishedToMain();

      PlayMainAnimation();
    }
  }

  public void ConnectOnFinishedToMain()
  {
    if (!AnimationInfo.MainAnimationData.Animation.IsConnected(AnimatedSprite2D.SignalName.AnimationFinished, CallOnMainAnimationFinish))
    {
      AnimationInfo.MainAnimationData.Animation.Connect(
       AnimatedSprite2D.SignalName.AnimationFinished,
       CallOnMainAnimationFinish
     );
    }
  }

  public void DisconnectOnFinishedFromMain()
  {
    if (AnimationInfo.MainAnimationData.Animation.IsConnected(AnimatedSprite2D.SignalName.AnimationFinished, CallOnMainAnimationFinish))
    {
      AnimationInfo.MainAnimationData.Animation.Disconnect(
       AnimatedSprite2D.SignalName.AnimationFinished,
       CallOnMainAnimationFinish
     );
    }
  }

  public void OnMainAnimationFinished()
  {
    DisconnectOnFinishedFromMain();
    StopMainAnimation();
    AnimationInfo.MainAnimationData = AnimationInfo.DefaultAnimationData;
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