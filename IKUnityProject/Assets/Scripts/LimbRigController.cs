using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LimbRigController : MonoBehaviour
{
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

    public void SetTargetPosition(Vector3 position)
    {
        _target.position = position;
    }
}