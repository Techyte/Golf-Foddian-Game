using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TempeartureManager : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private Transform player;
    [SerializeField] private float threshold = 15;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    // private void Update()
    // {
    //     float playerY = player.position.y;
    //     float scale = maxHeight - minHeight;
    //     float playerCompletion = playerY / scale;
    //
    //     float temperature = playerCompletion * (threshold * 2);
    //
    //     WhiteBalance balance = null;
    //
    //     foreach (var component in volume.sharedProfile.components)
    //     {
    //         if (component is WhiteBalance)
    //         {
    //             balance = (WhiteBalance)component;
    //         }
    //     }
    //
    //     Debug.Log(-temperature);
    //     balance.temperature.value = -temperature;
    // }
}
