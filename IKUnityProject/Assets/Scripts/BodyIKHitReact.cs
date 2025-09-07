using System.Collections;
using UnityEngine;

public class BodyIKHitReact : IKHitDetection
{
    [SerializeField] private float _minStunTime = 0.5f;
    [SerializeField] private float _maxStunTime = 0.7f;
    [SerializeField] private Transform _target;

    protected override void OnHit(HitData hitData)
    {
        StartCoroutine(HitCoroutine(hitData));
    }

    private IEnumerator HitCoroutine(HitData hitData)
    {
        _hitReact.DisableTrigger();
        
        var timer = 0f;
        var duration = Mathf.Clamp(_maxStunTime * hitData.Force, _minStunTime, _maxStunTime);
        var startDir = _target.right;
        var endDir = hitData.Direction * -1;
        
        var startRot = LookRotationX(startDir, Vector3.up);
        var endRot = LookRotationX(endDir, Vector3.up);
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            var currentRot = Quaternion.Slerp(startRot, endRot, _curve.Evaluate(timer / duration));
            _target.rotation = currentRot;
            yield return null;
        }
        
        _hitReact.EnableTrigger();
        StopStun?.Invoke();
    }
    
    private Quaternion LookRotationX(Vector3 xDir, Vector3 upHint)
    {
        Vector3 x = xDir.normalized;

        // Se l'upHint è quasi parallelo a xDir, correggi per evitare degenerazioni
        if (Vector3.Dot(x, upHint.normalized) > 0.999f)
            upHint = Vector3.up; 

        // Ricostruzione ortogonale
        Vector3 z = Vector3.Cross(x, upHint).normalized;
        Vector3 y = Vector3.Cross(z, x);

        // Crea la rotazione dalla base (x,y,z)
        Matrix4x4 m = Matrix4x4.identity;
        m.SetColumn(0, new Vector4(x.x, x.y, x.z, 0));
        m.SetColumn(1, new Vector4(y.x, y.y, y.z, 0));
        m.SetColumn(2, new Vector4(z.x, z.y, z.z, 0));

        return m.rotation;
    }
}