using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Atom {
	public Vector3 pos;
	public Color col;
	public int index;
}

public class Simulator : MonoBehaviour {
	public int totalAtoms;
	private LineRenderer[] lines;
	private Atom[] atoms;
	public GameObject line;
	public int steps;
	public bool withColor;
	[Range(0.01f, 1f)]
	public float waitTime;
	private bool isPaused;
	[Range(0.1f, 15f)]
	public float minMov, maxMov;
	private List<GameObject> allAtoms = new List<GameObject>();
 
	void Awake() {
		lines = new LineRenderer[totalAtoms];
		atoms = new Atom[totalAtoms];
	}

	[ContextMenu("Begin")]
	public void BeginSimulation() {
		isPaused = false;
		StopSimulation();
		lines = new LineRenderer[totalAtoms];
		atoms = new Atom[totalAtoms];
		for(int i=0; i<totalAtoms; i++) {
			atoms[i].col = Random.ColorHSV();
			atoms[i].index = i;
			GameObject currentLine = Instantiate(line);
			lines[i] = currentLine.GetComponent<LineRenderer>();
			StartCoroutine(SpawnLater(atoms[i]));
		}
	}
	
	[ContextMenu("Stop")]
	public void StopSimulation() {
		StopAllCoroutines();
		foreach(GameObject current in allAtoms) {
			Destroy(current);
		}
		foreach(LineRenderer line in lines) {
			if(line!=null)
			line.positionCount = 0;
		}
		allAtoms.Clear();
	}
	[ContextMenu("Pause")]
	public void PauseSimulation() {
		if(!isPaused) {
		Time.timeScale = 0f;
		isPaused = true;
		}
		else {
		Time.timeScale = 1f;
		isPaused = false;
		}
	}
	private IEnumerator SpawnLater(Atom atoms) {
	for(int i=0; i<steps; i++) {
		atoms = CalculateAtom(atoms);
		SpawnSphere(atoms);
		yield return new WaitForSeconds(waitTime);
	}
	}
	public Atom CalculateAtom(Atom atoms) {
		float alpha, theta, mov;
		alpha = Random.Range(0, 2*Mathf.PI);
		theta = Random.Range(-Mathf.PI, Mathf.PI);
		mov = Random.Range(minMov, maxMov);
		Vector3 newMovement = new Vector3(mov*Mathf.Sin(theta)*Mathf.Cos(alpha), mov*Mathf.Sin(theta)*Mathf.Sin(alpha), mov*Mathf.Cos(theta));
		atoms.pos += newMovement;
		return atoms;
	}

	public void SpawnSphere(Atom atoms) {
		lines[atoms.index].positionCount++;
		lines[atoms.index].SetPosition(lines[atoms.index].positionCount-1, atoms.pos);
		if(withColor) {
			Color randomColor = Random.ColorHSV();
			lines[atoms.index].startColor = randomColor;
			lines[atoms.index].endColor = randomColor;
		}
	}
}
