using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

	public bool isGame = false;

	public int rows = 10;
	public int cols = 10;

	public Color colorA = Color.black;
	public Color colorB = Color.white;

	public Sprite baseA;
	public Sprite baseB;

	public Sprite warriorImgA;
	public Sprite warriorImgB;

	int currentClan = 0;
	bool isA = true;

	int totalBoards = 0;


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
		pos -= desp;
	}

	public void genUp(){
		currentClan = 0;
		isA = true;
		pos = new Vector3(0,0,0) - desp;
	}

	void addPlayer(Transform clan){
//		Debug.Log (clan.name+ " ADD clan " + isA + " isFirst " + isFirst + " currentA " + currentClan + " currentB " + currentClan);
		if (isA) {
			games [currentClan].clanA = clan.GetComponent<clanController> ();
			games [currentClan].clanNA = clan.GetComponent<clanNEAT> ();
			games [currentClan].traA = clan;

			clan.name = "clan A_" + currentClan.ToString ();
			games [currentClan].clanA.clanName = "clan A_" + currentClan.ToString ();
			games [currentClan].clanA.origin = games [currentClan].ori; 
			games [currentClan].clanA.dOrigin = new Vector3 (0.1f, 0, 0);
			games [currentClan].clanA.deathPosition = new Vector3 (-1, 0, 0) + games[currentClan].ori;
			games [currentClan].clanA.warriorImg = warriorImgA;
			games [currentClan].clanNA.itsMyTurn = true;

		} else {
			games [currentClan].clanB = clan.GetComponent<clanController> ();
			games [currentClan].clanNB = clan.GetComponent<clanNEAT> ();
			games [currentClan].traB = clan;

			games [currentClan].clanB.clanName = "clan B_" + currentClan.ToString ();
			games [currentClan].clanB.origin = games [currentClan].ori;
			games [currentClan].clanB.dOrigin = new Vector3 (9.1f, 9, 0);
			games [currentClan].clanB.deathPosition = new Vector3 (11, 9, 0) + games[currentClan].ori;
			games [currentClan].clanB.warriorImg = warriorImgB;


			games [currentClan].clanNB.itsMyTurn = false;
			games [currentClan].clanA.enemy = clan;
			games [currentClan].clanNA.enemy = clan;
			games [currentClan].clanB.enemy = games [currentClan].traA;
			games [currentClan].clanNB.enemy = games [currentClan].traA;

			games [currentClan].clanA.customStart ();
			games [currentClan].clanB.customStart ();

		}
	}

	public void addNewClan(Transform clan){
		if (isGame) {
			if (isA) {
				if (totalBoards <= currentClan) {
					games [currentClan] = new game ();
					pos += desp;
					games [currentClan].ori = pos;
					grid.createGrid (pos + new Vector3 (0.5f, 0, 0), 10, 10, colorA, colorB, totalBoards, baseA, baseB);
				}
				addPlayer (clan);
				isA = !isA;

			} else {
				clan.name = "clan B_" + currentClan.ToString ();
				clan.GetComponent<clanController> ().clanName = "clan B_" + currentClan.ToString ();
				addPlayer (clan);
				++currentClan;
				if (currentClan > totalBoards) {
					++totalBoards;
					Debug.Log (totalBoards + " boards created");
				}
				isA = !isA;
			}
		}
	}
}
