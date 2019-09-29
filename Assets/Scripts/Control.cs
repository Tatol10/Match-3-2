using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private Model model;
    // Start is called before the first frame update
    void Start()
    {
        model = FindObjectOfType<Model>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DestroyMatchesAt(int row, int column)
    {
        if (model.allDots[row, column].GetComponent<View>().match)
        {
            Destroy(model.allDots[row, column]);
            model.allDots[row, column] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < model.width; i++)
        {
            for (int j = 0; j < model.hight; j++)
            {
                if (model.allDots[i, j] != null)
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
        for (int i = 0; i < model.width; i++)
        {
            for (int j = 0; j < model.hight; j++)
            {
                if (model.allDots[i, j] == null)
                {
                    nullcount++;
                }
                else if (nullcount > 0)
                {
                    model.allDots[i, j].GetComponent<View>().column -= nullcount;
                    model.allDots[i, j] = null;
                }
            }
            nullcount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < model.width; i++)
        {
            for (int j = 0; j < model.hight; j++)
            {
                if (model.allDots[i, j] == null)
                {
                    Vector2 temPos = new Vector2(i, j);
                    int dotToUse = Random.Range(0, model.dots.Length);
                    GameObject piece = Instantiate(model.dots[dotToUse], temPos, Quaternion.identity);
                    model.allDots[i, j] = piece;
                }
            }
        }
    }
    private bool MatchOnBoard()
    {
        for (int i = 0; i < model.width; i++)
        {
            for (int j = 0; i < model.hight; i++)
            {
                if (model.allDots[i, j] != null)
                {
                    if (model.allDots[i, j].GetComponent<View>().match)
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
