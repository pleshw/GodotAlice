using Entity;
using Extras;
using Godot;
using Scene;
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

		CallDeferred(nameof(SetPlayerCards));

		Pressed += () =>
		{
			ReleaseFocus();
		};


		MouseEntered += () =>
		{
			SetGrayscaleShader(false);
		};

		MouseExited += () =>
		{
			SetGrayscaleShader(true);
			CharacterSprite.Freeze = true;
			CharacterSprite.Stop();
		};
	}

	private void SetPlayerCards()
	{
		StageLoader stageLoader = GetTree().Root.GetNode<MainMenu>("MainMenu").StageLoader;
		Player = stageLoader.PlayerManager.InstantiatePlayerByName(GodotFileName.MainCharacters.Pawn, stageLoader);
		Player.UseParentMaterial = true;

		CharacterSprite = Player.AnimatedBody;
		CharacterSprite.Ready += () =>
		{
			CharacterSprite.Freeze = true;
			CharacterSprite.Stop();

			CharacterSpriteControl.AddChild(Player);
		};
	}

	public void SetGrayscaleShader(bool grayScaleActive)
	{
		ShaderMaterial shaderMaterial = CharacterSpriteContainer.Material as ShaderMaterial;
		shaderMaterial.SetShaderParameter("active", grayScaleActive);
		if (!grayScaleActive)
		{
			Player.PlayAttackAnimation();
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
