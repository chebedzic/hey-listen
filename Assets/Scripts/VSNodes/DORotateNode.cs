using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

[UnitCategory("Custom Units")]
[UnitSubtitle("Tweens the rotation of a Target Transform component")]
public class DORotateNode : Unit
{
    [DoNotSerialize]
    public ControlInput inputTrigger;

    [DoNotSerialize]
    public ControlOutput outputTrigger;

    [DoNotSerialize]
    public ValueInput targetTransform;

    [DoNotSerialize]
    public ValueInput rotationAmount;

    [DoNotSerialize]
    public ValueInput rotationDuration;

    [DoNotSerialize]
    public ValueInput rotationTween;

    private Transform myTransform;
    protected override void Definition()
    {
        //The lambda to execute our node action when the inputTrigger port is triggered.
        inputTrigger = ControlInput("inputTrigger", (flow) =>
        {
            myTransform = flow.GetValue<Transform>(targetTransform);
            myTransform.DOComplete();
            myTransform.DORotate(flow.GetValue<Vector3>(rotationAmount), flow.GetValue<float>(rotationDuration), RotateMode.FastBeyond360)
            .SetEase(flow.GetValue<Ease>(rotationTween));
            return outputTrigger;
        });

        outputTrigger = ControlOutput("outputTrigger");

        targetTransform = ValueInput<Transform>("Target Transform", null);
        rotationAmount = ValueInput<Vector3>("Rotation Amount", Vector3.zero);
        rotationDuration = ValueInput<float>("Rotation Duration", .5f);
        rotationTween = ValueInput<Ease>("Rotation Tween", Ease.InOutSine);

    }
}
