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

	private List<Button> AllButtons
	{
		get
		{
			return [LoadGameButton, SingleGameButton, CoopGameButton, QuitGameButton];
		}
	}

	public MainMenu()
	{
	}

	public override void _Ready()
	{
		base._Ready();
		SceneManager.Preload([GodotFilePath.Menus.SingleCharacterMenu, GodotFilePath.Menus.CoopCharacterMenu, GodotFilePath.Menus.MultiplayerConnectionMenu]);

		GetWindow().GrabFocus();

		SingleCharacterMenuScene = SceneManager.CreateInstance<Control>(GodotFilePath.Menus.SingleCharacterMenu, "SingleCharacterMenu");
		MultiplayerMenuScene = SceneManager.CreateInstance<CoopNetworkOptionsMenu>(GodotFilePath.Menus.MultiplayerConnectionMenu, "MultiplayerConnectionMenu");
		CoopCharacterMenuScene = SceneManager.CreateInstance<CoopCharacterMenu>(GodotFilePath.Menus.CoopCharacterMenu, "CoopCharacterMenu");

		SceneManager.AddScenesToRootDeferred();

		SceneManager.SetScene(this);
		SetButtonEvents();
		SetMultiplayerEvents();
	}

	private void SetMultiplayerEvents()
	{
		MultiplayerController.OnLobbyHosted += () =>
		{
			CoopCharacterMenuScene.IsHost = true;
			SceneManager.SetScene(CoopCharacterMenuScene);
		};
	}

	public void SetButtonEvents()
	{
		QuitGameButton.Pressed += QuitEvent;
		SingleGameButton.Pressed += () => SceneManager.SetScene(SingleCharacterMenuScene);
		CoopGameButton.Pressed += () => SceneManager.SetScene(MultiplayerMenuScene);

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

	private void QuitEvent()
	{
		GetTree().Quit();
	}
}
