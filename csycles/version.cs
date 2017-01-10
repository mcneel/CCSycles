using System.Reflection;

[assembly: AssemblyVersion(RhinoBuildConstants.VERSION_STRING)]
[assembly: AssemblyFileVersion(RhinoBuildConstants.VERSION_STRING)]
[assembly: AssemblyInformationalVersion("Rhino " + RhinoBuildConstants.MAJOR_MINOR_VERSION_STRING)]

static class RhinoBuildConstants
{
  public const string VERSION_STRING = "1.0.0.01000";
  public const string MAJOR_MINOR_VERSION_STRING = "1.0";
}

