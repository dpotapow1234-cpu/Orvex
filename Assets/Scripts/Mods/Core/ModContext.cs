using System.Collections;
using UnityEngine;

using System.IO;

public class ModContext
{
    public ModInfo Info { get; private set; }

    public ModRegistry Registry { get; private set; }

    public string ModDirectory { get; private set; }

    public string AssetsDirectory =>
        Path.Combine(ModDirectory, "Assets");

    public ModContext(
        ModInfo info,
        ModRegistry registry,
        string directory)
    {
        Info = info;
        Registry = registry;
        ModDirectory = directory;
    }

    public void Log(string message)
    {
        UnityEngine.Debug.Log(
            $"[Mod: {Info.id}] {message}"
        );
    }
}