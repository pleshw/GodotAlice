using Godot;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class AnimatedBody : Node2D
{
	public StringName AnimationPlaying
	{
		get
		{
			return ReferenceSprite.Animation;
		}
	}

	/// <summary>
	/// Sprite that will be used to get animation names, frame count etc
	/// </summary>
	[Export]
	public AnimatedSprite2D ReferenceSprite { get; set; }

	private readonly List<AnimatedSprite2D> _sprites = [];

	public int Frame
	{
		get
		{
			return ReferenceSprite.Frame;
		}
	}

	public bool FlipH
	{
		get
		{
			return ReferenceSprite.FlipH;
		}

		set
		{
			_sprites.ForEach(s => s.FlipH = value);
		}
	}

	public float SpeedScale
	{
		get
		{
			return ReferenceSprite.SpeedScale;
		}

		set
		{
			_sprites.ForEach(s => s.SpeedScale = value);
		}
	}

	public int GetFrameCount(StringName animationName)
	{
		return ReferenceSprite.SpriteFrames.GetFrameCount(animationName);
	}

	public bool IsPlaying()
	{
		return ReferenceSprite.IsPlaying();
	}

	public void Play()
	{
		_sprites.ForEach(s => s.Play());
	}

	public void Play(StringName animationName)
	{
		_sprites.ForEach(s => s.Play(animationName));
	}

	public void Stop()
	{
		_sprites.ForEach(s => s.Stop());
	}
}
