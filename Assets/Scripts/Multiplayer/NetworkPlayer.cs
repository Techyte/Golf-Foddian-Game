using TMPro;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public ushort id;
    public TextMeshPro username;
}

public class NetworkPlayerData
{
    public ushort id;
    public Vector2 position;
    public string username;
}