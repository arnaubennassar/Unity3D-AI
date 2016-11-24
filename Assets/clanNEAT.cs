using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class clanNEAT : UnitController {
	bool IsRunning;
	IBlackBox box;
	public bool itsMyTurn = false;
	public Transform enemyNeat;
	int currentWarrior = 0;

	void FixedUpdate() {
		//Change the number of inputs/outputs at "Optimizer.cs"

			//int active = transform.GetComponent<clanController> ().aliveWarriors;
		if (IsRunning && itsMyTurn) {
			//Calculate INPUT (i.e. "frontsensor", "leftsensor", ....)
			ISignalArray inputArr = box.InputSignalArray;
			if (currentWarrior < 0)
				currentWarrior = 0;
			else if (currentWarrior == transform.GetComponent<clanController> ().totalWarriors - 1) {
				currentWarrior = -1;
				itsMyTurn = false;
				enemyNeat.GetComponent<clanNEAT> ().giveMeTurn ();
			} else
				++currentWarrior;
			clanController.maps maps = new clanController.maps ();
			maps = transform.GetComponent<clanController> ().getMap ();
			clanController.Warrior current = new clanController.Warrior ();
			current = transform.GetComponent<clanController> ().getWarrior (currentWarrior);
			if (current.def > 0) {
				inputArr [0] = current.atck;
				inputArr [1] = current.def;
				inputArr [2] = current.pos.x;
				inputArr [3] = current.pos.y;
				int sX = (int)transform.GetComponent<clanController> ().boardSize.x;
				int sY = (int)transform.GetComponent<clanController> ().boardSize.y;
				for (int i = 0; i < sX; ++i) {
					for (int j = 0; i < sY; ++j) {
						inputArr [i * sY + j + 4] = maps.attack [i, j];
					}
				}
				int desp = 4 + sX * sY;
				for (int i = 0; i < sX; ++i) {
					for (int j = 0; i < sY; ++j) {
						inputArr [i * sY + j + desp] = maps.defense [i, j];
					}
				}
				box.Activate ();
				ISignalArray outputArr = box.OutputSignalArray;

				//Do stuff with the output (i.e. move an object using "steer" and "gas")
				var upDown = (float)outputArr [0];
				var leftRight = (float)outputArr [1];
				var attack = (float)outputArr [2];

				if ((upDown + leftRight) / 2 > attack) {
					Vector2 aux = new Vector2 ();
					if (upDown > 0.7f)
						aux.x = 1;
					else if (upDown < 0.3f)
						aux.y = -1;
					if (leftRight > 0.7f)
						aux.x = 1;
					else if (leftRight < 0.3f)
						aux.y = -1;
					transform.GetComponent<clanController> ().move (currentWarrior, aux);
				} else if (attack > 0.5f)
					transform.GetComponent<clanController> ().attack (currentWarrior);
				++currentWarrior;
				/*	var steer = (float)outputArr[0] * 2 - 1;
            var gas = (float)outputArr[1] * 2 - 1;

            var moveDist = gas * Speed * Time.deltaTime;
            var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

            transform.Rotate(new Vector3(0, turnAngle, 0));
            transform.Translate(Vector3.forward * moveDist);	 
        */	
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