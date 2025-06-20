using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    private static SpawnManager _instance;
    
    public Transform[] Fighters = new Transform[2];

    [SerializeField] private GameObject _fighterPrefab;
    [SerializeField] private Transform[] _spawnPoints = new Transform[2];

    private bool _flipped;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        var gamepads = Gamepad.all;
        for (var i = 0; i < 2; i++)
        {
            Fighters[i] = PlayerInput.Instantiate
            (
                _fighterPrefab,
                playerIndex: i,
                controlScheme: "Gamepad",
                pairWithDevice: gamepads[i]
            ).transform;
            
            Fighters[i].position = _spawnPoints[i].position;
            Fighters[i].rotation = _spawnPoints[i].rotation;
        }
    }

    private void Update()
    {
        CheckFaceDirection();
    }

    private void CheckFaceDirection()
    {
        var currentFlip = Fighters[1].position.z < Fighters[0].position.z;
        if (_flipped == currentFlip)
            return;
        
        if (currentFlip)
        {
            _flipped = true;
            Fighters[0].eulerAngles = new Vector3(0, 180, 0);
            Fighters[1].eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            _flipped = false;
            Fighters[0].eulerAngles = new Vector3(0, 0, 0);
            Fighters[1].eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
