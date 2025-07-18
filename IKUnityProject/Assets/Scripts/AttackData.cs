using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public LimbRigController LimbRig => _limbRig;
    public AnimationClip Clip => _clip;
    public AnimationCurve CurveIn => _curveIn;
    public AnimationCurve CurveOut => _curveOut;
    
    [SerializeField] private LimbRigController _limbRig;
    [SerializeField] private AnimationClip _clip;
    [SerializeField] private AnimationCurve _curveIn;
    [SerializeField] private AnimationCurve _curveOut;
}