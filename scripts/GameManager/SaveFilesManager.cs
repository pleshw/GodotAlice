using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;

namespace GameManager;

public partial class SaveFilesManager : Node
{
  public static string UserData
  {
    get
    {
      return ProjectSettings.GlobalizePath("user://");
    }
  }

  public static string UserSavesDirectory
  {
    get
    {
      return ProjectSettings.GlobalizePath("user://player/");
    }
  }

  public static void CreateNewPlayerSaveFile(string playerData)
  {
    EnsureDirectoryExists();

    string lastSaveFile = GetLastPlayerSaveFile();

    string newSaveFileName = lastSaveFile != null ? $"saveFile{GetSaveFileNumber(lastSaveFile) + 1}.json" : "saveFile0.json";

    try
    {
      File.WriteAllText(Path.Join(UserSavesDirectory, newSaveFileName), playerData);
    }
    catch (System.Exception err)
    {
      GD.Print(err);
    }
  }

  private static void EnsureDirectoryExists()
  {
    if (!Directory.Exists(UserSavesDirectory))
    {
      Directory.CreateDirectory(UserSavesDirectory);
    }
  }

  static int GetSaveFileNumber(string filename)
  {
    string numberPart = MyRegex().Match(filename).Value;
    if (int.TryParse(numberPart, out int saveNumber))
    {
      return saveNumber;
    }
    else
    {
      return -1; // Indicates failure to parse
    }
  }

  private static string GetLastPlayerSaveFile()
  {
    string[] saveFiles = Directory.GetFiles(UserSavesDirectory, "saveFile*");

    return saveFiles.OrderByDescending(f => new FileInfo(f).LastWriteTime).FirstOrDefault();
  }

  [GeneratedRegex(@"\d+")]
  private static partial Regex MyRegex();
}