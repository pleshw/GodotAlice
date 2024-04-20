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
}
