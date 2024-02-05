using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public ushort id;
}

public class NetworkPlayerData
{
    public ushort id;
    public Vector2 position;
}