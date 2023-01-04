using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Show teleport ray only when the player is triggering them
/// https://youtube.com/watch?v=fZXKGJYri1Y&si=EnSIkaIECMiOmarE&t=1345
/// </summary>
public class ToggleController : MonoBehaviour
{
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;

    public InputHelpers.Button teleportActivationButton;

    private void Update()
    {
        if (leftTeleportRay)
        {
            UpdateRayState(leftTeleportRay);
        }
        
        if (rightTeleportRay)
        {
            UpdateRayState(rightTeleportRay);
        }
    }

    private void UpdateRayState(XRController controller)
    {
        var xrInteractorLineVisual = controller.GetComponent<XRInteractorLineVisual>();
        
        bool currentState = CheckIfActivated(controller);
        bool previousState = xrInteractorLineVisual.enabled;
        
        // Change only when the state is changing between two frames
        if (currentState != previousState)
        {
            xrInteractorLineVisual.enabled = currentState;
            xrInteractorLineVisual.reticle.SetActive(currentState);
        }
    }
    
    private bool CheckIfActivated(XRController controller)
    {
        controller.inputDevice.IsPressed(teleportActivationButton, out bool isActivated);
        return isActivated;
    }
}
