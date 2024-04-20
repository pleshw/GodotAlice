using Godot;
using System;

namespace UI;

public partial class CharacterSelectButton : Button
{
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

		Pressed += ReleaseFocus;
		CharacterSprite.Stop();
		MouseEntered += () =>
		{
			(CharacterSprite.Material as ShaderMaterial).SetShaderParameter("active", false);
			CharacterSprite.Play("default");
		};

		MouseExited += () =>
		{
			(CharacterSprite.Material as ShaderMaterial).SetShaderParameter("active", true);
			CharacterSprite.Stop();
		};
	}
}
