using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Fighter : MonoBehaviour
{
    [SerializeField] private LimbController _leftArm;
    [SerializeField] private LimbController _rightArm;
    [SerializeField] private LimbController _leftLeg;
    [SerializeField] private LimbController _rightLeg;

    [SerializeField] private AnimationClip _punchClip;
    [SerializeField] private AnimationClip _heavyPunchClip;
    
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    [Button]
    public void Punch()
    {
        StartCoroutine(ActionCoroutine("Punch", _leftArm, _punchClip));
    }
    
    [Button]
    public void HeavyPunch()
    {
        StartCoroutine(ActionCoroutine("HeavyPunch", _leftArm, _heavyPunchClip));
    }

    private IEnumerator ActionCoroutine(string animParameter, LimbController limbController, AnimationClip clip)
    {
        var clipDuration = clip.length;
        var actionConnection = clip.events[0].time;
        var timer = 0f;
        _anim.SetTrigger(animParameter);
        while (timer < actionConnection)
        {
            limbController.SetRigWeight(timer / actionConnection);
            timer += Time.deltaTime;
            yield return null;
        }
        
        limbController.SetRigWeight(1);
        var remainingTime = clipDuration - actionConnection;
        
        while (timer < remainingTime)
        {
            limbController.SetRigWeight(1 - (timer / remainingTime));
            timer += Time.deltaTime;
            yield return null;
        }

        limbController.SetRigWeight(0);
    }

    private IEnumerator PunchCoroutine()
    {
        var clipDuration = _punchClip.length;
        var timer = 0f;
        _anim.SetTrigger("Punch");
        while (timer < clipDuration / 2)
        {
            _leftArm.SetRigWeight(timer / (clipDuration / 2));
            timer += Time.deltaTime;
            yield return null;
        }
        
        _leftArm.SetRigWeight(1);

        timer = 0;
        while (timer < clipDuration / 2)
        {
            _leftArm.SetRigWeight(1 - timer / (clipDuration / 2));
            timer += Time.deltaTime;
            yield return null;
        }
        
        _leftArm.SetRigWeight(0);
    }
}

[Serializable]
public class LimbController
{
    [SerializeField] private Rig _rig;
    [SerializeField] private Transform _target;

    public void SetRigWeight(float weight)
    {
        _rig.weight = weight;
    }

    public void SetTargetPosition(Vector3 position)
    {
        _target.position = position;
    }
}
