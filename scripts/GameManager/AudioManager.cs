
using Godot;
using Extras;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GameManager;

public partial class AudioManager : Node
{
  public SceneManager SceneManager
  {
    get
    {
      return GetNode<SceneManager>("/root/SceneManager");
    }
  }

  private static List<StringName> MenuActionFilePaths
  {
    get
    {
      return [
        GodotFilePath.Sounds.Intro,
        GodotFilePath.Sounds.MenuConfirmAction,
        GodotFilePath.Sounds.MenuHoverAction,
        GodotFilePath.Sounds.MenuMinorConfirm,
        GodotFilePath.Sounds.MenuMinorConfirm2,
      ];
    }
  }

  public readonly Dictionary<StringName, AudioStreamPlayer> PreloadedAudios = [];

  public AudioStreamPlayer CreateAudioStreamPlayer(string filePath)
  {
    AudioStream audioStream = LoadAudioFromFile(filePath, out string fileName);
    return new()
    {
      Name = fileName,
      Stream = audioStream
    };
  }


  private AudioStream LoadAudioFromFile(string filePath, out string fileName)
  {
    // Load the audio stream from the file
    AudioStream audioStream = (ResourceLoader.Load<AudioStream>(filePath) ?? throw new System.Exception("Failed to load audio stream from file: " + filePath)) ?? throw new System.Exception("File not found: " + filePath);
    fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
    return audioStream;
  }

  public AudioManager() : base()
  {
    MenuActionFilePaths.ForEach(m =>
    {
      var streamPlayer = CreateAudioStreamPlayer(m);
      streamPlayer.VolumeDb -= 5;
      PreloadedAudios.Add(streamPlayer.Name, streamPlayer);
    });
  }

  public override void _Ready()
  {
    base._Ready();
    CallDeferred(nameof(AddPreloadedAudiosToRoot));
  }

  public void AddPreloadedAudiosToRoot()
  {
    foreach (var item in PreloadedAudios)
    {
      GetTree().Root.AddChild(item.Value);
    }

    AudioReadyEvent();
  }

  public event Action OnAudioReady;
  public void AudioReadyEvent()
  {
    OnAudioReady?.Invoke();
  }
}