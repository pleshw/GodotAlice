using Godot;
using System;

namespace UI;

public partial class CharacterSelectButton : Button
{
	private bool _selected = false;
	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			_selected = value;
			SetGrayscaleShader(!_selected);
		}
	}

	[Export]
	public MarginContainer MarginContainer;

	[Export]
	public VBoxContainer VBoxContainer;

	[Export]
	public Label TitleLabel;

	[Export]
	public Control CharacterSpriteControl;

	[Export]
	public AnimatedSprite2D CharacterSprite;

	public override void _Ready()
	{
		base._Ready();

		(CharacterSprite.Material as ShaderMaterial).SetShaderParameter("active", true);

		Pressed += () =>
		{
			ReleaseFocus();
			Selected = !Selected;
		};

		CharacterSprite.Stop();

		MouseEntered += () =>
		{
			if (!Selected)
			{
				SetGrayscaleShader(false);
			}
		};

		MouseExited += () =>
		{
			if (!Selected)
			{
				SetGrayscaleShader(true);
			}
		};
	}

	public void SetGrayscaleShader(bool state)
	{
		(CharacterSprite.Material as ShaderMaterial).SetShaderParameter("active", state);
		CharacterSprite.Play("default");
	}
}
