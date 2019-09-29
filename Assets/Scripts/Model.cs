using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                GameObject backTile = Instantiate(tilePrefab, tempPos, Quaternion.identity) as GameObject;//instancia el board
                backTile.transform.parent = this.transform;//cambia la gerarquia para que sea mas ordenado
                backTile.name = "( " + i + " , " + j + " )";//cambia el nombre en el inspector
                int dotUse = Random.Range(0, dots.Length);
                int contMax = 0;
                while (MatchesAt(i, j, dots[dotUse]) && contMax < 100)
                {
                    dotUse = Random.Range(0, dots.Length);
                    contMax++;
                }
                contMax = 0;
                GameObject dot = Instantiate(dots[dotUse], tempPos, Quaternion.identity);//instancia el board
                dot.transform.parent = this.transform;//cambia la gerarquia para que sea mas ordenado
                dot.name = "( " + i + " , " + j + " )";//cambia el nombre en el inspector
                allDots[i, j] = dot;
            }
        }
    }

    private bool MatchesAt(int row, int column, GameObject piece)//chequear que no empiece el juego con 3 iguales segidos
    {
        if (row > 1 && column > 1)
        {
            if (allDots[row - 1, column].tag == piece.tag && allDots[row - 2, column].tag == piece.tag)
            {
                return true;
            }
            if (allDots[row, column - 1].tag == piece.tag && allDots[row, column - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (row <= 1 || column <= 1)
        {
            if (row > 1)
            {
                if (allDots[row - 1, column].tag == piece.tag && allDots[row - 2, column].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[row, column - 1].tag == piece.tag && allDots[row, column - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
