using UnityEngine;

public interface IOrvexMod
{
    void OnLoad(ModContext context);

    void OnUnload();
}