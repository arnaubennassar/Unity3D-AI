using UnityEngine;
using System.Collections;

public class gridCreator : MonoBehaviour {
	/*
	public int rows = 10;
	public int cols = 10;

	public Color colorA = Color.black;
	public Color colorB = Color.white;
	*/

	Shader shad;

	// Use this for initialization
	public void createGrid (Vector3 ori, int rows, int cols, Color colorA, Color colorB) {
		Vector3 pos = ori;
		shad = Shader.Find("Unlit/Color");
		Vector3 rowIncrease = new Vector3 (1, 0, 0);
		Vector3 colIncrease = new Vector3 (0, 1, 0);
		bool colorAorBcol = true;
		bool colorAorBrow = true;
		for (int i = 0; i < rows; ++i) {
			for (int j = 0; j < cols; ++j) {
				GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				quad.transform.position = pos;
				pos += colIncrease;
				quad.GetComponent<Renderer> ().material.shader = shad;
				if (colorAorBcol)
					quad.GetComponent<Renderer> ().material.SetColor ("_Color", colorA);
				else quad.GetComponent<Renderer> ().material.SetColor ("_Color", colorB);
				colorAorBcol = !colorAorBcol;
			}
			pos = ori + rowIncrease*(i+1);
			colorAorBcol = !colorAorBrow;
			colorAorBrow = !colorAorBrow;
		}
	}

}
