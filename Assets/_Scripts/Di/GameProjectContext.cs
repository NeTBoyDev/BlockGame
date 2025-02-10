using System.Collections.Generic;
using _Di;
using _Scripts.Configuration;
using _Scripts.Systems.Game;
using _Scripts.Abstractions;
using _Scripts.Abstractions.Interfaces;
using _Scripts.Factory;
using _Scripts.Services;
using _Scripts.Systems;
using UnityEngine;
using DI;
using UniRx;
using ILogger = _Scripts.Abstractions.Interfaces.ILogger;
using Notification = _Scripts.Ui.Notification;

namespace _Scripts.Di
{
    public class GameProjectContext : ProjectContext
    {
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private BoxCollider2D _groundCollider;
        [SerializeField] private BoxCollider2D _trashCollider;
        [SerializeField] private CanvasGroup _canvasGroup;
        private CompositeDisposable _disposable = new();
        public override void RegisterDependencies()
        {
            RegisterFromInstance(_gameConfiguration.ScrollMaterials);
            RegisterFromInstance(_gameConfiguration.Blocks);
            RegisterFromInstance(_disposable);
            RegisterFromInstance(_trashCollider);
            RegisterFromInstance(_groundCollider).WithTag("GroundCollider");
            RegisterFromInstance(_canvasGroup);

            Register<BlockFactory>(false);
            Register<BlockButtonFactory>(false);

            Register<IInputHandler, TouchSystem>(false);
            Register<ILogger, LoggerSystem>(false);
            Register<IBoundsChecker, BoundsCheckerService>(false);
            Register<IBlockTrashService, BlockTrashService>(false);
            Register<IBlockPlacementService, BlockPlacementService>(false);
            Register<ISaveService<List<BlockPresenterBase>,BlockModelBase>, SaveProgressService>(false);

            Register<GameModel>(true);
            Register<TouchSystem>(true);
            Register<BackgroundScrollSystem>(false);
            Register<Notification>(false);
            
            RegisterFromScene<GameView>();
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
