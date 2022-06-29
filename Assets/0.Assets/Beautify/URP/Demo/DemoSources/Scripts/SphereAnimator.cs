using UnityEngine;


namespace Beautify.Universal {

    public class SphereAnimator : MonoBehaviour {

        Rigidbody rb;

        void Start() {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            if (transform.position.z < 1f) {
                rb.AddForce(Vector3.forward * 10f);
            } else if (transform.position.z > 8f) {
                rb.AddForce(-Vector3.forward * 10f);
            }
        }
    }
}