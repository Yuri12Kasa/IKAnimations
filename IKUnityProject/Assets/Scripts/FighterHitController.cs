using System;
using UnityEngine;

public class FighterHitController : MonoBehaviour
{
    public Action StartStun;
    public Action StopStun;
    
    [SerializeField] private IKHitDetection[] _hitDetections;
    
    private void Start()
    {
        foreach (var hitDetection in _hitDetections)
        {
            hitDetection.StartStun += () => StartStun?.Invoke();
            hitDetection.StopStun += () => StopStun?.Invoke();
        }
    }
}