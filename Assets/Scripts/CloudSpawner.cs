using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private List<Sprite> cloudSprites;
    [SerializeField] private float desiredCloudCount = 8;
    [SerializeField] private Transform player;
    [SerializeField] private float playerSpawnDistanceY = 10;
    [SerializeField] private float playerSpawnDistanceX = 10;
    [SerializeField] private float cloudSpawnDelay = 5;
    [SerializeField] private float cloudMoveSpeed;
    [SerializeField] private float cloudSize = 4;

    private float _currentCloudCount => _currentClouds.Count;

    private List<Transform> _currentClouds = new List<Transform>();

    private bool canSpawn = true;

    private void Update()
    {
        if (_currentCloudCount < desiredCloudCount && canSpawn)
        {
            SpawnCloud();
        }
        else if (_currentCloudCount > desiredCloudCount)
        {
            Destroy(_currentClouds[0]);
        }

        List<Transform> destroy = new List<Transform>();

        foreach (var cloud in _currentClouds)
        {
            cloud.position = new Vector3(cloud.position.x - cloudMoveSpeed * Time.deltaTime, cloud.position.y, 0);

            Vector2 relativeCloudPos = cloud.position - player.position;

            if (relativeCloudPos.y > playerSpawnDistanceY / 2)
            {
                destroy.Add(cloud);
            }
            else if (relativeCloudPos.y < -playerSpawnDistanceY / 2)
            {
                destroy.Add(cloud);
            }
            else if (relativeCloudPos.x < -playerSpawnDistanceX / 2)
            {
                destroy.Add(cloud);
            }
        }

        foreach (var cloud in destroy)
        {
            _currentClouds.Remove(cloud);
            Destroy(cloud.gameObject);
        }
    }

    private void SpawnCloud()
    {
        int rand = Random.Range(0, cloudSprites.Count);

        Sprite sprite = cloudSprites[rand];

        float yPos = Random.Range(player.position.y - playerSpawnDistanceY / 2,
            player.position.y + playerSpawnDistanceY / 2);

        Transform newCloud = new GameObject("Cloud").transform;
        SpriteRenderer renderer = newCloud.gameObject.AddComponent<SpriteRenderer>();

        renderer.sprite = sprite;

        newCloud.position = new Vector3(player.position.x + playerSpawnDistanceX /2, yPos, 0);
        newCloud.localScale *= cloudSize;
        
        _currentClouds.Add(newCloud);

        StartCoroutine(CloudSpawnCooldown());
    }

    private IEnumerator CloudSpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(cloudSpawnDelay);
        canSpawn = true;
    }
}
