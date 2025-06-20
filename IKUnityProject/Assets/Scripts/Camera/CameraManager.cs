using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        private static CameraManager _instance;

        [SerializeField] private Transform _target;
        [SerializeField] private float _yOffset = 1;
        [SerializeField] private float _distanceRatio = 1.33f;
        [SerializeField] private Vector2 _minMaxDistance = new Vector2(2, 4);
        [SerializeField] private Transform _cameraHolder;
        
        private Transform[] _fighters;
        
        private void Awake()
        {
            if(!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _fighters = SpawnManager.Instance.Fighters;
        }

        private void Update()
        {
            _target.position = (_fighters[1].position + _fighters[0].position) / 2 + Vector3.up * _yOffset;
            var x = Vector3.Distance(_fighters[0].position, _fighters[1].position) / _distanceRatio;
            x = Mathf.Clamp(x, _minMaxDistance.x, _minMaxDistance.y);
            _cameraHolder.position = new Vector3(x, _target.position.y, _target.position.z);
            _cameraHolder.LookAt(_target.position);
        }
    }
}
