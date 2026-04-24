using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
   [SerializeField]
   private float _moveSpeed = 4f;

   public Cell Cell { get; set; }

   public event Action<Unit> OnMoveEndCallback;

   private Coroutine _moveCoroutine;

   public void OnPointerClick(PointerEventData eventData)
   {
      if (Cell != null)
      {
         Cell.OnPointerClick(eventData);
      }
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      if (Cell != null)
      {
         Cell.OnPointerEnter(eventData);
      }
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      if (Cell != null)
      {
         Cell.OnPointerExit(eventData);
      }
   }

   public void Move(Cell targetCell)
   {
      if (targetCell == null)
      {
         return;
      }

      if (_moveCoroutine != null)
      {
         StopCoroutine(_moveCoroutine);
      }

      _moveCoroutine = StartCoroutine(MoveCoroutine(targetCell));
   }

   private IEnumerator MoveCoroutine(Cell targetCell)
   {
      Vector3 targetPosition = targetCell.transform.position;
      targetPosition.y = transform.position.y;

      while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
      {
         transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            _moveSpeed * Time.deltaTime);

         yield return null;
      }

      transform.position = targetPosition;

      if (Cell != null)
      {
         Cell.Unit = null;
      }

      Cell = targetCell;
      Cell.Unit = this;

      _moveCoroutine = null;

      OnMoveEndCallback?.Invoke(this);
   }
}
