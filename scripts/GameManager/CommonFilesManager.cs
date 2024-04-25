using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Godot;

namespace GameManager;

public partial class CommonFilesManager : Node
{
  public static string UserData
  {
    get
    {
      return ProjectSettings.GlobalizePath("user://");
    }
  }

  public static void CreateFile(string fileName, string fileData)
  {
    try
    {
      File.WriteAllText(Path.Join(UserData, fileName), fileData);
    }
    catch (System.Exception err)
    {
      GD.Print(err);
    }
  }

  public static void CreateFile<T>(string fileName, T fileData)
  {
    CreateFile(fileName, JsonSerializer.Serialize(fileData));
  }

  public static T DeserializeFromFile<T>(string filePath)
  {
    T data;

    if (!File.Exists(filePath))
    {
      GD.Print("File not found: " + filePath);
      return default;
    }

    try
    {
      string fileContent = File.ReadAllText(filePath);
      data = JsonSerializer.Deserialize<T>(fileContent);
      return data;
    }
    catch (System.Exception ex)
    {
      GD.Print("Error reading file: " + ex.Message);
      return default;
    }
  }

}