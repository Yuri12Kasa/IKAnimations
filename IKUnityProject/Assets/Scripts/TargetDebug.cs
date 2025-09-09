using UnityEngine;

namespace Assets.Scripts
{
    public class TargetDebug : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            SpawnManager.Instance.OnDebugModeOn += EnableDebug;
            SpawnManager.Instance.OnDebugModeOff += DisableDebug;
            if(SpawnManager.Instance.DebugMode)
            {
                EnableDebug();
            }
            else
            {
                DisableDebug();
            }
        }

        private void DisableDebug()
        {
            _meshRenderer.enabled = false;
        }

        private void EnableDebug()
        {
            _meshRenderer.enabled = true;
        }

        private void Destroy()
        {
            SpawnManager.Instance.OnDebugModeOn -= EnableDebug;
            SpawnManager.Instance.OnDebugModeOff -= DisableDebug;
        }
    }
}