using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class clanNEAT : UnitController {
	bool IsRunning;
	IBlackBox box;

	void FixedUpdate() {
			
		}

	public override void Stop()
	{
		this.IsRunning = false;
	}

	public override void Activate(IBlackBox box)
	{
		this.box = box;
		this.IsRunning = true;
	}

	public override float GetFitness()
	{
		// Implement a meaningful fitness function here, for each unit.
		return 0;
	}
}