using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
   [SerializeField, Tooltip("Поворот")]
   private Vector3 _rotate;

   private Rigidbody _body;

   private WaitForFixedUpdate _waitForFixedUpdate;

   private void Awake()
   {
      _body = GetComponent<Rigidbody>();

      _body.isKinematic = true;

      _waitForFixedUpdate = new WaitForFixedUpdate();
   }

   // Start is called before the first frame update
   private IEnumerator Start()
   {
      yield return _waitForFixedUpdate;

      while (true)
      {
         Vector3 angle = _rotate * Time.fixedDeltaTime;

         _body.MoveRotation(_body.rotation * Quaternion.Euler(angle));

         yield return _waitForFixedUpdate;
      }
   }
}
