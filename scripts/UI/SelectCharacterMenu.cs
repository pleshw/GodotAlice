
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entity;
using Extras;
using GameManager;
using Godot;

namespace UI;

public class SpriteFileJson
{
  public required string[] SpriteList { get; set; }
}


public partial class SelectCharacterMenu : Control
{

  private string[] spriteList;

  private GameResourceManagerBase<Resource> SpritesResourceManager = new(GodotFolderPath.ItemResources);

  private GameNodeManagerBase<Button> FramesResourceManager = new();

  [Export]
  public TabContainer SpriteOptions;

  [Export]
  public Panel PlayerFrame;

  [Export]
  public Panel AttributesPanel;

  public Godot.Collections.Dictionary ItemList = [];



  [Export]
  public GridContainer GridHats;

  [Export]
  public GridContainer GridBody;

  [Export]
  public GridContainer GridPants;

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

  public CommonFilesManager CommonFilesManager
  {
    get
    {
      return GetNode<CommonFilesManager>("/root/CommonFilesManager");
    }
  }

  private Player playerInstance;

  public SelectCharacterMenu()
  {
    spriteList = CommonFilesManager.GetFileDeserialized<SpriteFileJson>("mainSprites.json").SpriteList;
    FramesResourceManager.Preload(["res://prefabs/items/custom_sprite_frame.tscn"]);
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

    CallDeferred(nameof(InstantiateSprites));
  }

  public void InstantiateSprites()
  {
    foreach (var sprite in spriteList)
    {
      Button customFrameButton = FramesResourceManager.CreateInstance<Button>("res://prefabs/items/custom_sprite_frame.tscn", "custom_sprite_frame" + Path.GetFileNameWithoutExtension(sprite));
      AnimatedSprite2D customFrameSprite = customFrameButton.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
      SpriteFrames spriteInstance = SpritesResourceManager.CreateInstance(sprite, sprite) as SpriteFrames;
      spriteInstance.ResourceLocalToScene = true;

      int frameSizeInPixels = 80;

      // Set the scale of the sprite
      customFrameButton.Size = Vector2.One * frameSizeInPixels;
      customFrameSprite.Centered = false;

      customFrameSprite.SpriteFrames = spriteInstance;
      customFrameSprite.ResizeUsingScale(customFrameButton.Size);
      customFrameSprite.Play("Idle");

      customFrameButton.Pressed += () =>
      {
        SpriteFrames spriteInstance2 = SpritesResourceManager.CreateInstance(sprite, "test" + sprite) as SpriteFrames;
        playerInstance.AnimatedBody.ChangePart("Hat", spriteInstance2);
      };

      GridHats.AddChild(customFrameButton);
    }
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

  public void CreatePlayerModel()
  {
    playerInstance = PlayerManager.GetPlayerInstance(GodotFileName.MainCharacters.Pawn);
    playerInstance.Scale = Vector2.One * 3;

    Vector2 playerCenterPosition = (GetViewportRect().Size / 2) - (playerInstance.AnimatedBody.Size / 2);

    /// Removing the menu margin distance
    playerInstance.GlobalPosition += playerCenterPosition with { X = playerCenterPosition.X - 50, Y = playerCenterPosition.Y - 142 };

    AttributesPanel.AddChild(playerInstance);
  }
}