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

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool MatchesAt(int row, int column, GameObject piece)//chequear que no empiece el juego con 3 iguales segidos
    {
        if (row > 1 && column > 1)
        {
            if(allDots[row - 1, column].tag == piece.tag && allDots[row - 2, column].tag == piece.tag)
            {
                return true;
            }
            if (allDots[row , column - 1].tag == piece.tag && allDots[row , column - 2].tag == piece.tag)
            {
                return true;
            }
        } else if (row <= 1 || column <= 1)
        {
            if(row > 1)
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

    private void DestroyMatchesAt(int row, int column)
    {
        if(allDots[row , column].GetComponent<Dot>().match)
        {
            Destroy(allDots[row, column]);
            allDots[row, column] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreseColumnCo());
    }
    private IEnumerator DecreseColumnCo()//hace caer lo de arriba a los espacios vacios
    {
        int nullcount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullcount ++;
                }else if (nullcount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().column -= nullcount;
                    allDots[i, j] = null;
                }
            }
            nullcount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 temPos = new Vector2(i, j);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], temPos, Quaternion.identity);
                    allDots[i, j] = piece;
                }
            }
        }
    }
    private bool MatchOnBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; i < hight; i++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().match)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);
        while (MatchOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }
}
