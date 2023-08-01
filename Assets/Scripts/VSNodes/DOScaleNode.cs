using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

[UnitCategory("Custom Units")]
[UnitSubtitle("Tweens the scale of a Target Transform component")]
public class DOScaleNode : Unit
{
    [DoNotSerialize]
    public ControlInput inputTrigger;

    [DoNotSerialize]
    public ControlOutput outputTrigger;

    [DoNotSerialize]
    public ValueInput targetTransform;

    [DoNotSerialize]
    public ValueInput scale;

    [DoNotSerialize]
    public ValueInput scaleDuration;

    [DoNotSerialize]
    public ValueInput scaleEase;

    private Transform myTransform;
    protected override void Definition()
    {
        //The lambda to execute our node action when the inputTrigger port is triggered.
        inputTrigger = ControlInput("inputTrigger", (flow) =>
        {
            myTransform = flow.GetValue<Transform>(targetTransform);
            myTransform.DOComplete();
            myTransform.DOScale(flow.GetValue<float>(scale), flow.GetValue<float>(scaleDuration)).SetEase(flow.GetValue<Ease>(scaleEase));
            return outputTrigger;
        });

        outputTrigger = ControlOutput("outputTrigger");

        targetTransform = ValueInput<Transform>("Target Transform", null);
        scale = ValueInput<float>("Scale Amount", 0);
        scaleDuration = ValueInput<float>("Scale Duration", .5f);
        scaleEase = ValueInput<Ease>("Scale Ease", Ease.OutSine);

    }
}
