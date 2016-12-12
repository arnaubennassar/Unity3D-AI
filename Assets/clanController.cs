using UnityEngine;
using System;
using System.Collections;


public class clanController : MonoBehaviour {


	public int totalWarriors = 10;
	public int aliveWarriors;
	public int totalAtckPwr = 100;
	public int totalDefPwr = 100;
	public int basePower = 2000;
	public Vector2 basePos;
	public bool uniformPowerDistribution = true;
	public Vector3 desp = new Vector3(0.09f,0,0);
	public Vector3 scale  = new Vector3(0.03f,0.03f,0);
	public Vector2 boardSize = new Vector2(10,10);

	public string clanName;
	public Transform enemy;
	public Vector3 origin;
	public Vector3 dOrigin;
	public Vector3 deathPosition;
	public Sprite warriorImg;
	public bool isUser = false;


	int dangerRad = 10;


	float lastMove = 0;
	float lastAttack = 0;

	public Warrior[] warriors;

	bool first = true;
	bool started = false;

	public class Warrior
	{
		public int id;
		public GameObject gO;
		public int atck;
		public int def;
		public Vector2 pos;
	}

	public class maps
	{
		public int[,] attack;
		public int[,] defense;
	}

	void Start(){
		FindObjectOfType<Controller> ().addNewClan (transform);
	}

	public void customStart () {
		
	//	origin = new Vector3 (Random.Range (0.4f, 9.4f), Random.Range (0.4f, 9.4f), 0);
		aliveWarriors = totalWarriors;
		warriors = new Warrior[totalWarriors];
		int ttAtck = totalAtckPwr;
		int ttDef = totalDefPwr;
		for (int i = 0; i < totalWarriors; ++i) {
			warriors [i] = new Warrior ();
			warriors [i].id = i;
			if (uniformPowerDistribution) {
				warriors [i].atck = totalAtckPwr / totalWarriors;
				warriors [i].def = totalDefPwr / totalWarriors;
			} else {
				if (i == totalWarriors - 1) {
					warriors [i].atck = ttAtck;
					warriors [i].def = ttDef;
				} else {
					int aux = UnityEngine.Random.Range (1, ttAtck);
					warriors [i].atck = aux;
					ttAtck -= aux;
					aux = UnityEngine.Random.Range (1, ttDef);
					warriors [i].def = aux;
					ttDef -= aux;
				}
			}
			warriors [i].gO = new GameObject ();
			warriors [i].gO.transform.parent = transform;
			warriors [i].gO.AddComponent<SpriteRenderer> ().sprite = warriorImg;
			warriors [i].gO.transform.position = origin + dOrigin + desp*i;
			warriors [i].pos = new Vector2 ((int)origin.x + (int)dOrigin.x, (int)origin.y + (int) dOrigin.y);
			warriors [i].gO.name = clanName + i.ToString ();
			warriors [i].gO.transform.localScale = scale;
		}
		if (isUser)
			warriors [0].gO.GetComponent<SpriteRenderer> ().color = Color.green;
		basePos = warriors[0].pos;
		started = true;
	}

	void test (){
		Vector2 mv = new Vector2 (1,1);
		move (0, mv);
		move (1, mv * -1);

		move (0, mv);
		move (1, mv * -1);

		move (0, mv);
		move (1, mv * -1);

		move (0, mv);
		move (1, mv * -1);

		//move (0, mv);
		move (1, mv * -1);

		attack (0);
	}

/*	void Update(){
		if (first)
			test ();
		first = false;
	}	*/

	bool checkLimits(Vector2 posi)
	{
		if ((int)posi.x < (int)origin.x || (int)posi.y < (int)origin.y)
			return false;
		if ((int)posi.x >= (int)boardSize.x + (int)origin.x || (int)posi.y >= (int)boardSize.y + (int)origin.y )
			return false;
		return true;
	}

