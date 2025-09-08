using Sirenix.OdinInspector;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _fullBodyCollider;
    [SerializeField] private Rigidbody[] _ragdollRigidbodies;
    private Collider[] _ragdollColliders;
    

    private void Awake()
    {
        _ragdollColliders = new Collider[_ragdollRigidbodies.Length];
        for (var i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].isKinematic = true;
            _ragdollColliders[i] = _ragdollRigidbodies[i].GetComponent<Collider>();
            _ragdollColliders[i].enabled = false;
        }

    }

    
    public void EnableRagdoll()
    {
        _fullBodyCollider.GetComponent<Rigidbody>().isKinematic = true;
        _fullBodyCollider.enabled = false;
        _animator.enabled = false;
        
        foreach (var ragdollCollider in _ragdollColliders)
        {
            ragdollCollider.enabled = true;
        }
        
        foreach (var ragdollRigidbody in _ragdollRigidbodies)
        {
            ragdollRigidbody.isKinematic = false;
            ragdollRigidbody.useGravity = true;
        }
    }
}
