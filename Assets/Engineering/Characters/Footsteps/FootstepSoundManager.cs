using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundManager : MonoBehaviour
{
    [SerializeField] private FootstepManager footstepManager;
    public void FootstepWalk() { if (footstepManager != null) footstepManager.PlayWalkSound(); }
    public void FootstepRun() { if (footstepManager != null) footstepManager.PlayRunSound(); }
    public void FootstepJump() { if (footstepManager != null) footstepManager.PlayJumpSound(); }
    public void FootstepLand() { if (footstepManager != null) footstepManager.PlayLandSound(); }
}