	//return true if the movement is succesful
	public bool move (int warriorId, Vector2 direction)
	{
	//	lastAttack = 0;
		Vector3 aux = Vector3.zero;
		if (direction == Vector2.zero)
			lastMove = -1;
		else if (warriors.Length > warriorId && warriors[warriorId].def > 0)
		{
			if (direction.x > 0)
				aux += new Vector3 (1, 0, 0);
			else if (direction.x < 0)
				aux += new Vector3 (-1, 0, 0);
			if (direction.y > 0)
				aux += new Vector3 (0, 1, 0);
			else if (direction.y < 0)
				aux += new Vector3 (0, -1, 0);
			Vector2 v2aux = new Vector2 (aux.x + warriors [warriorId].pos.x, aux.y + warriors [warriorId].pos.y);
			if (checkLimits (v2aux)) {
				warriors [warriorId].gO.transform.position += aux;
				warriors [warriorId].pos = v2aux;
				lastMove = 1;
				return true;
			}
		}
		lastMove = -1;
		return false;
	}

	bool checkPosition (Vector2 pos, int id){
		if (warriors [id].def <= 0)
			return false;
		float auxX = warriors [id].gO.transform.position.x; 
		if (auxX < pos.x || auxX > pos.x + 1)
			return false;
		auxX = warriors [id].gO.transform.position.y; 
		if (auxX < pos.y || auxX > pos.y + 1)
			return false;
		return true;
	}

	public int getDamage (Vector2 pos, int dmg)
	{
		int selected = -1;
		for (int i = 0; i < totalWarriors; ++i) {
			if (checkPosition (pos, i)) {
				if (selected == -1)
					selected = i;
				else if (warriors [selected].def < warriors [i].def)
					selected = i;
			}
		}
		if (selected != -1) {
			if (warriors [selected].def > dmg) {
				warriors [selected].def -= dmg;
				totalDefPwr -= dmg;
				return dmg;
			} else {
				int auxDmg = warriors [selected].def;
				totalDefPwr -= auxDmg;
				totalAtckPwr -= warriors [selected].atck;
				warriors [selected].def = -1;
				warriors [selected].gO.transform.position = deathPosition;
				--aliveWarriors;
				return auxDmg;
			}
		} else if (Vector2.Distance (pos, basePos) < 1.5f) {
			Debug.Log ("making damage to the base");
			basePower -= dmg;
			return dmg;
		}
		return -1;
	}

	//return damage recived by the enemy
	public int attack (int warriorId){
		int auxDmg;
		if (warriorId < 0 || warriorId >= totalWarriors)
			auxDmg = 0;
		if (warriors [warriorId].def <= 0)
			auxDmg = 0;
		Vector2 aux = new Vector2 (warriors [warriorId].gO.transform.position.x, warriors [warriorId].gO.transform.position.y);
		auxDmg = enemy.GetComponent<clanController>().getDamage (aux, warriors [warriorId].atck);
		Debug.Log (auxDmg + " damage infricted by clan "+ clanName);
		if (auxDmg == 0)
			lastAttack = -1;
		else
			lastAttack = 1;
		//lastMove = 0;
		return auxDmg;
	}

	public int getWarriorState (int id){
		if (id >= totalWarriors || id < 0)
			return -1;
		if (warriors [id].def < 0)
			return 0;
		return 1;
	}

	public Warrior getWarrior (int id) {
		if (id >= totalWarriors || id < 0) {
			Warrior aux = new Warrior ();
			aux.def = -1;
			aux.id = -1;
			return aux;
		}
		return warriors [id];
	}

	public maps getMap (){
		int[,] myMapAttack = getMyMap(true);
		int[,] enemyMapAttack = enemy.GetComponent<clanController>(). getMyMap(true);
		int[,] myMapDefense = getMyMap(false);
		int[,] enemyMapDefense = enemy.GetComponent<clanController>(). getMyMap(false);
		for (int i = 0; i < boardSize.x; ++i) {
			for (int j = 0; j < boardSize.y; ++j) {
				myMapAttack [i,j] -= enemyMapAttack [i,j];
				myMapDefense [i,j] -= enemyMapDefense [i,j];
			}
		}
		maps ret = new maps ();
		ret.attack = myMapAttack;
		ret.defense = myMapDefense;
		return ret;
	}
	public int[,] getMyMap (bool attack){
		int[,] map = new int[(int)boardSize.x, (int)boardSize.y];
		for (int i = 0; i < boardSize.x; ++i) {
			for (int j = 0; j < boardSize.y; ++j) {
				map [i,j] = 0;
			}
		}
		for (int i = 0; i < totalWarriors; ++i) {
			
			if (warriors [i].def > 0) {
		//		
				if (attack){
					try{
						map [(int)warriors [i].pos.x - (int)origin.x, (int)warriors [i].pos.y -(int) origin.y] = warriors [i].atck;
					}
					catch(Exception e){
						Debug.Log ("warrior pos "+warriors [i].pos + " origin "+origin);
					}
				}
				else
					map [(int)warriors [i].pos.x - (int)origin.x, (int)warriors [i].pos.y - (int)origin.y] = warriors [i].def;
			}
		}
		return map;
	}

