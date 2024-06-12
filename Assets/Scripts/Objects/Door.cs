using UnityEngine;

namespace Objects
{
    public class Door: PickUp
    {
        [SerializeField] private Door related;
    
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            Destroy(related.gameObject);
        }
    }
}