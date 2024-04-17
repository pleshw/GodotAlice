using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity;

public delegate void OnFrameChangeEvent(AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount);
public delegate void OnAnimationFinishedEvent(AnimatedSprite2D animatedSprite, Transform2D initialTransform);

public partial class EntityAnimatedBody : Node2D
{

	/// <summary>
	/// The sprite with the most number of frames. Used as reference to know if the animation have to stop.
	/// </summary>
	public AnimatedSprite2D SpriteReference;

	public Dictionary<StringName, AnimatedSprite2D> PartsByName = [];

	public List<AnimatedSprite2D> Parts;

	public string AnimationPlaying = "";

	public override void _Ready()
	{
		base._Ready();
		Parts = GetChildren().OfType<AnimatedSprite2D>().ToList();
		Parts.ForEach(bp => PartsByName.Add(bp.Name, bp));
		SpriteReference = Parts.OrderByDescending(e => e.Frame).FirstOrDefault();
	}

	public void Play(StringName animationName)
	{
		Parts.ForEach(animatedSprite =>
		{
			SpriteFrames spriteFrames = animatedSprite.SpriteFrames;
			if (animatedSprite.SpriteFrames is null)
			{
				return;
			}

			List<string> animationNames = [.. animatedSprite.SpriteFrames.GetAnimationNames()];

			if (!animationNames.Contains(animationName))
			{
				return;
			}

			AnimationPlaying = animationName;
			animatedSprite.Play(animationName);
		});
	}

	public void Play(StringName animationName, OnFrameChangeEvent onAnimationProgress, OnAnimationFinishedEvent onAnimationFinished = null)
	{
		Parts.ForEach(animatedSprite =>
		{
			SpriteFrames spriteFrames = animatedSprite.SpriteFrames;

			if (animatedSprite.SpriteFrames is null)
			{
				return;
			}

			List<string> animationNames = [.. animatedSprite.SpriteFrames.GetAnimationNames()];

			if (!animationNames.Contains(animationName))
			{
				return;
			}

			Transform2D initialTransform = animatedSprite.Transform;

			int animationFrameCount = spriteFrames.GetFrameCount(animationName);

			void _onAnimationProgress() => ExecuteActionOnFrame(animatedSprite, initialTransform, animationFrameCount, onAnimationProgress);

			void _onAnimationFinished()
			{
				animatedSprite.Disconnect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(_onAnimationProgress));
				animatedSprite.Disconnect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(_onAnimationFinished));
				onAnimationFinished?.Invoke(animatedSprite, initialTransform);
			};

			animatedSprite.Connect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(_onAnimationProgress));
			animatedSprite.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(_onAnimationFinished));

			AnimationPlaying = animationName;
			animatedSprite.Play(animationName);
		});
	}

	public static void ExecuteActionOnFrame(AnimatedSprite2D animatedSprite, Transform2D initialTransform, int animationFrameCount, OnFrameChangeEvent onFrameChange)
	{
		int currentFrame = animatedSprite.Frame;
		onFrameChange(animatedSprite, initialTransform, currentFrame, animationFrameCount);
	}

	public void Stop()
	{
		Parts.ForEach(animatedSprite =>
		{
			SpriteFrames spriteFrames = animatedSprite.SpriteFrames;
			if (animatedSprite.SpriteFrames is null)
			{
				return;
			}

			animatedSprite.Stop();
		});
	}

	public bool IsPlaying()
	{
		return SpriteReference.IsPlaying();
	}


	public new void EmitSignal(StringName signalName, params Variant[] signalParams)
	{
		Parts.ForEach(animatedSprite =>
		{
			SpriteFrames spriteFrames = animatedSprite.SpriteFrames;
			if (animatedSprite.SpriteFrames is null)
			{
				return;
			}

			animatedSprite.EmitSignal(signalName, signalParams);
		});
	}

	public void SetSpeedScale(float scale)
	{
		Parts.ForEach(animatedSprite =>
		{
			SpriteFrames spriteFrames = animatedSprite.SpriteFrames;
			if (animatedSprite.SpriteFrames is null)
			{
				return;
			}

			animatedSprite.SpeedScale = scale;
		});
	}
}
