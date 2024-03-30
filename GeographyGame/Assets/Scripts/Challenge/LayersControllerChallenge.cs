/*******************************************************************
* Author            : Max Schneider and u3d
* Copyright         : MIT License
* File Name         : WorldMapManager.cs
* Description       : This file controlls the Material selection of the Dropdown Menu.
*
/******************************************************************/

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayersControllerChallenge : MonoBehaviour
{
    // Logic from u3d but rewritten to TMP_Dropdown by Max Schneider
    [SerializeField] TMP_Dropdown drop;
    List<string> options;

    void Awake()
    {
        drop.onValueChanged.AddListener(OnChange);
        WorldMapManager.EventChangeState += OnChangeState;
    }
    // If the user selects a different Material in the dropdown menu, material is changed
    private void OnChange(int id)
    {
        // Following Material Layers are by u3d
        if (id == 0) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Earth;
        // Following Material Layers are by Max SChneider 
        if (id == 1) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthNight;
        if (id == 2) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthJanuary;
        if (id == 3) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthAugust;
        if (id == 4) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthBorders;
        Debug.Log("State Changed");
    }
    private void OnDestroy()
    {
        drop.onValueChanged.RemoveListener(OnChange);
        WorldMapManager.EventChangeState -= OnChangeState;
    }
    void OnChangeState()
    {
        drop.value = (int)WorldMapManager.instance.CurrentState;
    }
}
