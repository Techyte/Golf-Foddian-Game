using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GolfBallController : MonoBehaviour
{
    [SerializeField] private float pushForce;
    [SerializeField] private float maxPushForce = 10;
    [SerializeField] private float currentVelBonusMultiplier = 0.3f;
    [SerializeField] private Camera cam;

    public float MaxPushForce => maxPushForce;

    private Rigidbody2D rb;

    private bool _canHit;
    private bool _colliding;
    private Vector2 _mouseStartPos;

    public Vector2 MousePosition => cam.ScreenToWorldPoint(Input.mousePosition);

    public Vector2 dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseStartPos = MousePosition;
        }
        
        if(_canHit && !PauseMenu.Instance._isOn)
        {
            if (Input.GetMouseButtonUp(0))
            {
                PushBall();
            }
        }

        if(Input.GetMouseButton(0))
        {
            dir = (_mouseStartPos - MousePosition) / 100;
        }
        else
        {
            dir = Vector2.zero;
        }
        
        dir = Vector2.ClampMagnitude(dir, maxPushForce);
    }

    private void FixedUpdate()
    {
        if (!Input.GetMouseButton(0) && _colliding)
        {
            rb.velocity = 0.9f * rb.velocity;
        }
    }

    #region collision

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _canHit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _canHit = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _canHit = true;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _colliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _colliding = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _colliding = true;
    }

    #endregion

    private void PushBall()
    {
        Debug.Log(dir.magnitude);

        float mag = dir.magnitude;
        
        dir.Normalize();
        
        rb.velocity = pushForce * mag * dir + (rb.velocity * currentVelBonusMultiplier);
    }
}
