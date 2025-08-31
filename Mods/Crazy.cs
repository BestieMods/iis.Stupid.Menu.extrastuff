using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using GorillaLocomotion;

public class BloodWater : MonoBehaviour
{
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    private bool isModified = false;

    public void RegisterButtons()
    {
        new Button("Blood Water Override", () =>
        {
            if (isModified) return;

            foreach (Renderer r in Resources.FindObjectsOfTypeAll<Renderer>())
            {
                if (r.name.ToLower().Contains("water") || r.gameObject.tag.ToLower() == "water")
                {
                    if (!originalColors.ContainsKey(r))
                    {
                        originalColors[r] = r.material.color;
                    }

                    r.material.color = Color.red;
                }
            }

            isModified = true;
        });

        new Button("Undo Blood Water", () =>
        {
            if (!isModified) return;

            foreach (var kvp in originalColors)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.material.color = kvp.Value;
                }
            }

            originalColors.Clear();
            isModified = false;
        });
    }
}
