using System.Collections.Generic;
using UnityEngine;

public class BallArrowManager : MonoBehaviour
{
    [SerializeField] private GolfBallController controller;
    [SerializeField] private List<Sprite> arrowSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float posOffset = 3.5f;
    
    private void Update()
    {
        if(!PauseMenu.Instance._isOn && controller.dir.magnitude > 0)
        {
            float percentageOfMax = controller.dir.magnitude / controller.MaxPushForce;
            
            int id = Mathf.FloorToInt(percentageOfMax * arrowSprites.Count);
            if (id >= arrowSprites.Count)
            {
                id = arrowSprites.Count - 1;
            }

            spriteRenderer.sprite = arrowSprites[id];
            
            var angle = Mathf.Atan2(controller.dir.y, controller.dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            transform.localPosition = controller.dir.normalized * posOffset;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }
}
