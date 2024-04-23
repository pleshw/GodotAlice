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
	public StringName CharacterName;

	[Export]
	public Label TitleLabel;


	public MainMenu MainMenu
	{
		get
		{
			return GetTree().Root.GetNode<MainMenu>("MainMenu");
		}
	}

	public override void _Ready()
	{
		base._Ready();

		TitleLabel.Text = CharacterName;

		Pressed += () =>
		{
			ReleaseFocus();
		};
	}
}
