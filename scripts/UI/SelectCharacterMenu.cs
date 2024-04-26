
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entity;
using Extras;
using GameManager;
using Godot;

namespace UI;


public class JsonSprite
{
  public required JsonSpriteInfo HatSprites { get; set; }
  public required JsonSpriteInfo ShirtSprites { get; set; }
  public required JsonSpriteInfo PantsSprites { get; set; }

  public void Deconstruct(out JsonSpriteInfo hatSprites, out JsonSpriteInfo shirtSprites, out JsonSpriteInfo pantsSprites)
  {
    hatSprites = HatSprites;
    shirtSprites = ShirtSprites;
    pantsSprites = PantsSprites;
  }

}


public class JsonSpriteInfo
{
  public required string DefaultSprite { get; set; }
  public required string[] SpriteList { get; set; }
}


public partial class SelectCharacterMenu : Control
{
  private readonly JsonSpriteInfo HatSprites;
  private readonly JsonSpriteInfo ShirtSprites;
  private readonly JsonSpriteInfo PantsSprites;

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
  public GridContainer GridShirts;

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
    (HatSprites, ShirtSprites, PantsSprites) = CommonFilesManager.GetFileDeserialized<JsonSprite>("mainSprites.json");
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
    SetupSpritesForGridContainer(HatSprites, GridHats, playerInstance.SpriteController.ChangeHat);
    SetupSpritesForGridContainer(ShirtSprites, GridShirts, playerInstance.SpriteController.ChangeShirt);
    SetupSpritesForGridContainer(PantsSprites, GridPants, playerInstance.SpriteController.ChangePants);
  }

  public void SetupSpritesForGridContainer(JsonSpriteInfo spriteInfo, GridContainer gridContainer, Action<SpriteFrames> changePartAction)
  {
    Button defaultHatSpriteButton = CreateSpriteShowcaseFrame(spriteInfo.DefaultSprite, () =>
    {
      SpriteFrames spriteInstance2 = SpritesResourceManager.CreateInstance(spriteInfo.DefaultSprite, playerInstance.Name + "ShowcaseSpriteFrame") as SpriteFrames;
      changePartAction(spriteInstance2);
    });

    defaultHatSpriteButton.MouseDefaultCursorShape = CursorShape.PointingHand;

    gridContainer.AddChild(defaultHatSpriteButton);

    foreach (var sprite in spriteInfo.SpriteList)
    {
      Button customFrameButton = CreateSpriteShowcaseFrame(sprite, () =>
      {
        SpriteFrames spriteInstance2 = SpritesResourceManager.CreateInstance(sprite, playerInstance.Name + "PartSpriteFrame") as SpriteFrames;
        changePartAction(spriteInstance2);
      });

      customFrameButton.MouseDefaultCursorShape = CursorShape.PointingHand;
      gridContainer.AddChild(customFrameButton);
    }
  }

  public Button CreateSpriteShowcaseFrame(string sprite, Action onPressed)
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
    customFrameSprite.Play("Showcase");

    customFrameButton.Pressed += () =>
    {
      onPressed();
    };

    return customFrameButton;
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