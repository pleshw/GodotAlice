
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

  public Godot.Collections.Dictionary ItemList = [];


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

  public SaveFilesManager SaveFilesManager
  {
    get
    {
      return GetNode<SaveFilesManager>("/root/SaveFilesManager");
    }
  }


  public SelectCharacterMenu()
  {

  }

  public void ConfirmCharacterCreation()
  {
    SaveFilesManager.CreateNewPlayerSaveFile(System.Text.Json.JsonSerializer.Serialize<PlayerSaveData>(new()
    {
      Name = "",
      Location = "none",
      AttributePointsByName = new Dictionary<string, int>
      {
        ["Agility"] = 2,
        ["Vitality"] = 8
      },
      SpriteFramesByPosition = new Dictionary<string, string>
      {
        ["Body"] = "",
        ["Hat"] = "test_item.res"
      },
    }));
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