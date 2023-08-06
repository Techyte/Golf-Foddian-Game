using UnityEngine;

public class BallArrowManager : MonoBehaviour
{
    [SerializeField] private GolfBallController controller;

    [SerializeField] private float maxPowerLength;

    private void Update()
    {
        if(!PauseMenu.Instance._isOn)
        {
            float XScale = controller.dir.magnitude / controller.MaxPushForce * maxPowerLength;
            
            transform.localScale = new Vector3(XScale, transform.localScale.y, 0);

            var angle = Mathf.Atan2(controller.dir.y, controller.dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            transform.localScale = new Vector3(0, transform.localScale.y, 0);
        }
    }
}
