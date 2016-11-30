using UnityEngine;
using System.Collections;

public class clanController : MonoBehaviour {


	public int totalWarriors = 10;
	public int aliveWarriors;
	public int totalAtckPwr = 100;
	public int totalDefPwr = 100;
	public bool uniformPowerDistribution = true;
	public Vector3 desp = new Vector3(0.09f,0,0);
	public Vector3 scale  = new Vector3(0.03f,0.03f,0);
	public Vector2 boardSize = new Vector2(10,10);

	public string clanName;
	public Transform enemy;
	public Vector3 origin;
	public Vector3 deathPosition;
	public Sprite warriorImg;

	float lastMove = 0;
	float lastAttack = 0;

	public Warrior[] warriors;

	bool first = true;

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

/*	void Start(){
		enemy = null;
		FindObjectOfType<Controller> ().addNewClan (transform);
	}
*/
	public void Start () {
		origin = new Vector3 (Random.Range (0.4f, 9.4f), Random.Range (0.4f, 9.4f), 0);
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
					int aux = Random.Range (1, ttAtck);
					warriors [i].atck = aux;
					ttAtck -= aux;
					aux = Random.Range (1, ttDef);
					warriors [i].def = aux;
					ttDef -= aux;
				}
			}
			warriors [i].gO = new GameObject ();
			warriors [i].gO.AddComponent<SpriteRenderer> ().sprite = warriorImg;
			warriors [i].gO.transform.position = origin + desp*i;
			warriors [i].pos = new Vector2 ((int)origin.x, (int) origin.y);
			warriors [i].gO.name = clanName + i.ToString ();
			warriors [i].gO.transform.localScale = scale;
		}
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

	bool checkLimits(Vector3 posi)
	{
		if (posi.x < 0 || posi.y < 0)
			return false;
		if (posi.x >= boardSize.x || posi.y >= boardSize.y)
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
			Vector3 vaux = warriors [warriorId].gO.transform.position;
			warriors [warriorId].gO.transform.position += aux;
			if (!checkLimits (warriors [warriorId].gO.transform.position)) {
				warriors [warriorId].gO.transform.position = vaux;
				warriors [warriorId].pos = new Vector2 ((int)vaux.x, (int) vaux.y);
				lastMove = -1;
				return false;
			}
			lastMove = 1;
			return true;
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
		}
		return 0;
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
				if (attack)
					map [(int)warriors [i].pos.x, (int)warriors [i].pos.y] = warriors [i].atck;
				else
					map [(int)warriors [i].pos.x, (int)warriors [i].pos.y] = warriors [i].def;
			}
		}
		return map;
	}

	public float fitness (int id){
		float aux = 0;
		if (lastMove == -1)
			aux += 0.5f;
		if (lastAttack == -1)
			aux += 0.5f;
		else if (lastAttack == 1)
			aux = -1;
		lastMove = 0;
		lastAttack = 0;
		return 1 - (Vector2.Distance (warriors [id].pos, enemy.GetComponent<clanController>().warriors[id].pos) / 14) - aux;
	}
}
