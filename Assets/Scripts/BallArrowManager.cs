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

            if (percentageOfMax > 1)
            {
                percentageOfMax = 1;
            }

            int id = 0;
            
            if (percentageOfMax >= 0.99)
            {
                id = arrowSprites.Count - 1;
            }
            else
            {
                id = Mathf.CeilToInt(percentageOfMax * arrowSprites.Count-1);
                if (id == 4)
                {
                    id = 3;
                }
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
