using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;//para poner el prefab
    public GameObject[] dots;
    public GameObject[,] allDots;
    public int width;
    public int hight;
    private BackgroundTile[,] allTiles;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width, hight];// iguala al arreglo 2D al ancho y alto
        allDots = new GameObject[width, hight];
        SetUp();
    }

    private void SetUp() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < hight; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                GameObject backTile = Instantiate(tilePrefab, tempPos, Quaternion.identity) as GameObject;//instancia el board
                backTile.transform.parent = this.transform;//cambia la gerarquia para que sea mas ordenado
                backTile.name = "( "+i+" , "+j+" )";//cambia el nombre en el inspector
                int dotUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotUse], tempPos, Quaternion.identity);//instancia el board
                dot.transform.parent = this.transform;//cambia la gerarquia para que sea mas ordenado
                dot.name = "( " + i + " , " + j + " )";//cambia el nombre en el inspector
                allDots[i, j] = dot;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
