using Godot;
using System;

namespace UI;

public partial class PlayerMenuPanelEquipment : Control
{
	[Export]
	public StringName Title;

	public Panel Panel;

	public Label TitleLabel;

	public override void _Ready()
	{
		base._Ready();
		Panel = GetNode<Panel>("Panel");
		TitleLabel = Panel.GetNode<Label>("Title");

		TitleLabel.Text = Title;
	}
}
