using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

	public int rows = 10;
	public int cols = 10;

	public Color colorA = Color.black;
	public Color colorB = Color.white;

	public Sprite warriorImgA;
	public Sprite warriorImgB;

	int currentClanA = 0;
	int currentClanB = 0;


	Vector3 pos = new Vector3(0,0,0);
	Vector3 desp = new Vector3(11,0,0);
	gridCreator grid;
	// Use this for initialization

	struct game {
		public Vector3 ori;
		public clanController clanA;
		public clanController clanB;
		public clanNEAT clanNA;
		public clanNEAT clanNB;
		public Transform traA;
		public Transform traB;
	}

	game[] games;


	void Start(){
		grid = new gridCreator ();
		games = new game[10000];
		games [currentClanA] = new game ();
	//	pos += desp;
		games [currentClanA].ori = pos;
		grid.createGrid (pos, 10, 10, colorA, colorB);
		//grid.createGrid (pos, 10, 10, colorA, colorB);
	}

	void addPlayer(Transform clan, bool isA, bool isFirst){
		Debug.Log (clan.name+ " ADD clan " + isA + " isFirst " + isFirst + " currentA " + currentClanA + " currentB " + currentClanB);
		if (isA) {
			games [currentClanA].clanA = clan.GetComponent<clanController> ();
			games [currentClanA].clanNA = clan.GetComponent<clanNEAT> ();
			games [currentClanA].traA = clan;


			games [currentClanA].clanA.clanName = "clan A_" + currentClanA.ToString ();
			games [currentClanA].clanA.origin = new Vector3 (-0.4f, 0, 0) + games [currentClanA].ori;
			games [currentClanA].clanA.deathPosition = new Vector3 (-1, 0, 0) + games[currentClanA].ori;
			games [currentClanA].clanA.warriorImg = warriorImgA;

			games [currentClanA].clanNA.itsMyTurn = true;
			if (!isFirst) {
				games [currentClanA].clanA.enemy = games [currentClanA].traB;
//				games [currentClanA].clanNA.enemyNeat = games [currentClanA].traB;
				games [currentClanA].clanB.enemy = games [currentClanA].traA;
//				games [currentClanA].clanNB.enemyNeat = games [currentClanA].traA;

//				games [currentClanA].clanA.customStart ();
//				games [currentClanA].clanB.customStart ();
			}
		} else {
			games [currentClanB].clanB = clan.GetComponent<clanController> ();
			games [currentClanB].clanNB = clan.GetComponent<clanNEAT> ();
			games [currentClanB].traB = clan;

			games [currentClanB].clanB.clanName = "clan B_" + currentClanB.ToString ();
			games [currentClanB].clanB.origin = new Vector3 (8.6f, 9, 0) + games [currentClanB].ori;
			games [currentClanB].clanB.deathPosition = new Vector3 (10, 9, 0) + games[currentClanB].ori;
			games [currentClanB].clanB.warriorImg = warriorImgB;

			games [currentClanB].clanNB.itsMyTurn = false;
			if (!isFirst) {
				games [currentClanB].clanA.enemy = games [currentClanA].traB;
		//		games [currentClanB].clanNA.enemyNeat = games [currentClanA].traB;
				games [currentClanB].clanB.enemy = games [currentClanA].traA;
		//		games [currentClanB].clanNB.enemyNeat = games [currentClanA].traA;

	//			games [currentClanB].clanA.customStart ();
	//			games [currentClanB].clanB.customStart ();
			}
		}
	}

	public void addNewClan(Transform clan){
		if (clan.name == "clan A(Clone)") {
			clan.name = "clan A_" + currentClanA.ToString ();
			if (currentClanA <= currentClanB && !(currentClanA == 0 && currentClanB == 0) )
				addPlayer (clan, true, false);
			else {
				games [currentClanA] = new game ();
				pos += desp;
				games [currentClanA].ori = pos;
				grid.createGrid (pos, 10, 10, colorA, colorB);
				addPlayer (clan, true, true);
			}
			++currentClanA;

		} else {
			clan.name = "clan B_" + currentClanB.ToString ();
			if (currentClanB <= currentClanA && !(currentClanA == 0 && currentClanB == 0))
				addPlayer (clan, false, false);
			else {
				games [currentClanB] = new game ();
				pos += desp;
				games [currentClanA].ori = pos;
				grid.createGrid (pos, 10, 10, colorA, colorB);
				addPlayer (clan, false, true);
			}
			++currentClanB;
		}
	}
}
