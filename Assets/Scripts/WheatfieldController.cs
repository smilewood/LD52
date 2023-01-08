using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WheatfieldController : MonoBehaviour
{
   private Dictionary<Vector2Int, WheatPlot> plots;
   public Vector2Int Height, Width;
   public GameObject WheatPlotPrefab;
   [Range(0, 1)]
   public float SpawnOdds;

   // Start is called before the first frame update
   void Start()
   {
      plots = new Dictionary<Vector2Int, WheatPlot>();
      foreach (int x in Enumerable.Range(Width.x, (Width.y - Width.x) + 1))
      {
         foreach (int y in Enumerable.Range(Height.x, (Height.y - Height.x) + 1))
         {
            if (Random.Range(0f, 1f) < SpawnOdds)
            {
               GameObject plot = Instantiate(WheatPlotPrefab, new Vector3(x, y, 0), Quaternion.identity, this.transform);
               plot.name = $"WheatPlot[{x},{y}]";
               plots[new Vector2Int(x, y)] = plot.GetComponent<WheatPlot>();
            }
         }
      }
   }

   // Update is called once per frame
   void Update()
   {

   }

   public bool IsWheatAtLocation(Vector2Int location)
   {
      //If there was never wheat or if it was harvested return false
      return plots.TryGetValue(location, out WheatPlot plot) && plot.Grown;
   }

   public WheatPlot GetPlotAtLocation(Vector2Int location)
   {
      return plots.GetValueOrDefault(location);
   }
}
