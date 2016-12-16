using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class game : MonoBehaviour {
	public bool isGame = false;
	public GameObject clan;
	GameObject clanA;
	GameObject clanB;
	public Sprite baseA;
	public Sprite baseB;
	public Sprite warriorImgA;
	public Sprite warriorImgB;
	public gridCreator grid;
	GameObject ui;
	public Controller cont;
	// Use this for initialization
	public void Start () {
		try{
			ui = GameObject.FindGameObjectWithTag ("UaI");
		}
		catch(Exception e){
		}
		if (isGame) {
			cont.isGame = false;
			clanA = GameObject.FindGameObjectWithTag ("Finish").GetComponent<Optimizer> ().customRB ();//Instantiate (clan) as GameObject;
			clanB = GameObject.FindGameObjectWithTag ("Finish").GetComponent<Optimizer> ().customRB ();//Instantiate (clan) as GameObject;

			//grid = new gridCreator ();
			grid.createGrid (new Vector3 (0.5f, -20, 0), 10, 10, Color.white, Color.blue, 0, baseA, baseB);
			clanA.GetComponent<clanController> ().origin = new Vector3 (0f, -20, 0);
			clanA.GetComponent<clanController> ().dOrigin = new Vector3 (0.1f, 0, 0); 
			clanA.GetComponent<clanController> ().deathPosition = new Vector3 (-0.1f, -20, 0); 
			clanA.GetComponent<clanController> ().warriorImg = warriorImgA;
			clanA.GetComponent<clanController> ().enemy = clanB.transform;

			clanB.GetComponent<clanController> ().origin = new Vector3 (0f, -20, 0);
			clanB.GetComponent<clanController> ().dOrigin = new Vector3 (9.1f, 9, 0);
			clanB.GetComponent<clanController> ().deathPosition = new Vector3 (10.7f, -11, 0); 
			clanB.GetComponent<clanController> ().warriorImg = warriorImgB;
			clanB.GetComponent<clanController> ().enemy = clanA.transform;
			clanB.GetComponent<clanController> ().isUser = true;
			clanB.GetComponent<clanController> ().scale = new Vector3 (0.04f, 0.04f, 1);

			clanA.GetComponent<clanNEAT> ().itsMyTurn = true;
			clanA.GetComponent<clanNEAT> ().enemy = clanB.transform;

			clanB.GetComponent<clanNEAT> ().itsMyTurn = false;
			clanB.GetComponent<clanNEAT> ().isUser = true;
			clanB.GetComponent<clanNEAT> ().userBlock = true;
			clanB.GetComponent<clanNEAT> ().enemy = clanA.transform;

			clanA.GetComponent<clanController> ().customStart ();
			clanB.GetComponent<clanController> ().customStart ();

			Camera.main.transform.position = new Vector3 (5.25f, -15.5f, -10);
			ui.GetComponent<Canvas> ().enabled = true;
			
		} else
			ui.GetComponent<Canvas> ().enabled = false;
	}
	
	public void up () {
		clanB.GetComponent<clanNEAT> ().Up ();
	}
	public void down () {
		clanB.GetComponent<clanNEAT> ().Down ();
	}
	public void left () {
		clanB.GetComponent<clanNEAT> ().Left ();
	}
	public void right () {
		clanB.GetComponent<clanNEAT> ().Right ();
	}
	public void upR () {
		clanB.GetComponent<clanNEAT> ().UpRight ();
	}
	public void downL () {
		clanB.GetComponent<clanNEAT> ().DownLeft ();
	}
	public void uleft () {
		clanB.GetComponent<clanNEAT> ().UpLeft ();
	}
	public void dright () {
		clanB.GetComponent<clanNEAT> ().DownRight ();
	}

	public void attack () {
		clanB.GetComponent<clanNEAT> ().Attack ();
	}

	public void destroyThem() {
		Destroy (clanA);
		Destroy (clanB);
	}
}
