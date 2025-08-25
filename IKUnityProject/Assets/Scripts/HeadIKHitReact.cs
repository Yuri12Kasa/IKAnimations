using System.Collections;
using UnityEngine;

public class HeadIKHitReact : IKHitDetection
{
    [SerializeField] private float _maxStunTime = 2;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance = 0.5f;
    
    protected override void OnHit(HitData hitData)
    {
        StartCoroutine(HitCoroutine(hitData));
    }

    private IEnumerator HitCoroutine(HitData hitData)
    {
        _hitReact.DisableTrigger();
        var timer = 0f;
        var duration = _maxStunTime * hitData.Force;
        var startPos = _target.position;
        var endPos = _head.position + hitData.Direction * _distance;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            _target.position = Vector3.Lerp(startPos, endPos, _curve.Evaluate(timer / duration));
            yield return null;
        }
        
        _hitReact.EnableTrigger();
    }
}