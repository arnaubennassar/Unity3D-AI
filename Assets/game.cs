using UnityEngine;
using System.Collections;
using System.Xml;

public class game : MonoBehaviour {
	public bool isGame = false;
	public GameObject clan;
	GameObject clanA;
	GameObject clanB;
	public Sprite baseA;
	public Sprite baseB;
	public Sprite warriorImgA;
	public Sprite warriorImgB;
	gridCreator grid;
	// Use this for initialization
	void Start () {
		if (isGame) {
		//	GameObject.FindGameObjectWithTag ("Finish").gameObject.SetActive (false);
			clanA = GameObject.FindGameObjectWithTag("Finish").GetComponent<Optimizer>().customRB();//Instantiate (clan) as GameObject;
			clanB = GameObject.FindGameObjectWithTag("Finish").GetComponent<Optimizer>().customRB();//Instantiate (clan) as GameObject;

			grid = new gridCreator ();
			grid.createGrid (new Vector3(0.5f,-20,0), 10, 10, Color.white, Color.blue, 0, baseA, baseB);
			clanA.GetComponent<clanController>(). origin = new Vector3 (0f, -20, 0);
			clanA.GetComponent<clanController>().dOrigin = new Vector3 (0.1f, 0, 0); 
			clanA.GetComponent<clanController>().deathPosition = new Vector3 (-0.1f, -20, 0); 
			clanA.GetComponent<clanController>().warriorImg = warriorImgA;
			clanA.GetComponent<clanController> ().enemy = clanB.transform;

			clanB.GetComponent<clanController>().origin = new Vector3 (0f, -20, 0);
			clanB.GetComponent<clanController>().dOrigin = new Vector3 (9.1f, 9, 0);
			clanB.GetComponent<clanController>().deathPosition = new Vector3 (10.7f, -11, 0); 
			clanB.GetComponent<clanController>().warriorImg = warriorImgB;
			clanB.GetComponent<clanController> ().enemy = clanA.transform;
			clanB.GetComponent<clanController> ().isUser = true;
			clanB.GetComponent<clanController> ().scale = new Vector3 (0.04f,0.04f,1);

			clanA.GetComponent<clanNEAT>().itsMyTurn = true;
			clanA.GetComponent<clanNEAT> ().enemy = clanB.transform;
			/*
			SimpleExperiment experiment = new SimpleExperiment();
			XmlDocument xmlConfig = new XmlDocument();
			TextAsset textAsset = (TextAsset)Resources.Load("experiment.config");
			xmlConfig.LoadXml(textAsset.text);
			experiment.SetOptimizer( GameObject.FindGameObjectWithTag("Finish").GetComponent<Optimizer>() );

			experiment.Initialize("War Experiment", xmlConfig.DocumentElement, 25, 9);
			clanA.GetComponent<clanNEAT> ().box = Utility.LoadBrain ("/Users/arnaubennassar/Library/Application Support/DefaultCompany/PCG project/car.champ.xml", experiment );
			*/
			clanB.GetComponent<clanNEAT> ().itsMyTurn = false;
			clanB.GetComponent<clanNEAT> ().isUser = true;
			clanB.GetComponent<clanNEAT> ().enemy = clanA.transform;
			//clanB.GetComponent<clanNEAT> ().box = Utility.LoadBrain (@"/Users/arnaubennassar/Library/Application Support/DefaultCompany/PCG project/car.champ.xml", experiment );

			clanA.GetComponent<clanController> ().customStart ();
			clanB.GetComponent<clanController> ().customStart ();

			Camera.main.transform.position = new Vector3 (5.25f, -15.5f, -10);

			/*
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
			*/
		}
		else GameObject.FindGameObjectWithTag ("UaI").gameObject.SetActive (false);
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
	public void attack () {
		clanB.GetComponent<clanNEAT> ().Attack ();
	}
}
