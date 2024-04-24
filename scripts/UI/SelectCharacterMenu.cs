
using Entity;
using Extras;
using GameManager;
using Godot;

namespace UI;

public partial class SelectCharacterMenu : Control
{
  [Export]
  public TabContainer SpriteOptions;

  [Export]
  public Panel PlayerFrame;

  [Export]
  public Panel AttributesPanel;


  public PlayerManager PlayerManager
  {
    get
    {
      return GetNode<PlayerManager>("/root/PlayerManager");
    }
  }

  public AudioManager AudioManager
  {
    get
    {
      return GetNode<AudioManager>("/root/AudioManager");
    }
  }


  public SelectCharacterMenu()
  {

  }

  public override void _Ready()
  {
    base._Ready();
    CallDeferred(nameof(CreatePlayerModel));

    SpriteOptions.TabHovered += (long tab) =>
    {
      if (tab != SpriteOptions.CurrentTab)
      {
        AudioManager.PreloadedAudios["MenuHoverAction"].Play();
      }
    };

    SpriteOptions.TabChanged += (long tab) =>
    {
      AudioManager.PreloadedAudios["MenuMinorConfirm2"].Play();
    };
  }

  public void CreatePlayerModel()
  {
    Player playerInstance = PlayerManager.GetPlayerInstance(GodotFileName.MainCharacters.Pawn);
    playerInstance.Scale = Vector2.One * 3;

    Vector2 playerCenterPosition = (GetViewportRect().Size / 2) - (playerInstance.AnimatedBody.Size / 2);

    /// Removing the menu margin distance
    playerInstance.GlobalPosition += playerCenterPosition with { X = playerCenterPosition.X - 50, Y = playerCenterPosition.Y - 142 };

    AttributesPanel.AddChild(playerInstance);
  }
}