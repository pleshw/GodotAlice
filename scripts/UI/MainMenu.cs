using Extras;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI;

public partial class MainMenu : Control
{
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


	private Button _newGameButton;
	public Button NewGameButton
	{
		get
		{
			_newGameButton ??= FirstMenu.GetNode<Button>("NewGameButton");
			return _newGameButton;
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

	private Control NewCharacterScene;

	private List<CanvasItem> AllScenes
	{
		get
		{
			return [(NewCharacterScene as CanvasItem),];
		}
	}

	public override void _Ready()
	{
		base._Ready();
		NewCharacterScene = ResourceLoader.Load<PackedScene>(GodotFilePath.CreateCharacterMenu).Instantiate() as Control;
		SetButtonEvents();
	}

	public void SetButtonEvents()
	{
		QuitGameButton.Pressed += QuitEvent;
		NewGameButton.Pressed += LoadNewCharacterScene;
	}

	public void LoadNewCharacterScene()
	{
		Visible = false;
		AllScenes.ForEach(s => s.Visible = false);
		NewCharacterScene.Visible = true;
		GetTree().Root.AddChild(NewCharacterScene);
	}

	void QuitEvent()
	{
		GetTree().Quit();
	}
}
