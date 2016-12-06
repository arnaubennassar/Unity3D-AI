using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class clanNEAT : UnitController {
	public bool IsRunning;
	IBlackBox box;
	public bool itsMyTurn = true;
	public Transform enemy;
	public int currentWarrior = 0;

	void FixedUpdate() {
		//Change the number of inputs/outputs at "Optimizer.cs"

			//int active = transform.GetComponent<clanController> ().aliveWarriors;
		if (IsRunning && itsMyTurn) {
			//Calculate INPUT (i.e. "frontsensor", "leftsensor", ....)
			ISignalArray inputArr = box.InputSignalArray;

			clanController.maps maps = new clanController.maps ();

			clanController.Warrior current = new clanController.Warrior ();
			current = transform.GetComponent<clanController> ().getWarrior (currentWarrior);
			maps = transform.GetComponent<clanController> ().getMap ();
			
			if (current.def > 0) {
				inputArr [0] = current.atck;
				inputArr [1] = current.def;
				inputArr [2] = current.pos.x;
				inputArr [3] = current.pos.y;
				int sX = 10;
				int sY = 10;
				for (int i = 0; i < sX; ++i) {
					for (int j = 0; j < sY; ++j) {
						inputArr [i * sY + j + 4] = maps.attack [i, j];
					}
				}
				int desp = 4 + sX * sY;
				for (int i = 0; i < sX; ++i) {
					for (int j = 0; j < sY; ++j) {
						inputArr [i * sY + j + desp] = maps.defense [i, j];
					}
				}
				box.Activate ();
				ISignalArray outputArr = box.OutputSignalArray;

				//Do stuff with the output (i.e. move an object using "steer" and "gas")
				var up = (float)outputArr [0];
				var down = (float)outputArr [1];
				var left = (float)outputArr [2];
				var right = (float)outputArr [3];
				var attack = (float)outputArr [4];

				Vector2 aux = new Vector2 ();
				if (up >= down)
					aux.y = 1;
				else if (down >= up)
					aux.y = -1;
				if (right > left)
					aux.x = 1;
				else if (left > right)
					aux.x = -1;
				transform.GetComponent<clanController> ().move (currentWarrior, aux);
				if (attack > 0.5f)
					transform.GetComponent<clanController> ().attack (currentWarrior);
				//++currentWarrior;
				/*	var steer = (float)outputArr[0] * 2 - 1;
        var gas = (float)outputArr[1] * 2 - 1;

        var moveDist = gas * Speed * Time.deltaTime;
        var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

        transform.Rotate(new Vector3(0, turnAngle, 0));
        transform.Translate(Vector3.forward * moveDist);	 
    */	
			} 
			
			++currentWarrior;
			currentWarrior %= 10;
			if (currentWarrior == 0) {
				currentWarrior = 0;
				itsMyTurn = !itsMyTurn;
				enemy.GetComponent<clanNEAT> ().giveMeTurn ();
			} 
		} 
	}

	public void giveMeTurn(){
		itsMyTurn = true;
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
		float fit = transform.GetComponent<clanController>().fitness(currentWarrior);

		if (fit > 0) return fit;
		return 0;
	}
}