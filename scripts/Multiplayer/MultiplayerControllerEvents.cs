
using System;

namespace Multiplayer;

public partial class MultiplayerController
{
  public event Action OnLobbyHosted;
  public void LobbyHostedEvent()
  {
    OnLobbyHosted?.Invoke();
  }

  public event Action OnLobbyJoined;
  public void LobbyJoinedEvent()
  {
    OnLobbyJoined?.Invoke();
  }

  public event Action OnQuitLobby;
  public void QuitLobbyEvent()
  {
    OnQuitLobby?.Invoke();
  }
}