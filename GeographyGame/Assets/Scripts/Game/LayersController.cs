using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayersController : MonoBehaviour
{
    [SerializeField] TMP_Dropdown drop;
    List<string> options;

    void Awake()
    {

        drop.onValueChanged.AddListener(OnChange);
        WorldMapManager.EventChangeState += OnChangeState;
    }
    private void OnChange(int id)
    {

        if (id == 0) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Earth;
        if (id == 1) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Politic;
        if (id == 2) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Population;
        if (id == 3) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Science;
        if (id == 4) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Transport;
        if (id == 5) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Disaster;
        if (id == 6) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.Climat;
        if (id == 7) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthDay;
        if (id == 8) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthNight;
        if (id == 9) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthJanuary;
        if (id == 10) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthAugust;
        if (id == 11) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthBorders;
        if (id == 12) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.EarthDetails;
        if (id == 13) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.OceanFlow;
        if (id == 14) WorldMapManager.instance.CurrentState = WorldMapManager.instance.CurrentState = WorldMapManager.State.SeaLevelRise;
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
