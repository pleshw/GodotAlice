using Extras;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI;

public partial class MainMenu : Control
{

	private ResourcePreloader _preloader;
	public ResourcePreloader Preloader
	{
		get
		{
			_preloader ??= new();

			return _preloader;
		}
	}

	private Control _firstMenu;
	public Control FirstMenu
	{
		get
		{
			_firstMenu ??= GetNode<VBoxContainer>("FirstMenu");
			return _firstMenu;
		}
	}

	private Button _loadGameButton;
	public Button LoadGameButton
	{
		get
		{
			_loadGameButton ??= FirstMenu.GetNode<Button>("LoadGameButton");
			return _loadGameButton;
		}
	}

	private Button _singleGameButton;
	public Button SingleGameButton
	{
		get
		{
			_singleGameButton ??= FirstMenu.GetNode<Button>("NewGameButton");
			return _singleGameButton;
		}
	}

	private Button _coopGameButton;
	public Button CoopGameButton
	{
		get
		{
			_coopGameButton ??= FirstMenu.GetNode<Button>("CoopGameButton");
			return _coopGameButton;
		}
	}


	private Button _quitGameButton;
	public Button QuitGameButton
	{
		get
		{
			_quitGameButton ??= FirstMenu.GetNode<Button>("QuitGameButton");
			return _quitGameButton;
		}
	}

	private Control SingleCharacterMenuScene;
	private Control CoopCharacterMenuScene;

	PackedScene SingleCharacterMenuPackedScene;
	PackedScene CoopCharacterMenuPackedScene;

	private List<CanvasItem> AllScenes
	{
		get
		{
			return [this, SingleCharacterMenuScene, CoopCharacterMenuScene];
		}
	}

	private List<Button> AllButtons
	{
		get
		{
			return [LoadGameButton, SingleGameButton, CoopGameButton, QuitGameButton];
		}
	}

	public MainMenu()
	{
		SingleCharacterMenuPackedScene = ResourceLoader.Load<PackedScene>(GodotFilePath.SingleCharacterMenu);
		CoopCharacterMenuPackedScene = ResourceLoader.Load<PackedScene>(GodotFilePath.CoopCharacterMenu);

		Preloader.AddResource(GodotFilePath.SingleCharacterMenu, SingleCharacterMenuPackedScene);
		Preloader.AddResource(GodotFilePath.CoopCharacterMenu, CoopCharacterMenuPackedScene);
	}

	public override void _Ready()
	{
		base._Ready();
		SingleCharacterMenuScene = (Preloader.GetResource(GodotFilePath.SingleCharacterMenu) as PackedScene).Instantiate() as Control;
		CoopCharacterMenuScene = (Preloader.GetResource(GodotFilePath.CoopCharacterMenu) as PackedScene).Instantiate() as Control;
		SetButtonEvents();
	}

	public void SetButtonEvents()
	{
		QuitGameButton.Pressed += QuitEvent;
		SingleGameButton.Pressed += LoadSingleCharacterMenuScene;
		CoopGameButton.Pressed += LoadCoopCharacterMenuScene;
		AllButtons.ForEach(b => b.Pressed += () => b.ReleaseFocus());
	}

	public void LoadSingleCharacterMenuScene()
	{
		AllScenes.ForEach(s => s.Visible = false);
		SingleCharacterMenuScene.Visible = true;
		GetTree().Root.AddChild(SingleCharacterMenuScene);
	}

	public void LoadCoopCharacterMenuScene()
	{
		AllScenes.ForEach(s => s.Visible = false);
		CoopCharacterMenuScene.Visible = true;
		GetTree().Root.AddChild(CoopCharacterMenuScene);
	}

	void QuitEvent()
	{
		GetTree().Quit();
	}
}
