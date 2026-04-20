using UnityEngine;

public class Gates : MonoBehaviour
{
   [SerializeField, Tooltip("Ñ÷¸ò")]
   private int _score = 0;

   private void OnTriggerEnter(Collider ball)
   {
      if (ball.GetComponent<Ball>() != null)
      {
         _score++;

         Debug.Log($"Current score: {_score}");

         Destroy(ball.gameObject);
      }
   }
}
