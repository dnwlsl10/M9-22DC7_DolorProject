using UnityEngine;
using Random = System.Random;

namespace Project.Scripts.Fractures
{
    public class FractureThis : MonoBehaviour
    {
        [SerializeField] private Anchor anchor = Anchor.Bottom;
        [SerializeField] private int chunks = 500;
        [SerializeField] private float density = 50;
        [SerializeField] private float internalStrength = 100;
            
        [SerializeField] private Material insideMaterial;
        [SerializeField] private Material outsideMaterial;

        private Random rng = new Random();

        private void Start()
        {
            // GetComponent<Collider>().enabled = true;
            FractureGameobject();
            // GetComponent<Collider>().enabled = true;
        }
GameObject go;
ChunkGraphManager cg;
        public void FractureGameobject()
        {
            var seed = rng.Next();
            cg = Fracture.FractureGameObject(
                gameObject,
                anchor,
                seed,
                chunks,
                insideMaterial,
                outsideMaterial,
                internalStrength,
                density,
                out go
            );
            go.SetActive(false);
        }

        public void TEST()
        {
            gameObject.SetActive(false);
            go.SetActive(true);


            cg.SearchGraph();
            
            Invoke("T", 0.1f);
        }

        void T()
        {

            foreach(var g in go.GetComponentsInChildren<Rigidbody>())
                g.AddExplosionForce(10000, transform.position + Vector3.up * 50, 20);
            
        }
    }
}