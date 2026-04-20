using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
   [SerializeField, Tooltip("Начальная точка")]
   private Vector3 _start;

   [SerializeField, Tooltip("Конечная точка")]
   private Vector3 _end;

   [SerializeField, Tooltip("Скорость передвижения")]
   private float _speed = 3f;

   [SerializeField, Tooltip("Пауза в точках")]
   private float _delay = 1f;

   private Rigidbody _body;

   private WaitForFixedUpdate _waitForFixedUpdate;

   private Vector3 _worldStart;

   private Vector3 _worldEnd;

   private void Awake()
   {
      _body = GetComponent<Rigidbody>();
      _body.isKinematic = true;

      _waitForFixedUpdate = new WaitForFixedUpdate();
   }

   private void OnValidate()
   {
      if (_speed < 0f)
      {
         _speed = 0f;
      }
      if (_delay < 0f)
      {
         _delay = 0f;
      }
   }

   private IEnumerator Start()
   {
      _worldStart = transform.position + transform.TransformDirection(_start);
      _worldEnd = transform.position + transform.TransformDirection(_end);

      while (true)
      {
         yield return MoveToPoint(_worldEnd);
         yield return new WaitForSeconds(_delay);

         yield return MoveToPoint(_worldStart);
         yield return new WaitForSeconds(_delay);
      }
   }

   private IEnumerator MoveToPoint(Vector3 target)
   {
      while (true)
      {
         float step = _speed * Time.fixedDeltaTime;
         Vector3 nextPosition = Vector3.MoveTowards(_body.position, target, step);

         _body.MovePosition(nextPosition);

         if (Vector3.Distance(nextPosition, target) <= 0.001f)
         {
            _body.MovePosition(target);
            yield break;
         }

         yield return _waitForFixedUpdate;
      }
   }

   private void OnDrawGizmos()
   {
      if (Application.isPlaying)
      {
         return;
      }

      Vector3 worldStart = transform.position + transform.TransformDirection(_start);
      Vector3 worldEnd = transform.position + transform.TransformDirection(_end);

      Gizmos.color = Color.green;

      Gizmos.DrawLine(worldStart, worldEnd);
      Gizmos.DrawSphere(worldStart, 0.25f);
      Gizmos.DrawSphere(worldEnd, 0.25f);
   }
}
