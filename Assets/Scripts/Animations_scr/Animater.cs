using UnityEngine;

namespace CryptoWars.Animations
{
    public class Animater : MonoBehaviour
    {
        [SerializeField] string flipZParameter = "IsGravityUpwards";
        [SerializeField] string groundedParameter = "IsGrounded";
        [SerializeField] string floatParameter = "TriggerFloat";

        [Header("Walk Animation Settings")]
        [SerializeField] float walkSpeed = 1f;
        [SerializeField] string walkingParameter = "IsWalking";
        [SerializeField] string walkSpeedParameter = "WalkSpeed";

        Animator anm;

        private void Awake() { anm = GetComponent<Animator>(); }

        public void FlipZAxis(bool flipped) { anm.SetBool(flipZParameter, flipped); }

        public void PlayWalkAnimation(bool isWalking)
        {
            anm.SetFloat(walkSpeedParameter, walkSpeed);
            anm.SetBool(walkingParameter, isWalking);
        }

        public void SetGroundedParam(bool isGrounded)
        {
            if (isGrounded == anm.GetBool(groundedParameter)) { return; }

            anm.SetTrigger(floatParameter);
            anm.SetBool(groundedParameter, isGrounded);
        }
    }
}