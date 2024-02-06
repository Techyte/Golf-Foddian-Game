namespace Game
{
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [RequireComponent(typeof(Rigidbody2D))]
    public class GolfBallController : MonoBehaviour
    {
        [SerializeField] private float pushForce;
        [SerializeField] private float maxPushForce = 10;
        [SerializeField] private float currentVelBonusMultiplier = 0.3f;
        [SerializeField] private Camera cam;
        [SerializeField] private float mouseDistanceAtMax= 10;
        [SerializeField] private float volume = 0.5f;
        [SerializeField] private bool soundEffects = true;
        [SerializeField] private List<AudioSource> ballHitSounds;
        [SerializeField] private List<AudioSource> ballBounceSounds;
        [SerializeField] private float bounceForce = 20;

        public float MaxPushForce => maxPushForce;

        private Rigidbody2D rb;

        private bool _canHit;
        private bool _colliding;
        private bool _fallingThroughSand;
        private Vector2 _mouseStartPos;

        private Vector2 MousePosition => cam.ScreenToWorldPoint(Input.mousePosition);

        public Vector2 dir;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            mouseDistanceAtMax = PlayerPrefs.GetFloat("MouseDistanceAtMax");
            volume = PlayerPrefs.GetFloat("Volume", 0.5f);
            soundEffects = PlayerPrefs.GetInt("SoundEffects", 1) == 1;

            foreach (var sound in ballHitSounds)
            {
                sound.volume = volume;
            }
            
            foreach (var sound in ballBounceSounds)
            {
                sound.volume = volume;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !FinishLine.finished)
            {
                _mouseStartPos = MousePosition;
            }
            
            if(_canHit && !PauseMenu.Instance._isOn && rb.velocity.y >= 0 && !FinishLine.finished)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    PushBall();
                    PlayHitSound();
                }
            }

            if(Input.GetMouseButton(0) && !FinishLine.finished)
            {
                dir = _mouseStartPos - MousePosition;

                float percentage = dir.magnitude / mouseDistanceAtMax;

                if (percentage > 1)
                {
                    percentage = 1;
                }

                dir = dir.normalized * (maxPushForce * percentage);
            }
            else
            {
                dir = Vector2.zero;
            }
        }

        private void PlayHitSound()
        {
            if(soundEffects)
            {
                int id = Random.Range(0, ballHitSounds.Count);
                ballHitSounds[id].Play();
            }
        }

        private void PlayBounce()
        {
            if(soundEffects)
            {
                ballBounceSounds[0].Play();
            }
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
            if (!collision.gameObject.CompareTag("Ice"))
            {
                _colliding = true;
            }

            if (collision.gameObject.CompareTag("Bounce"))
            {
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y * bounceForce);
                Debug.Log(rb.velocity);
            }

            if (rb.velocity.y > rb.velocity.x)
            {
                PlayBounce();
            }
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
            float mag = dir.magnitude;
            
            dir.Normalize();
            
            rb.velocity = pushForce * mag * dir + (rb.velocity * currentVelBonusMultiplier);
        }

        public void SetCanHit(bool value)
        {
            _canHit = value;
            if (!value)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
        }
    }   
}