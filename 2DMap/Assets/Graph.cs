using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Utilities;
using UnityEngine;

public class Graph : MonoBehaviour {

	public float graphWidth;
 public float graphHeight;
 LineRenderer newLineRenderer;
 List<int> decibels;
 int vertexAmount = 50;
 float xInterval;
 
     GameObject parentCanvas;
 
     // Use this for initialization
     void Start ()
     {/*
         parentCanvas = GameObject.Find("MarkerInfoDisplay");
         newLineRenderer = parentCanvas.AddComponent<LineRenderer>();
         
         graphWidth = newLineRenderer.GetComponent<RectTransform>().rect.width;
         graphHeight = newLineRenderer.GetComponent<RectTransform>().rect.height;
         Debug.Log(graphHeight);
         Debug.Log(graphWidth);
         //newLineRenderer = line.GetComponentInChildren<LineRenderer>();
         newLineRenderer.positionCount = vertexAmount;
 
         xInterval = graphWidth / vertexAmount;
         */
     }

    private void Update()
    {
       /* int listSize = 10000;
        decibels = new List<int>(listSize);
        for (int i = 0; i < listSize; i+=100)
        {
            decibels.Add(i);
            Debug.Log(i);
        }
        Draw(decibels);*/
    }

        //Display 1 minute of data or as much as there is.
        public void Draw(List<int> decibels)
     {
        /* Debug.Log("In draw");
         if (decibels.Count == 0)
             return;
 
         float x = 0;
 
         for (int i = 0; i < vertexAmount && i < decibels.Count; i++)
         {
             Debug.Log("In draw for loop");
             int _index = decibels.Count - i - 1;
 
             float y = decibels[_index] * (graphHeight/130); //(Divide grapheight with the maximum value of decibels.
             x = i * xInterval;
 
             newLineRenderer.SetPosition(i, new Vector3(x - graphWidth / 2 , y - graphHeight / 2 , -5));
        }
        */
     }
}
