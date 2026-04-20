using UnityEngine;

public class Ball : MonoBehaviour
{   
   private Vector3 _scale;

   private void Awake()
   {
      _scale = transform.localScale;

      SetScale();
   }

   private void OnTransformParentChanged()
   {
      SetScale();
   }

   private void SetScale()
   {
      var parent = transform.parent;

      if (parent != null)
      {
         float scaleX = _scale.x / parent.lossyScale.x;
         float scaleY = _scale.y / parent.lossyScale.y;
         float scaleZ = _scale.z / parent.lossyScale.z;

         transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

         transform.localPosition = new Vector3(0, 0.6f, 0.75f);
      }
      else
      {
         transform.localScale = _scale;
      }
   }
}
