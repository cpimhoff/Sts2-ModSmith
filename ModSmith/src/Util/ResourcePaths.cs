namespace Cpimhoff.Sts2.ModSmith.Util;

public class ResourcePaths(string modId)
{
  private readonly string ModId = modId;

  public string Mod(string path)
  {
    return $"res://{ModId}/{path}";
  }

  public string Global(string path)
  {
    return $"res://{path}";
  }

  public string ModSmith(string path)
  {
    return $"res://ModSmith/{path}";
  }
}
