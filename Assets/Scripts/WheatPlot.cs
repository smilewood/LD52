using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatPlot : MonoBehaviour
{
   public bool Grown = true;
   public GameObject GraphicsGrown, GraphicsCut;

   // Start is called before the first frame update
   void Start()
   {
      GraphicsGrown.SetActive(true);
      GraphicsCut.SetActive(false);
   }

   // Update is called once per frame
   void Update()
   {

   }

   public void Harvest()
   {
      Grown = false;
      GraphicsGrown.SetActive(false);
      GraphicsCut.SetActive(true);
   }
}