	float getDanger (int id, Vector2 pos){
		float ret = 0;
		int tw = enemy.GetComponent<clanController> ().totalWarriors;
		for (int i = 0; i < tw; ++i) {
			if (Vector2.Distance (pos, enemy.GetComponent<clanController> ().warriors [i].pos) <= dangerRad) {
				ret += enemy.GetComponent<clanController> ().warriors [i].atck / Vector2.Distance (pos, enemy.GetComponent<clanController> ().warriors [i].pos);
			}
		}
		return ret;
	}

	float getSuperiority (Vector2 pos){
		float ret = 0;
		int tw = totalWarriors;
		for (int i = 0; i < tw; ++i) {
			if (Vector2.Distance (pos, warriors [i].pos) <= dangerRad) {
				ret += warriors [i].atck / Vector2.Distance (pos, warriors [i].pos);
			}
		}
		return ret;
	}

	public float[] getInput (int id){
		float[] ret = new float[25];
		Vector2 pos = warriors [id].pos;
		Vector2 eBase = enemy.GetComponent<clanController> ().basePos;
		int i = 0;
		for (int auxi = 0; auxi < 9; ++auxi){	//for all possible directions
			i = auxi;
			if (auxi > 4)
				--i;
			if (i != 4) {
				Vector2 vaux = pos + new Vector2 (i % 3 - 1, i / 3 - 1);
				if (checkLimits (vaux)) {
					ret [i * 3 + 0] = (1 / Vector2.Distance (new Vector2 (), eBase)) * 100;	//distance to eBase
					ret [i * 3 + 1] = getDanger(id, vaux);									//danger
					ret [i * 3 + 2] = getSuperiority(vaux);									//superiority
				} else {
					ret [i * 3 + 0] = 0;
					ret [i * 3 + 1] = 0;
					ret [i * 3 + 2] = 0;
				}
			}
		}
		if (enemy.GetComponent<clanController>().getDamage(pos, 0) == 0) ret [24] = 100;	//attack input
		else ret [24] = 0;
		return ret;
	}

	public float fitness (int id){

		float fit = 0;
		if (started) {
			float aux = 0;
			if (lastMove == -1)
				aux += 1000;
			else if (lastAttack == 1)
				aux = -1;
			lastMove = 0;
			lastAttack = 0;
			fit -= aux;

			//AVG distance of warriors to enemy Base
			Vector2 eBase = enemy.GetComponent<clanController> ().basePos;
			float avgD = 0;
			int count = 0;
			for (int i = 0; i < totalWarriors; ++i) {
				if (warriors [i].def > 0) {
					avgD += Vector2.Distance (warriors [i].pos, eBase);
					++count;
				}
			}
			avgD = avgD / count;
			if (avgD != 0)
				fit += 100 / avgD;

			fit += (float)basePower - (float)enemy.GetComponent<clanController> ().basePower ;

			float difPwr = (float)(totalAtckPwr + totalDefPwr) - (float)(enemy.GetComponent<clanController> ().totalAtckPwr + enemy.GetComponent<clanController> ().totalDefPwr);
			fit += difPwr;	
			if (enemy.GetComponent<clanController> ().getBP() == 0) {
				Debug.Log ("GG \n\n\n\n\n\n");
				return 10000;
			} else if (basePower == 0) {
				Debug.Log ("DAAAAAMM ");
				return 0;
			}	
		}
		if (float.IsInfinity (fit))
			Debug.Log ("wat da fak" + fit);
		return Mathf.Max(0, fit);
	}

	public int getBP(){
		return basePower;
	}
}
