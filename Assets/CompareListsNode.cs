using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CompareListsNode : Unit
{
    [DoNotSerialize]
    public ControlInput inputTrigger;

    [DoNotSerialize]
    public ControlOutput outputTrigger;

    [DoNotSerialize]
    public ValueInput myValueA;

    [DoNotSerialize]
    public ValueInput myValueB;

    [DoNotSerialize]
    public ValueOutput result;

    private bool resultValue;
    protected override void Definition()
    {
        inputTrigger = ControlInput("inputTrigger", (flow) =>
        {
            resultValue = CompareLists.AreEqual(flow.GetValue<List<Action>>(myValueA), flow.GetValue<List<Action>>(myValueB));
            return outputTrigger;
        });
        outputTrigger = ControlOutput("outputTrigger");

        myValueA = ValueInput<List<Action>>("Desired Combination");
        myValueB = ValueInput<List<Action>>("Incoming Combination");
        result = ValueOutput<bool>("result", (flow) => resultValue);

        Requirement(myValueA, inputTrigger); //To display that we need the myValueA value to be set to let the unit process
        Requirement(myValueB, inputTrigger); //To display that we need the myValueB value to be set to let the unit process
        Succession(inputTrigger, outputTrigger); //To display that the input trigger port input will exits at the output trigger port exit. Not setting your succession also grays out the connected nodes but the execution is still done.
        Assignment(inputTrigger, result);//To display the data that is written when the inputTrigger is triggered to the result string output.
    }
}