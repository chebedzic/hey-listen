using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

[UnitCategory("Custom Units")]
[UnitSubtitle("Tweens the rotation of a Target Transform component")]
public class DOMoveNode : Unit
{
    [DoNotSerialize]
    public ControlInput inputTrigger;

    [DoNotSerialize]
    public ControlOutput outputTrigger;

    [DoNotSerialize]
    public ValueInput targetTransform;

    [DoNotSerialize]
    public ValueInput moveDestination;

    [DoNotSerialize]
    public ValueInput moveDuration;

    [DoNotSerialize]
    public ValueInput moveEase;

    private Transform myTransform;
    protected override void Definition()
    {
        //The lambda to execute our node action when the inputTrigger port is triggered.
        inputTrigger = ControlInput("inputTrigger", (flow) =>
        {
            myTransform = flow.GetValue<Transform>(targetTransform);
            myTransform.DOComplete();
            myTransform.DOMove(flow.GetValue<Vector3>(moveDestination), flow.GetValue<float>(moveDuration))
            .SetEase(flow.GetValue<Ease>(moveEase));
            return outputTrigger;
        });

        outputTrigger = ControlOutput("outputTrigger");

        targetTransform = ValueInput<Transform>("Target Transform", null);
        moveDestination = ValueInput<Vector3>("Move Destination", Vector3.zero);
        moveDuration = ValueInput<float>("Move Duration", .5f);
        moveEase = ValueInput<Ease>("Move Tween", Ease.InOutSine);

    }
}
