using UnityEngine;

namespace CryptoWars.Animations
{
    public class Animater : MonoBehaviour
    {
        [SerializeField] string flipZParameter = "IsGravityUpwards";

        Animator anm;

        private void Awake() { anm = GetComponent<Animator>(); }

        public void FlipZAxis() { anm.SetBool(flipZParameter, !anm.GetBool(flipZParameter)); } 
    }
}