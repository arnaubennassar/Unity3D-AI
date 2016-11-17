using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class clanNEAT : UnitController {
	bool IsRunning;
	IBlackBox box;

	void FixedUpdate() {
		//Change the number of inputs/outputs at "Optimizer.cs"


		if (IsRunning) {
			//Calculate INPUT (i.e. "frontsensor", "leftsensor", ....)
			ISignalArray inputArr = box.InputSignalArray;
			/*	inputArr[0] = frontSensor;
				inputArr[1] = leftFrontSensor;
				inputArr[2] = leftSensor;
				inputArr[3] = rightFrontSensor;
				inputArr[4] = rightSensor;	
			*/

			box.Activate ();
			ISignalArray outputArr = box.OutputSignalArray;

			//Do stuff with the output (i.e. move an object using "steer" and "gas")

			/*	var steer = (float)outputArr[0] * 2 - 1;
	            var gas = (float)outputArr[1] * 2 - 1;

	            var moveDist = gas * Speed * Time.deltaTime;
	            var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

	            transform.Rotate(new Vector3(0, turnAngle, 0));
	            transform.Translate(Vector3.forward * moveDist);	 
	        */	
		}
	}

	public override void Stop()
	{
		//CONTROL TURNS ??
		this.IsRunning = false;
	}

	public override void Activate(IBlackBox box)
	{
		//CONTROL TURNS ??
		this.box = box;
		this.IsRunning = true;
	}

	public override float GetFitness()
	{
		// Implement a meaningful fitness function here, for each unit.
		float fit = 0;
		//Calculate fitness in "fit"
		//Something like ( currentDef / totalDef   - enemycurrentDef / enemytotalDef  + (same with attack instead Def) + (same with danger/leading)  )/N items   )
		if (fit > 0) return fit;
		return 0;
	}
}