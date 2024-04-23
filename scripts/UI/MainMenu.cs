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

	public GameResourceManager<CanvasItem> ResourceManager;

	public AudioManager AudioManager
	{
		get
		{
			return GetNode<AudioManager>("/root/AudioManager");
		}
	}

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

	private Button _backSceneButton;
	public Button BackSceneButton
	{
		get
		{
			_backSceneButton ??= GetNode<Button>("BackSceneButton");
			return _backSceneButton;
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

	private List<Button> AllButtons
	{
		get
		{
			return [LoadGameButton, SingleGameButton, QuitGameButton];
		}
	}

	public MainMenu()
	{
	}

	public override void _Ready()
	{
		base._Ready();
		SceneManager.CurrentScene = this;
		SceneManager.Preload([GodotFilePath.Menus.SingleCharacterMenu]);

		GetWindow().GrabFocus();

		SingleCharacterMenuScene = SceneManager.CreateInstance<Control>(GodotFilePath.Menus.SingleCharacterMenu, "SingleCharacterMenu");

		SceneManager.AddScenesToRootDeferred();
		AudioManager.AddScenesToRootDeferred();

		SceneManager.SetScene(this);

		SceneManager.OnSceneStackChange += () =>
		{
			if (SceneManager.SceneStack.Count > 1)
			{
				BackSceneButton.Show();
			}
			else
			{
				BackSceneButton.Hide();
			}
		};

		SetButtonEvents();
	}

	public void SetButtonEvents()
	{
		QuitGameButton.Pressed += QuitEvent;
		SingleGameButton.Pressed += () => SceneManager.SetScene(SingleCharacterMenuScene);

		AllButtons.ForEach(b => b.Pressed += () =>
		{
			if (!b.Disabled)
			{
				AudioManager["MenuButtonConfirm"].Play();
			}

			b.ReleaseFocus();
		});

		AllButtons.ForEach(b => b.MouseEntered += () =>
		{
			if (!b.Disabled)
			{
				AudioManager["MenuButtonHover"].Play();
			}
		});
	}

	private void QuitEvent()
	{
		GetTree().Quit();
	}
}
