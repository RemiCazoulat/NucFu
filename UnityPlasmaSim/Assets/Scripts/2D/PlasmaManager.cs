using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

public class PlasmaManager : MonoBehaviour
{
   public int gridSizeX;
   public float xyRatio;

   private Vector2[] plasmaSquares;
   private int gridSizeY;

   private void Start()
   {
      gridSizeY = (int)(gridSizeX * xyRatio);
      plasmaSquares = new Vector2[gridSizeX * gridSizeY];
   }

   public void BubbleSort(int[] array)
   {
      for (var i = 0; i < array.Length - 1; ++i)
        for (var j = 0; j < array.Length - 1; ++j)
          if (array[j] > array[j + 1])
            {
              var temp = array[j];
              array[j] = array[j + 1];
              array[j + 1] = temp;
            }
   }

   
   
}
