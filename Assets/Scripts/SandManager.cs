using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class SandManager : MonoBehaviour
{

    [SerializeField] private float stayTime;

    private TilemapCollider2D _collider2D;

    private bool _falling;
    private bool _playerOn;

    private float _timer;

    private void Awake()
    {
        _collider2D = GetComponent<TilemapCollider2D>();
    }

    private void Update()
    {
        if (_playerOn)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _falling = true;
                EnableTrigger();
            }
            else
            {
                _falling = false;
            }
        }
        else
        {
            _falling = false;
            _timer = stayTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            _playerOn = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player") && !_falling)
        {
            _playerOn = false;
            DisableTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _playerOn = false;
            DisableTrigger();
        }
    }

    private void EnableTrigger()
    {
        Debug.Log("enable trigger");
        _collider2D.isTrigger = true;
    }

    private void DisableTrigger()
    {
        _collider2D.isTrigger = false;
    }
}
