using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LimbRigController : MonoBehaviour
{
    public Transform TargetToFollow;
    public float Weight => _rig.weight;
    [SerializeField] private Transform _target;
    
    private Rig _rig;

    private void Awake()
    {
        _rig = GetComponent<Rig>();
    }

    public void SetRigWeight(float weight)
    {
        _rig.weight = weight;
    }

    private void Update()
    {
        _target.position = TargetToFollow.position;
        _target.rotation = TargetToFollow.rotation;
    }

    public void SetTargetToFollow(Transform target)
    {
        TargetToFollow = target;
    }
}