﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public int column;
    public int row;
    public int previousC;
    public int previousR;
    public int targetX;
    public int targetY;
    public float swipeA = 0;// angulo del swipe
    public float swipeResist = 1f;//por lo menos tener una unidad de distancia para mover, es para resolver el bug de clikear y que las cosas se mueban
    public bool match = false;//para cheaquear si hay match
    private Model model;// referencia a el tablero
    private Control control;
    private GameObject otherDot;//referencia a el otro dot
    private Vector2 firstTouch;//condenadas de donde empieza a tocar 
    private Vector2 lastTouch;//condenadas de donde termina de tocar
    private Vector2 tempPosition;
    // Start is called before the first frame update
    void Start()
    {
        model = FindObjectOfType<Model>();//Iguala el board de este script a un objeto de clase board que encuentre 
        //esto funciona porque solo va a haber un board
        control = FindObjectOfType<Control>();
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
            mySprite.color = new Color(1f, 1f, 1f, .2f);// cambia el color de sprite 
        }
        targetX = row;
        targetY = column;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //moverse al objetivo
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (model.allDots[row, column] != this.gameObject)
            {
                model.allDots[row, column] = this.gameObject;
            }
        }
        else
        {
            //setear la posicion directamente
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //moverse al objetivo
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (model.allDots[row, column] != this.gameObject)
            {
                model.allDots[row, column] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }
    public IEnumerator CheckMoveCo()//corutina funciona a la par del update pero cada tiempo determinado
    {
        yield return new WaitForSeconds(.5f);//esperar por 0.5 seg
        if (otherDot != null)
        {
            if (!match && !otherDot.GetComponent<View>().match)
            {
                otherDot.GetComponent<View>().row = row;
                otherDot.GetComponent<View>().column = column;
                row = previousR;
                column = previousC;
            }
            else
            {
                control.DestroyMatches();//si no vuelven es porque hay match entonces destuirlo
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

    void TanAngulo()
    {
        if (Mathf.Abs(lastTouch.x - firstTouch.x) > swipeResist || Mathf.Abs(lastTouch.y - firstTouch.y) > swipeResist)
        {
            swipeA = Mathf.Atan2(lastTouch.y - firstTouch.y, lastTouch.x - firstTouch.x) * 180 / Mathf.PI;//Tangente a la menos 1 del angulo
            //Debug.Log(swipeA);
            DirAngulo();
        }
    }

    void DirAngulo()
    {
        if (swipeA > -45 && swipeA <= 45 && row < model.width - 1)
        {
            //derecha
            otherDot = model.allDots[row + 1, column];
            otherDot.GetComponent<View>().row -= 1;
            row += 1;
        }
        else if (swipeA > 45 && swipeA <= 135 && row < model.hight - 1)
        {
            //arriba
            otherDot = model.allDots[row, column + 1];
            otherDot.GetComponent<View>().column -= 1;
            column += 1;
        }
        else if ((swipeA > 135 || swipeA <= -135) && row > 0)
        {
            //izquierda
            otherDot = model.allDots[row - 1, column];
            otherDot.GetComponent<View>().row += 1;
            row -= 1;
        }
        else if (swipeA > -45 && swipeA <= -135 && column > 0)
        {
            //abajo
            otherDot = model.allDots[row, column - 1];
            otherDot.GetComponent<View>().column += 1;
            column -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }
    void FindMatch()
    {
        if (row > 0 && row < model.width - 1)//no puede haber match si la columna es mas chico que 0 o mas grande que el board
        {
            GameObject leftDot = model.allDots[row - 1, column];
            GameObject rightDot = model.allDots[row + 1, column];
            if (leftDot != null && rightDot != null)
            {
                if (leftDot.tag == this.gameObject.tag && rightDot.tag == this.gameObject.tag)
                {
                    leftDot.GetComponent<View>().match = true;
                    rightDot.GetComponent<View>().match = true;
                    match = true;
                }
            }
        }
        if (column > 0 && column < model.hight - 1)//no puede haber match si la columna es mas chico que 0 o mas grande que el board
        {
            GameObject downDot = model.allDots[row, column - 1];
            GameObject upDot = model.allDots[row, column + 1];
            if (downDot != null && upDot != null)
            {
                if (downDot.tag == this.gameObject.tag && upDot.tag == this.gameObject.tag)
                {
                    downDot.GetComponent<View>().match = true;
                    upDot.GetComponent<View>().match = true;
                    match = true;
                }
            }
        }

    }
}
