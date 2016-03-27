using UnityEngine;
using System.Collections;

public class GridBrain : MonoBehaviour {

	public static int w = 6;
	public static int h = 20;
	public static Transform[,] grid = new Transform[w,h];
	public GameObject cursorForPosition;

	public GameObject[] candy;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("checkToDelete", 5f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("t")){
			Destroy (grid[1,1].gameObject);
			grid [1, 1] = null;
		}
		checkearFichaParaBajar ();
	}

	public static Vector2 roundVec2(Vector2 v) {
		return new Vector2(Mathf.Round(v.x),
			Mathf.Round(v.y));
	}

	public void spawnLine(int lineNumber){
		float offset = 0;
		if (MainControlFlow.actualPlayerToSpanw == 2) {
			offset = 12;
		}
		for (int i = 0; i < w; i++) {
			int j = Random.Range(0, candy.Length);	
			Vector3 position = new Vector3 (offset + i, lineNumber, 0);
			Instantiate (candy [j], position, Quaternion.identity);
		}
	}
		
	public void deleteFicha(int[] toDelete){
		for (int i = 0; i < toDelete.Length; i+=2) {
			Destroy (grid [(int)toDelete [i],(int) toDelete [i + 1]].gameObject);
			grid [(int)toDelete[i], (int) toDelete[i+1]] = null;
		}
	}

	public void deleteFicha(TuplaPosicion[] toDelete){
		for (int i = 0; i < toDelete.Length; i++) {
			if (toDelete [i] != null) {
				Destroy (grid [toDelete [i].posicionX, toDelete [i].posicionY].gameObject);
				grid [toDelete [i].posicionX, toDelete [i].posicionY] = null;
			}
		}
	}

	void checkearFichaParaBajar(){
		for (int i = 1; i < h; i++) {
			for (int j = 0; j < w; j++) {
				bajarFicha (j, i);
			}
		}
	}

	void bajarFicha(int posx, int posy){
		while (grid[posx, posy - 1] == null && posy - 1 >= 0 && grid[posx, posy] != null) {
			grid [posx, posy -1] = grid [posx, posy];
			grid [posx, posy -1].position += new Vector3 (0, -1, 0);
			grid [posx, posy] = null;
		}
	}

	void checkToDelete(){
		goAgain:
		object[] fiveH;
		object[] fourH;
		object[] threeH;
		object[] fiveV;
		object[] fourV;
		object[] threeV;
		TuplaPosicion[] fichasABorrar = new TuplaPosicion[1000];
		int lastIndex = 0;

		for (int i = 0; i < Main.height; i++) {
			for (int j = 0; j < w; j++) {
				fiveH = checkFiveH(j,i);
				if ((bool)fiveH[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fiveH [1];
					for (int k = 0; k < 5; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					goto vertical;
				}
				fourH = checkFourH(j,i);
				if ((bool)fourH[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fourH [1];
					for (int k = 0; k < 4; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					goto vertical;
				}
				threeH = checkThreeH(j,i);
				if ((bool)threeH[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) threeH [1];
					for (int k = 0; k < 3; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					goto vertical;
				}
				vertical:
				fiveV = checkFiveH(j,i);
				if ((bool)fiveV[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fiveV [1];
					for (int k = 0; k < 5; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					continue;
				}
				fourV = checkFourV(j,i);
				if ((bool)fourV[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) fourV [1];
					for (int k = 0; k < 4; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					continue;
				}
				threeV = checkThreeV(j,i);
				if ((bool)threeV[0]){
					TuplaPosicion[] tuplas = (TuplaPosicion[]) threeV [1];
					for (int k = 0; k < 3; k++) {
						fichasABorrar [lastIndex] = tuplas [k];
						lastIndex++;
					}
					continue;
				}
			}
		}
		removeDuplatedTuples (fichasABorrar);
		deleteFicha (fichasABorrar);
	}

	void removeDuplatedTuples(TuplaPosicion[] tuplas){
		for (int i = 0; i < tuplas.Length; i++) {
			for (int j = 0; j < tuplas.Length; j++) {
				if (i == j) {
					continue;
				}
				if (tuplas[i] != null && tuplas [i].compararTupla (tuplas [j])) {
					tuplas [j] = null;
				}
			}
		}
	}

	object[] checkFiveH(int posx, int posy){
		if (posx + 4 < 6 ) {
			if (grid[posx, posy] != null && grid[posx+1, posy] != null && grid[posx+2, posy] != null && 
				grid[posx+3, posy] != null && grid[posx+3, posy] != null) {
				if (grid [posx, posy].tag == grid [posx + 1, posy].tag &&
					grid[posx, posy].tag == grid [posx +2, posy].tag &&
					grid[posx +2, posy].tag == grid[posx +3, posy].tag &&
					grid[posx +3, posy].tag == grid[posx +4, posy].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					TuplaPosicion fic1 = new TuplaPosicion (posx, posy);
					TuplaPosicion fic2 = new TuplaPosicion (posx+1, posy);
					TuplaPosicion fic3 = new TuplaPosicion (posx+2, posy);
					TuplaPosicion fic4 = new TuplaPosicion (posx+3, posy);
					TuplaPosicion fic5 = new TuplaPosicion (posx+4, posy);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4, fic5 };
					object[] result = {true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	object[] checkFourH(int posx, int posy){
		if (posx + 3 < 6 ) {
			if (grid[posx, posy] != null && grid[posx+1, posy] != null && grid[posx+2, posy] != null && grid[posx+3, posy] != null) {
				if (grid [posx, posy].tag == grid [posx + 1, posy].tag &&
					grid[posx, posy].tag == grid [posx +2, posy].tag &&
					grid[posx +2, posy].tag == grid[posx +3, posy].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					TuplaPosicion fic1 = new TuplaPosicion (posx, posy);
					TuplaPosicion fic2 = new TuplaPosicion (posx+1, posy);
					TuplaPosicion fic3 = new TuplaPosicion (posx+2, posy);
					TuplaPosicion fic4 = new TuplaPosicion (posx+3, posy);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4 };
					object [] result = {true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	object[] checkThreeH(int posx, int posy){
		if (posx + 2 < 6 ) {
			if (grid[posx, posy] != null && grid[posx+1, posy] != null && grid[posx+2, posy] != null) {
				if (grid [posx, posy].tag == grid [posx + 1, posy].tag &&
					grid[posx, posy].tag == grid [posx +2, posy].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					TuplaPosicion fic1 = new TuplaPosicion (posx, posy);
					TuplaPosicion fic2 = new TuplaPosicion (posx+1, posy);
					TuplaPosicion fic3 = new TuplaPosicion (posx+2, posy);
					TuplaPosicion[] array = { fic1, fic2, fic3 };
					object [] result = {true, array};
					return result;				
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	object[] checkFiveV(int posx, int posy){
		if (true){//posy + 4 < 8 ) {
			if (grid[posx, posy] != null && grid[posx, posy+1] != null && grid[posx, posy+2] != null && 
				grid[posx, posy+3] != null && grid[posx, posy+4] != null) {
				if (grid [posx, posy].tag == grid [posx , posy+ 1].tag &&
					grid[posx, posy].tag == grid [posx , posy+2].tag &&
					grid[posx, posy].tag == grid [posx , posy+3].tag &&
					grid[posx, posy].tag == grid [posx , posy+4].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					TuplaPosicion fic1 = new TuplaPosicion (posx, posy);
					TuplaPosicion fic2 = new TuplaPosicion (posx, posy+ 1);
					TuplaPosicion fic3 = new TuplaPosicion (posx, posy+ 2);
					TuplaPosicion fic4 = new TuplaPosicion (posx, posy+ 3);
					TuplaPosicion fic5 = new TuplaPosicion (posx, posy+ 4);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4, fic5 };
					object[] result = { true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	object[] checkFourV(int posx, int posy){
		if (true){//posy + 3 < 8 ) {
			if (grid[posx, posy] != null && grid[posx, posy+1] != null && grid[posx, posy+2] != null && 
				grid[posx, posy+3] != null) {
				if (grid [posx, posy].tag == grid [posx , posy+ 1].tag &&
					grid[posx, posy].tag == grid [posx , posy+2].tag &&
					grid[posx, posy].tag == grid [posx , posy+3].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					TuplaPosicion fic1 = new TuplaPosicion (posx, posy);
					TuplaPosicion fic2 = new TuplaPosicion (posx, posy+ 1);
					TuplaPosicion fic3 = new TuplaPosicion (posx, posy+ 2);
					TuplaPosicion fic4 = new TuplaPosicion (posx, posy+ 3);
					TuplaPosicion[] array = { fic1, fic2, fic3, fic4 };
					object[] result = { true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	object[] checkThreeV(int posx, int posy){
		if (true){///posy + 2 < 8 ) {
			if (grid[posx, posy] != null && grid[posx, posy+1] != null && grid[posx, posy+2] != null) {
				if (grid [posx, posy].tag == grid [posx , posy+ 1].tag &&
					grid[posx, posy].tag == grid [posx , posy+2].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					TuplaPosicion fic1 = new TuplaPosicion (posx, posy);
					TuplaPosicion fic2 = new TuplaPosicion (posx, posy+ 1);
					TuplaPosicion fic3 = new TuplaPosicion (posx, posy+ 2);
					TuplaPosicion[] array = { fic1, fic2, fic3 };
					object[] result = { true, array};
					return result;
				}
			}
		}
		object[] result2 = {false};
		return result2;
	}

	/* Aca lo que deberia hacer es que los metodos de check devuelvan un array de objects
	//donde el object[0] es true or false y en caso de true, el object[1] es un array de
	//int con las fichas a borrar que se usaria luego para llamar al metodo de borrar.
	//Incluso podria tener un array[2] con la cantidad de fichas, esto con la idea de ver 
	//cuanto borro con cada moviemiento.
	//Tambien hay que agregar una variable bool que dice si en la ultima corrida borro, asi 
	//se pueden hacer los combos sin problema.
	void checkToDelete(){
		goAgain:
		for (int i = 0; i < Main.height; i++) {
			for (int j = 0; j < w; j++) {
				if (checkFiveH (j, i)) {
					i = 0;
					j = 0;
					goto goAgain;
				} else if (checkFiveV (j, i)) {
					i = 0;
					j = 0;
					goto goAgain;
				} else if (checkFourV (j, i)) {
					i = 0;
					j = 0;
					goto goAgain;
				} else if ( checkFourH (j, i)) {
					i = 0;
					j = 0;
					goto goAgain;
				} else if (checkThreeH (j, i)) {
					i = 0;
					j = 0;
					goto goAgain;
				} else if (checkThreeV (j, i)) {
					i = 0;
					j = 0;
					goto goAgain;
				}
			}
		}
	}

	int checkFiveH(int posx, int posy){
		if (posx + 4 < 6 ) {
			if (grid[posx, posy] != null && grid[posx+1, posy] != null && grid[posx+2, posy] != null && 
				grid[posx+3, posy] != null && grid[posx+3, posy] != null) {
				if (grid [posx, posy].tag == grid [posx + 1, posy].tag &&
					grid[posx, posy].tag == grid [posx +2, posy].tag &&
					grid[posx +2, posy].tag == grid[posx +3, posy].tag &&
					grid[posx +3, posy].tag == grid[posx +4, posy].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					int[] array = { posx, posy, posx + 1, posy, posx + 2, posy , posx+3, posy, posx+4, posy};
					deleteFicha (array);
					return 5;
				}
			}
		}
		return 0;
	}

	int checkFourH(int posx, int posy){
		if (posx + 3 < 6 ) {
			if (grid[posx, posy] != null && grid[posx+1, posy] != null && grid[posx+2, posy] != null && grid[posx+3, posy] != null) {
				if (grid [posx, posy].tag == grid [posx + 1, posy].tag &&
					grid[posx, posy].tag == grid [posx +2, posy].tag &&
					grid[posx +2, posy].tag == grid[posx +3, posy].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					int[] array = { posx, posy, posx + 1, posy, posx + 2, posy , posx+3, posy};
					deleteFicha (array);
					return 4;
				}
			}
		}
		return 0;
	}

	int checkThreeH(int posx, int posy){
		if (posx + 2 < 6 ) {
			if (grid[posx, posy] != null && grid[posx+1, posy] != null && grid[posx+2, posy] != null) {
			if (grid [posx, posy].tag == grid [posx + 1, posy].tag &&
				grid[posx, posy].tag == grid [posx +2, posy].tag) {
				print ("Tengo que borrar aca!" + posx + posy);
				int[] array = { posx, posy, posx + 1, posy, posx + 2, posy };
				deleteFicha (array);
				return 3;
				}
			}
		}
		return 0;
	}

	int checkFiveV(int posx, int posy){
		if (true){//posy + 4 < 8 ) {
			if (grid[posx, posy] != null && grid[posx, posy+1] != null && grid[posx, posy+2] != null && 
				grid[posx, posy+3] != null && grid[posx, posy+4] != null) {
				if (grid [posx, posy].tag == grid [posx , posy+ 1].tag &&
					grid[posx, posy].tag == grid [posx , posy+2].tag &&
					grid[posx, posy].tag == grid [posx , posy+3].tag &&
					grid[posx, posy].tag == grid [posx , posy+4].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					int[] array = { posx, posy, posx , posy+ 1, posx , posy + 2, posx , posy + 3, posx , posy + 4};
					deleteFicha (array);
					return 55;
				}
			}
		}
		return 0;
	}

	int checkFourV(int posx, int posy){
		if (posy + 3 < 8 ) {
			if (grid[posx, posy] != null && grid[posx, posy+1] != null && grid[posx, posy+2] != null && 
				grid[posx, posy+3] != null) {
				if (grid [posx, posy].tag == grid [posx , posy+ 1].tag &&
					grid[posx, posy].tag == grid [posx , posy+2].tag &&
					grid[posx, posy].tag == grid [posx , posy+3].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					int[] array = { posx, posy, posx , posy+ 1, posx , posy + 2, posx , posy + 3};
					deleteFicha (array);
					return 44;
				}
			}
		}
		return 0;
	}

	int checkThreeV(int posx, int posy){
		if (posy + 2 < 8 ) {
			if (grid[posx, posy] != null && grid[posx, posy+1] != null && grid[posx, posy+2] != null) {
				if (grid [posx, posy].tag == grid [posx , posy+ 1].tag &&
					grid[posx, posy].tag == grid [posx , posy+2].tag) {
					print ("Tengo que borrar aca!" + posx + posy);
					int[] array = { posx, posy, posx , posy+ 1, posx , posy + 2};
					deleteFicha (array);
					return 33;
				}
			}
		}
		return 0;
	}
	*/


}
