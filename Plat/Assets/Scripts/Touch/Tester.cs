using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{

    public TouchCustom.TYPE testerType;
    SpriteRenderer spriteRenderer;
    int i=0;

    public void Touch()
    {
        if (testerType == TouchCustom.TYPE.SAMPLE)
        {
            i++;
            switch (i)
            {
                case 0:
                    spriteRenderer.color = Color.white;
                    break;
                case 1:
                    spriteRenderer.color = Color.blue;
                    break;
                case 2:
                    spriteRenderer.color = Color.yellow;
                    break;
                case 3:
                    spriteRenderer.color = Color.red;
                    break;
                case 4:
                    spriteRenderer.color = Color.black;
                    i = -1;
                    break;
                default:
                    break;
            }

        }
    }

    private void Start()
    {
        if (testerType == TouchCustom.TYPE.SLIDE)
        {
            TouchController.slideDelegate += Slide;
            TouchController.pinchDelegate += Pinche;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Slide(Vector2 vector2)
    {
        Vector3 position = transform.position;
        position.x += vector2.x*Time.deltaTime;
        position.y += vector2.y * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, -2, 2);
        position.y = Mathf.Clamp(position.y, -2, 2);
        transform.position = position;
    }

    private void Pinche(float value)
    {
        transform.localScale += Vector3.one*value;
    }

    private void AddToDelegate()
    {
        TouchController.dragDelegate = Drag;
    }

    private void Drag(Vector3 position)
    {
        Debug.Log(position);
        transform.position = position;
    }
}
