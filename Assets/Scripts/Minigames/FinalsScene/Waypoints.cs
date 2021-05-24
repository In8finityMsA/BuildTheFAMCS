using UnityEngine;

public class Waypoints : MonoBehaviour
{
   private static Transform[] _points;
   public static Transform[] _Points => _points;

   private void Awake()
   {
      _points = new Transform[transform.childCount];
      for (int i = 0; i < _points.Length; i++)
      {
         _points[i] = transform.GetChild(i);
      }
   }
}
