using Player;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Collider), typeof(Animator))]
    public class Gate: MonoBehaviour
    {
        [SerializeField] private AudioClip sound;

        private Animator _animator;
        private Collider _collider;
        private static readonly int OpenTrigger = Animator.StringToHash("open");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerController>(out _)) return;
            _animator.SetTrigger(OpenTrigger);
            _collider.enabled = false;
            PlayerAudio.Instance.Play(sound);
        }
    }
}