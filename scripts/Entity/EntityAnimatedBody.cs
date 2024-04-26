using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity;

public delegate void OnFrameChangeEvent(AnimatedSprite2D animatedSprite, Transform2D initialTransform, int currentFrame, int animationFrameCount);
public delegate void OnAnimationFinishedEvent(AnimatedSprite2D animatedSprite, Transform2D initialTransform);

public struct AnimationRequestInput()
{
	public required StringName Name;
	public float ForceDuration = -1;
	public OnFrameChangeEvent OnFrameChange = null;
	public OnAnimationFinishedEvent OnFinished = null;
}

public partial class EntityAnimatedBody : Node2D
{
	/// <summary>
	/// The sprite with the most number of frames. Used as reference to know if the animation have to stop.
	/// </summary>
	public AnimatedSprite2D SpriteReference;

	public Dictionary<StringName, AnimatedSprite2D> PartsByName = [];

	public List<AnimatedSprite2D> Parts;

	public string AnimationPlaying = "";

	private bool _freeze = false;
	public bool Freeze
	{
		get
		{
			return _freeze;
		}
		set
		{
			Parts.ForEach(p => p.EmitSignal(AnimatedSprite2D.SignalName.AnimationFinished));
			Stop();
			FreezeEvent();
			_freeze = value;
		}
	}

	public Vector2 Size
	{
		get
		{
			return GetChildren().OfType<AnimatedSprite2D>().First().SpriteFrames.GetFrameTexture("Idle", 0).GetSize();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		Parts = GetChildren().OfType<AnimatedSprite2D>().ToList();
		Parts.ForEach(bp => PartsByName.Add(bp.Name, bp));
		SpriteReference = Parts.OrderByDescending(e => e.Frame).FirstOrDefault();
	}

	public void ChangePart(StringName partName, SpriteFrames newSprite)
	{
		Stop();

		List<AnimatedSprite2D> selectedParts = Parts.Where(p => p.Name == partName).ToList();
		selectedParts.ForEach(p =>
		{
			newSprite.ResourceName = Name + partName + "SpriteFrames";
			p.SpriteFrames = newSprite;
			p.Play();
		});
	}


	public void Play(StringName animationName)
	{
		Freeze = false;
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

			animatedSprite.GetParent<Node2D>().Visible = true;
			animatedSprite.Visible = true;

			AnimationPlaying = animationName;
			animatedSprite.Play(animationName);
		});
	}

	public void Play(AnimationRequestInput animationRequest)
	{
		Freeze = false;
		Parts.ForEach(animatedSprite =>
		{
			SpriteFrames spriteFrames = animatedSprite.SpriteFrames;

			if (animatedSprite.SpriteFrames is null)
			{
				return;
			}

			List<string> animationNames = [.. animatedSprite.SpriteFrames.GetAnimationNames()];

			if (!animationNames.Contains(animationRequest.Name))
			{
				return;
			}

			animatedSprite.GetParent<Node2D>().Visible = true;
			animatedSprite.Visible = true;

			Transform2D initialTransform = animatedSprite.Transform;

			int animationFrameCount = spriteFrames.GetFrameCount(animationRequest.Name);
			float defaultAnimationSpeed = animatedSprite.SpeedScale;
			double defaultAnimationFPS = spriteFrames.GetAnimationSpeed(animationRequest.Name);

			if (animationRequest.ForceDuration > 0)
			{
				double animationDuration = animationFrameCount / defaultAnimationFPS;
				double speedScale = animationDuration / animationRequest.ForceDuration;
				animatedSprite.SpeedScale = (float)speedScale;
			}

			void _onAnimationProgress() => ExecuteActionOnFrame(animatedSprite, initialTransform, animationFrameCount, animationRequest.OnFrameChange);

			void _onAnimationFinished()
			{
				animatedSprite.Disconnect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(_onAnimationProgress));
				animatedSprite.Disconnect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(_onAnimationFinished));
				animationRequest.OnFinished?.Invoke(animatedSprite, initialTransform);

				if (animationRequest.ForceDuration > 0)
				{
					animatedSprite.SpeedScale = defaultAnimationSpeed;
				}
			};

			animatedSprite.Connect(AnimatedSprite2D.SignalName.FrameChanged, Callable.From(_onAnimationProgress));
			animatedSprite.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(_onAnimationFinished));

			AnimationPlaying = animationRequest.Name;
			animatedSprite.Play(animationRequest.Name);
		});
	}

	public static void ExecuteActionOnFrame(AnimatedSprite2D animatedSprite, Transform2D initialTransform, int animationFrameCount, OnFrameChangeEvent onFrameChange)
	{
		if (onFrameChange is null)
		{
			return;
		}

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
