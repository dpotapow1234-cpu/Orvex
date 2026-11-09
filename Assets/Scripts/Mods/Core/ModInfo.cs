using System;

[Serializable]
public class ModInfo
{
    public string id;
    public string name;
    public string version;
    public string author;
    public string description;

    public string gameVersion;
    public string apiVersion;

    public string type;

    public string[] dependencies;

    // Точка входа для C# DLL
    public string entryPoint;
}