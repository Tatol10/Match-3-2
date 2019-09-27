using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public int previousC;
    public int previousR;
    public int targetX;
    public int targetY;
    public float swipeA = 0;// angulo del swipe
    public bool match = false;//para cheaquear si hay match
    private Board board;// referencia a el tablero
    private GameObject otherDot;//referencia a el otro dot
    private Vector2 firstTouch;//condenadas de donde empieza a tocar 
    private Vector2 lastTouch;//condenadas de donde termina de tocar
    private Vector2 tempPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();//Iguala el board de este script a un objeto de clase board que encuentre 
        //esto funciona porque solo va a haber un board
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetX;
        column = targetY;
        previousR = row;
        previousC = column;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatch();
        if (match)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f,1f,1f,.2f);// cambia el color de sprite 
        }
        targetX = row;
        targetY = column;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //moverse al objetivo
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //setear la posicion directamente
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[row, column] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //moverse al objetivo
            tempPosition = new Vector2(transform.position.x ,targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[row, column] = this.gameObject;
        }
    }

    public IEnumerator CheckMoveCo()//corutina funciona a la par del update pero cada tiempo determinado
    {
        yield return new WaitForSeconds(.5f);//esperar por 0.5 seg
        if(otherDot != null)
        {
            if(!match && !otherDot.GetComponent<Dot>().match)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousR;
                column = previousC;
            }
            otherDot = null;
        }
    }

    private void OnMouseDown()
    {
        //Camara to wordpoint sirve para cambiar de posicion de pixeles a coodenadas del mundo
        firstTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);//cuando se apreta guarda las cordenadas
        //Debug.Log(firstTouch);
    }

    private void OnMouseUp()
    {
        //Camara to wordpoint sirve para cambiar de posicion de pixeles a coodenadas del mundo
        lastTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);//cuando se apreta guarda las cordenadas
        //Debug.Log(lastTouch);
        TanAngulo();
    }

    void TanAngulo() {
        swipeA = Mathf.Atan2(lastTouch.y - firstTouch.y, lastTouch.x - firstTouch.x) * 180 / Mathf.PI;//Tangente a la menos 1 del angulo
        //Debug.Log(swipeA);
        DirAngulo();
    }

    void DirAngulo()
    {
        if (swipeA > -45 && swipeA <= 45 && row < board.width-1)
        {
            //derecha
            otherDot = board.allDots[row + 1, column];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if (swipeA > 45 && swipeA <= 135 && row < board.hight-1)
        {
            //arriba
            otherDot = board.allDots[row, column + 1];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if ((swipeA > 135 || swipeA <= -135) && row > 0)
        {
            //izquierda
            otherDot = board.allDots[row -1, column];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        else if (swipeA > -45 && swipeA <= -135 && column >0)
        {
            //abajo
            otherDot = board.allDots[row, column -1];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }
    void FindMatch()
    {
        if (row > 0 && row < board.width - 1)//no puede haber match si la columna es mas chico que 0 o mas grande que el board
        {
            GameObject leftDot = board.allDots[row - 1, column];
            GameObject rightDot = board.allDots[row + 1, column];
            if (leftDot.tag == this.gameObject.tag && rightDot.tag == this.gameObject.tag)
            {
                leftDot.GetComponent<Dot>().match = true;
                rightDot.GetComponent<Dot>().match = true;
                match = true;
            }
        }
        if (column > 0 && column < board.hight - 1)//no puede haber match si la columna es mas chico que 0 o mas grande que el board
        {
            GameObject downDot = board.allDots[row , column - 1];
            GameObject upDot = board.allDots[row , column + 1];
            if (downDot.tag == this.gameObject.tag && upDot.tag == this.gameObject.tag)
            {
                downDot.GetComponent<Dot>().match = true;
                upDot.GetComponent<Dot>().match = true;
                match = true;
            }
        }

    }
}
