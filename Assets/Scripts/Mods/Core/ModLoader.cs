using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class ModLoader : MonoBehaviour
{
    public static ModLoader Instance { get; private set; }

    public ModRegistry Registry { get; private set; }

    public string ModsPath =>
        Path.Combine(Application.persistentDataPath, "Mods");

    private readonly List<IOrvexMod> loadedCodeMods =
        new List<IOrvexMod>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Registry = new ModRegistry();

        LoadMods();
    }

    public void LoadMods()
    {
        if (!Directory.Exists(ModsPath))
            Directory.CreateDirectory(ModsPath);

        string[] directories = Directory.GetDirectories(ModsPath);

        foreach (string directory in directories)
            LoadMod(directory);

        Debug.Log(
            $"[Orvex Mods] Loaded {loadedCodeMods.Count} code mods"
        );
    }

    private void LoadMod(string directory)
    {
        try
        {
            string manifest = Path.Combine(directory, "mod.json");

            if (!File.Exists(manifest))
                return;

            ModInfo info = JsonUtility.FromJson<ModInfo>(
                File.ReadAllText(manifest)
            );

            if (info == null ||
                string.IsNullOrWhiteSpace(info.id) ||
                string.IsNullOrWhiteSpace(info.name))
            {
                Debug.LogWarning("[Mods] Invalid manifest: " + directory);
                return;
            }

            ModContext context = new ModContext(
                info,
                Registry,
                directory
            );

            LoadData(info, directory);

            if (info.type == "hybrid" || info.type == "code")
                LoadCode(info, directory, context);

            Debug.Log("[Mods] Loaded: " + info.name);
        }
        catch (Exception ex)
        {
            Debug.LogError(
                $"[Mods] Error loading {directory}: {ex}"
            );
        }
    }

    private void LoadData(ModInfo info, string directory)
    {
        string blocksPath = Path.Combine(directory, "blocks.json");

        if (File.Exists(blocksPath))
        {
            ModBlockCollection collection =
                JsonUtility.FromJson<ModBlockCollection>(
                    File.ReadAllText(blocksPath)
                );

            if (collection?.blocks != null)
            {
                foreach (ModBlock block in collection.blocks)
                    Registry.RegisterBlock(info.id, block);
            }
        }

        string itemsPath = Path.Combine(directory, "items.json");

        if (File.Exists(itemsPath))
        {
            ModItemCollection collection =
                JsonUtility.FromJson<ModItemCollection>(
                    File.ReadAllText(itemsPath)
                );

            if (collection?.items != null)
            {
                foreach (ModItem item in collection.items)
                    Registry.RegisterItem(info.id, item);
            }
        }
    }

    private void LoadCode(
        ModInfo info,
        string directory,
        ModContext context)
    {
        string dllPath = Path.Combine(
            directory,
            info.name + ".dll"
        );

        if (!File.Exists(dllPath))
            return;

        Assembly assembly = Assembly.Load(
            File.ReadAllBytes(dllPath)
        );

        Type entryType = assembly.GetType(info.entryPoint);

        if (entryType == null ||
            !typeof(IOrvexMod).IsAssignableFrom(entryType) ||
            entryType.IsAbstract)
        {
            Debug.LogError("[Mods] Invalid entry point: " + info.id);
            return;
        }

        IOrvexMod mod = (IOrvexMod)Activator.CreateInstance(entryType);

        mod.OnLoad(context);

        loadedCodeMods.Add(mod);
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        for (int i = loadedCodeMods.Count - 1; i >= 0; i--)
        {
            try
            {
                loadedCodeMods[i].OnUnload();
            }
            catch (Exception ex)
            {
                Debug.LogError("[Mods] Unload error: " + ex);
            }
        }

        loadedCodeMods.Clear();
        Instance = null;
    }
}