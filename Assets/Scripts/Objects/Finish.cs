using Player;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Collider))]
    public class Finish: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerController>(out _)) return;
            GameManager.Instance.Win();
        }
    }
}