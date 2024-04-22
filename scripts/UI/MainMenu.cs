using Extras;
using GameManager;
using Godot;
using Multiplayer;
using Scene;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI;

public partial class MainMenu : Control
{

	[Export]
	public MultiplayerController MultiplayerController;

	public GameResourceManager<CanvasItem> ResourceManager;

	public SceneManager SceneManager
	{
		get
		{
			return GetNode<SceneManager>("/root/SceneManager");
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

	private CoopCharacterMenu CoopCharacterMenuScene;
	private Control SingleCharacterMenuScene;
	private CoopNetworkOptionsMenu MultiplayerMenuScene;

	PackedScene CoopCharacterMenuPackedScene;
	PackedScene SingleCharacterMenuPackedScene;
	PackedScene MultiplayerConnectionMenuPackedScene;

	private List<CanvasItem> AllScenes
	{
		get
		{
			return [this, SingleCharacterMenuScene, MultiplayerMenuScene, CoopCharacterMenuScene];
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
		SceneManager.Preload([GodotFilePath.Menus.SingleCharacterMenu, GodotFilePath.Menus.CoopCharacterMenu, GodotFilePath.Menus.MultiplayerConnectionMenu]);
	}

	public override void _Ready()
	{
		base._Ready();

		GetWindow().GrabFocus();

		SingleCharacterMenuScene = SceneManager.CreateInstance<Control>(GodotFileName.Menus.SingleCharacterMenu, "SingleCharacterMenu");
		MultiplayerMenuScene = SceneManager.CreateInstance<CoopNetworkOptionsMenu>(GodotFileName.Menus.MultiplayerConnectionMenu, "MultiplayerConnectionMenu");
		CoopCharacterMenuScene = SceneManager.CreateInstance<CoopCharacterMenu>(GodotFileName.Menus.CoopCharacterMenu, "CoopCharacterMenu");

		SceneManager.AddScenesToRootDeferred();

		HideScenes();
		SwitchToScene(this);
		SetButtonEvents();
		SetMultiplayerEvents();
	}

	private void SetMultiplayerEvents()
	{
		MultiplayerController.OnLobbyHosted += () =>
		{
			CoopCharacterMenuScene.IsHost = true;
			SwitchToScene(CoopCharacterMenuScene);
		};
	}

	public void SetButtonEvents()
	{
		QuitGameButton.Pressed += QuitEvent;
		SingleGameButton.Pressed += () => SwitchToScene(SingleCharacterMenuScene);
		CoopGameButton.Pressed += () => SwitchToScene(MultiplayerMenuScene);

		MultiplayerMenuScene.OnHostGameButtonPressed += () =>
		{
			if (MultiplayerController.TryCreateLobby(out Error err))
			{
				GD.Print("Lobby created");
			}
			else
			{
				GD.Print("Failed to create new lobby: ", err.ToString());
			}
		};

		AllButtons.ForEach(b => b.Pressed += () => b.ReleaseFocus());
	}

	public void LoadMultiplayerMenuScene()
	{
		HideScenes();
		MultiplayerMenuScene.Visible = true;
	}

	public void LoadCoopMenuScene()
	{
		HideScenes();
		MultiplayerMenuScene.Visible = true;
	}

	private void AddScenesToRoot()
	{
		AllScenes.ForEach(s =>
		{
			if (s.GetParent() != GetTree().Root)
			{
				GetTree().Root.AddChild(s);
			}
		});
	}

	private void HideScenes()
	{
		AllScenes.ForEach(s => s.Visible = false);
	}

	private void SwitchToScene(CanvasItem scene)
	{
		HideScenes();
		scene.Visible = true;
	}

	private void QuitEvent()
	{
		GetTree().Quit();
	}
}
