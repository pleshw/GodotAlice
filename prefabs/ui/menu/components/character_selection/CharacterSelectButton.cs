using Entity;
using Godot;
using System;
using System.Collections.Generic;

namespace UI;

public partial class CharacterSelectButton : Button
{
	[Export]
	public Player Player;


	[Export]
	public MarginContainer MarginContainer;

	[Export]
	public VBoxContainer VBoxContainer;

	[Export]
	public Label TitleLabel;

	[Export]
	public Control CharacterSpriteControl;

	public EntityAnimatedBody CharacterSprite;

	public Control CharacterSpriteContainer
	{
		get
		{
			return Player.GetParent<Control>();
		}
	}

	public override void _Ready()
	{
		base._Ready();

		CharacterSprite = Player.AnimatedBody;

		Pressed += () =>
		{
			ReleaseFocus();
		};

		CharacterSprite.Freeze = true;
		CharacterSprite.Stop();

		MouseEntered += () =>
		{
			SetGrayscaleShader(false);
		};

		MouseExited += () =>
		{
			SetGrayscaleShader(true);
			CharacterSprite.Freeze = true;
			CharacterSprite.Stop();
			GD.Print("Stopped");
		};
	}

	public void SetGrayscaleShader(bool grayScaleActive)
	{
		ShaderMaterial shaderMaterial = CharacterSpriteContainer.Material as ShaderMaterial;
		shaderMaterial.SetShaderParameter("active", grayScaleActive);
		if (!grayScaleActive)
		{
			Player.PlayAttackAnimation();
			GD.Print("played");
		}
	}

	public event Action OnSelectedByPlayer;
	public void SelectedByPlayerEvent()
	{
		OnSelectedByPlayer?.Invoke();
	}

	public event Action OnQuitLobby;
	public void QuitLobbyEvent()
	{
		OnQuitLobby?.Invoke();
	}
}
