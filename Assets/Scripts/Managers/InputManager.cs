using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class InputManager : MonoBehaviour
{
   private SceneController _sceneController;    // injected

   private Controls.GameActions _controls;      // inject

   private Coroutine _restartCoroutine;

   [SerializeField]
   private GameObject _restartUI;

   [SerializeField]
   private Image _restartFill;

   [SerializeField, Range(0.1f, 1.0f)]
   private float _restartPushInSec = 0.25f;

   private void Start()
   {
      _controls.Restart.performed += OnRestartPerfomed;
      _controls.Restart.canceled += OnRestartCanceled;

      _restartFill.fillAmount = 0f;
      _restartUI.SetActive(false);
   }

   private void OnDestroy()
   {
      _controls.Restart.performed -= OnRestartPerfomed;
      _controls.Restart.canceled -= OnRestartCanceled;
   }

   private void OnRestartPerfomed(InputAction.CallbackContext obj)
   {
      _restartUI.SetActive(true);

      if (_restartCoroutine != null)
      {
         StopCoroutine(_restartCoroutine);
      }

      _restartCoroutine = StartCoroutine(Restarter());
   }

   private void OnRestartCanceled(InputAction.CallbackContext obj)
   {
      if (_restartCoroutine != null)
      {
         StopCoroutine(_restartCoroutine);
         _restartCoroutine = null;
      }

      if (_restartFill.fillAmount >= 1f)
      {
         _sceneController.OpenMainScene();
         return;
      }

      _restartFill.fillAmount = 0f;
      _restartUI.SetActive(false);
   }

   private IEnumerator Restarter()
   {
      _restartFill.fillAmount = 0f;

      while (_restartFill.fillAmount < 1f)
      {
         _restartFill.fillAmount += Time.deltaTime / _restartPushInSec;
         yield return null;
      }

      _restartFill.fillAmount = 1f;

      _restartCoroutine = null;
   }

   [Inject]
   private void Construct(SceneController sceneController, Controls.GameActions controls)
   {
      _sceneController = sceneController;
      _controls = controls;
   }
}
