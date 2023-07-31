using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

[UnitCategory("Custom Units")]
[UnitSubtitle("Tweens the rotation of a Target Transform component")]
public class DOJumpNode : Unit
{
    [DoNotSerialize]
    public ControlInput inputTrigger;

    [DoNotSerialize]
    public ControlOutput outputTrigger;

    [DoNotSerialize]
    public ValueInput targetTransform;

    [DoNotSerialize]
    public ValueInput jumpDestination;

    [DoNotSerialize]
    public ValueInput jumpPower;

    [DoNotSerialize]
    public ValueInput jumpAmount;

    [DoNotSerialize]
    public ValueInput jumpDuration;

    [DoNotSerialize]
    public ValueInput jumpDelay;

    [DoNotSerialize]
    public ValueInput jumpEase;

    private Transform myTransform;
    protected override void Definition()
    {
        //The lambda to execute our node action when the inputTrigger port is triggered.
        inputTrigger = ControlInput("inputTrigger", (flow) =>
        {
            myTransform = flow.GetValue<Transform>(targetTransform);
            myTransform.DOComplete();
            myTransform.DOLocalJump(flow.GetValue<Vector3>(jumpDestination), flow.GetValue<float>(jumpPower), flow.GetValue<int>(jumpAmount), flow.GetValue<float>(jumpDuration))
            .SetEase(flow.GetValue<Ease>(jumpEase)).SetDelay(flow.GetValue<float>(jumpDelay));
            return outputTrigger;
        });

        outputTrigger = ControlOutput("outputTrigger");

        targetTransform = ValueInput<Transform>("Target Transform", null);
        jumpDestination = ValueInput<Vector3>("Jump Destination", Vector3.zero);
        jumpDuration = ValueInput<float>("Jump Duration", .5f);
        jumpPower = ValueInput<float>("Jump Power", 3f);
        jumpAmount = ValueInput<int>("Amount of Jumps", 1);
        jumpEase = ValueInput<Ease>("Jump Tween", Ease.InOutSine);
        jumpDelay = ValueInput<float>("Jump Delay", 0);

    }
}
