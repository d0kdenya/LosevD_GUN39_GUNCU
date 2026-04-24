using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
   private Controls _controls;

   [SerializeField]
   private CellManager _cellManager;

   [SerializeField]
   private SceneController _controller;

   [SerializeField, Space(15f)]
   private CellPaletteSettings _cellPaletteSettings;

   public override void InstallBindings()
   {
      _controls = new Controls();
      _controls.Game.Enable();
      
      Container.BindInstance(_controls.Game).AsSingle();

      Container.BindInstance(_cellManager).AsSingle();
      Container.BindInstance(_controller).AsSingle();

      Container.BindInstance(_cellPaletteSettings).AsSingle();

      _cellManager.OnCellClicked += CellManagerOnCellClicked;
   }

   public void CellManagerOnCellClicked(Cell obj)
   {
      obj.SetSelect(_cellPaletteSettings.SelectCell);
   }

   private void OnDestroy()
   {
      if (_cellManager != null)
      {
         _cellManager.OnCellClicked -= CellManagerOnCellClicked;
      }

      if (_controls != null)
      {
         _controls.Game.Disable();
         _controls.Dispose();
      }
   }
}
