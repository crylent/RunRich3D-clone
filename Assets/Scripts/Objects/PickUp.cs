using Player;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Collider))]
    public class PickUp: MonoBehaviour
    {
        [SerializeField] private bool isNegative;
        [SerializeField] private int value = 1;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerMoneyComponent>(out var player)) return;
            if (isNegative) player.TakeMoney(value);
            else player.AddMoney(value);
            Destroy(gameObject);
        }
    }
}