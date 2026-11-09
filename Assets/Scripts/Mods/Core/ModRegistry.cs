using System.Collections.Generic;
using UnityEngine;

public class ModRegistry
{
    private readonly Dictionary<string, ModBlock> blocks =
        new Dictionary<string, ModBlock>();

    private readonly Dictionary<string, ModItem> items =
        new Dictionary<string, ModItem>();

    public bool RegisterBlock(string modId, ModBlock block)
    {
        if (block == null || string.IsNullOrWhiteSpace(block.id))
            return false;

        string id = modId + ":" + block.id;

        if (blocks.ContainsKey(id))
            return false;

        blocks.Add(id, block);

        Debug.Log("[Mods] Registered block: " + id);
        return true;
    }

    public bool RegisterItem(string modId, ModItem item)
    {
        if (item == null || string.IsNullOrWhiteSpace(item.id))
            return false;

        string id = modId + ":" + item.id;

        if (items.ContainsKey(id))
            return false;

        items.Add(id, item);

        Debug.Log("[Mods] Registered item: " + id);
        return true;
    }

    public ModBlock GetBlock(string id)
    {
        blocks.TryGetValue(id, out ModBlock block);
        return block;
    }

    public ModItem GetItem(string id)
    {
        items.TryGetValue(id, out ModItem item);
        return item;
    }

    public IReadOnlyDictionary<string, ModBlock> Blocks => blocks;
    public IReadOnlyDictionary<string, ModItem> Items => items;
}