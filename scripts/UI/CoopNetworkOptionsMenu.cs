using System;
using Godot;
using Multiplayer;

namespace UI;

public partial class CoopNetworkOptionsMenu : Control
{
  private VBoxContainer _menuContainer;
  public VBoxContainer MenuContainer
  {
    get
    {
      _menuContainer ??= GetNode<VBoxContainer>("MenuContainer");
      return _menuContainer;
    }
  }

  private Button _hostGameButton;
  public Button HostGameButton
  {
    get
    {
      _hostGameButton ??= MenuContainer.GetNode<Button>("HostGameButton");
      return _hostGameButton;
    }
  }

  private Button _joinGameButton;
  public Button JoinGameButton
  {
    get
    {
      _joinGameButton ??= MenuContainer.GetNode<Button>("JoinGameButton");
      return _joinGameButton;
    }
  }

  public override void _Ready()
  {
    base._Ready();

    HostGameButton.Pressed += HostGameButtonPressedEvent;
    JoinGameButton.Pressed += JoinGameButtonPressedEvent;
  }

  public event Action OnHostGameButtonPressed;
  public void HostGameButtonPressedEvent()
  {
    OnHostGameButtonPressed?.Invoke();
  }

  public event Action OnJoinGameButtonPressed;
  public void JoinGameButtonPressedEvent()
  {
    OnJoinGameButtonPressed?.Invoke();
  }
}