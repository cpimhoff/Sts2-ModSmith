namespace ModSmith.Util;

/// <summary>
/// Helper class for constructing Godot resource paths.
/// </summary>
public class ResourcePaths(string modId)
{
  private readonly string ModId = modId;

  /// <summary>
  /// Construct a Godot resource path relative to the current mod's root
  /// directory. Use to reference resources packaged with your mod.
  /// </summary>
  public string Mod(string path)
  {
    return $"res://{ModId}/{path}";
  }

  /// <summary>
  /// Construct a Godot resource path from the global resource root.
  /// Use to reference resources which are packaged with Slay the Spire 2.
  /// </summary>
  public string Global(string path)
  {
    return $"res://{path}";
  }

  /// <summary>
  /// Construct a Godot resource path relative to the ModSmith mod's root
  /// directory. Use to reference resources packaged with ModSmith, such
  /// as default images.
  ///
  /// A complete index of available resources is available in the ModSmith
  /// repository: https://github.com/cpimhoff/Sts2-ModSmith/tree/main/ModSmith/ModSmith
  /// </summary>
  public string ModSmith(string path)
  {
    return $"res://ModSmith/{path}";
  }
}
