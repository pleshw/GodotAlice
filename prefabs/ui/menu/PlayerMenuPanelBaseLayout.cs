using Godot;
using System;

namespace UI;

public partial class PlayerMenuPanelBaseLayout : Control
{
	[Export]
	public StringName Title;

	public Panel Panel;

	public Label TitleLabel;

	public MarginContainer Margin;
	public BoxContainer Container;
	public Panel Content;

	public override void _Ready()
	{
		base._Ready();
		Panel = GetNode<Panel>("Panel");
		Margin = Panel.GetNode<MarginContainer>("Margin");
		Container = Margin.GetNode<BoxContainer>("Container");
		TitleLabel = Container.GetNode<Label>("Title");
		Content = Container.GetNode<Panel>("Content");

		TitleLabel.Text = Title;
	}
}
