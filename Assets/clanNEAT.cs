using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using UnityEngine.UI;

public class clanNEAT : UnitController {
	public bool IsRunning;
	public IBlackBox box;
	public bool itsMyTurn = true;
	public bool isUser = false;
	public bool userBlock = false;
	public Transform enemy;
	public int currentWarrior = 0;

	void FixedUpdate() {
		//Change the number of inputs/outputs at "Optimizer.cs"
			//int active = transform.GetComponent<clanController> ().aliveWarriors;
		if (IsRunning && itsMyTurn && !userBlock) {
			//Calculate INPUT (i.e. "frontsensor", "leftsensor", ....)
			ISignalArray inputArr = box.InputSignalArray;

			//clanController.maps maps = new clanController.maps ();

			clanController.Warrior current = new clanController.Warrior ();
			current = transform.GetComponent<clanController> ().getWarrior (currentWarrior);
			//maps = transform.GetComponent<clanController> ().getMap ();
			
			if (current.def > 0) {
				float[] auxIN = transform.GetComponent<clanController> ().getInput (currentWarrior);
				for (int i = 0; i < 25; ++i)
					inputArr [i] = auxIN [i];
				/*
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
				*/
				box.Activate ();
				ISignalArray outputArr = box.OutputSignalArray;

				//Do stuff with the output (i.e. move an object using "steer" and "gas")
				double maxval = 0;
				int pos = 0;
				for (int i = 0; i < 9; ++i) {
					if (maxval < outputArr [i]) {
						maxval = outputArr [i];
						pos = i;
					}
				}

				if (pos == 0) transform.GetComponent<clanController> ().attack (currentWarrior);
				else if (pos == 1) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (-1,-1));
				else if (pos == 2) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (0, -1));
				else if (pos == 3) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (1, -1));
				else if (pos == 4) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (-1, 0));
				else if (pos == 5) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (1,  0));
				else if (pos == 6) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (-1, 1));
				else if (pos == 7) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (0,  1));
				else if (pos == 8) transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (1,  1));

				

				//++currentWarrior;
				/*	var steer = (float)outputArr[0] * 2 - 1;
        var gas = (float)outputArr[1] * 2 - 1;

        var moveDist = gas * Speed * Time.deltaTime;
        var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

        transform.Rotate(new Vector3(0, turnAngle, 0));
        transform.Translate(Vector3.forward * moveDist);	 
    */	
			} 
			userBlock = false;
			++currentWarrior;
			currentWarrior %= 10;
			if (currentWarrior == 0 && isUser) {
				userBlock = true;
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "It's your turn";
				if (transform.GetComponent<clanController> ().getWarrior (0).def <= 0) {
					GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You are dead";
					userBlock = false;
					++currentWarrior;
				}
			} 
			itsMyTurn = false;
			enemy.GetComponent<clanNEAT> ().giveMeTurn ();
		} 
	}

	public void Up (){
		
		if (userBlock && itsMyTurn) {
			transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (0, 1));
			++currentWarrior;
			userBlock = false;
			itsMyTurn = false;
			enemy.GetComponent<clanNEAT> ().giveMeTurn ();
			if ( transform.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You lose";
			else if ( enemy.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You win";
			else GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = transform.GetComponent<clanController>().getBP().ToString() + " - " + enemy.GetComponent<clanController>().getBP().ToString();
		}
	}
	public void Down (){
		if (userBlock && itsMyTurn) {
			transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (0,  -1));
			++currentWarrior;
			userBlock = false;
			itsMyTurn = false;
			enemy.GetComponent<clanNEAT> ().giveMeTurn ();
			if ( transform.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You lose";
			else if ( enemy.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You win";
			else GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = transform.GetComponent<clanController>().getBP().ToString() + " - " + enemy.GetComponent<clanController>().getBP().ToString();
		}
	}
	public void Left (){
		if (userBlock && itsMyTurn) {
			transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (-1,0));
			++currentWarrior;
			userBlock = false;
			itsMyTurn = false;
			enemy.GetComponent<clanNEAT> ().giveMeTurn ();
			if ( transform.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You lose";
			else if ( enemy.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You win";
			else GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = transform.GetComponent<clanController>().getBP().ToString() + " - " + enemy.GetComponent<clanController>().getBP().ToString();
		}
	}
	public void Right (){
		if (userBlock && itsMyTurn){ 
			transform.GetComponent<clanController> ().move (currentWarrior, new Vector2 (1,0));
			++currentWarrior;
			userBlock = false;
			itsMyTurn = false;
			enemy.GetComponent<clanNEAT> ().giveMeTurn ();
			if ( transform.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You lose";
			else if ( enemy.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You win";
			else GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = transform.GetComponent<clanController>().getBP().ToString() + " - " + enemy.GetComponent<clanController>().getBP().ToString();
		}
	}
	public void Attack(){
		if (userBlock && itsMyTurn) {
			transform.GetComponent<clanController> ().attack (currentWarrior);
			++currentWarrior;
			userBlock = false;
			itsMyTurn = false;
			enemy.GetComponent<clanNEAT> ().giveMeTurn ();
			if ( transform.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You lose";
			else if ( enemy.GetComponent<clanController>().getBP() <= 0)
				GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = "You win";
			else GameObject.FindGameObjectWithTag("Respawn").GetComponent<Text>().text = transform.GetComponent<clanController>().getBP().ToString() + " - " + enemy.GetComponent<clanController>().getBP().ToString();
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

		if (fit > 0.01f) return fit;
		return 0.01f;
	}
}