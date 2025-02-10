using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Abstractions;
using _Scripts.Abstractions.Interfaces;
using _Scripts.Services;
using _Scripts.Systems.Game;
using DI;
using UnityEngine;
using ILogger = _Scripts.Abstractions.Interfaces.ILogger;

public class GamePresenter : MonoBehaviour
{
    public static GamePresenter Instance { get; private set; }
    
    [Inject] private IBlockPlacementService _placementService;
    [Inject] private IBlockTrashService _trashService;
    [Inject] private ISaveService<List<BlockPresenterBase>,BlockModelBase> _saveService;
    [Inject] private IInputHandler _blockTouchSystem;
    [Inject] private ILogger _logger;
    [Inject] private GameModel _model;
    [Inject] private GameView _view;

    private BlockPresenterBase _currentInteractingBlock;
    private const string BLOCK_LAYER_NAME = "Block";

    private void Start()
    {
        _blockTouchSystem.OnPointerDown += TryGetBlock;
        _blockTouchSystem.OnPointerMove += MoveBlock;
        _blockTouchSystem.OnPointerUp += ReleaseBlock;
        
        _view.Initialize(_model.BlockModels);
        _blockTouchSystem.StartDetection();

        var loadedData = _saveService.Load(_model.BlockModels.First());

        if (loadedData != null && loadedData.Count > 0)
        {
            _model.AddRange(loadedData);
            _currentInteractingBlock = null;
            UpdateBlockListState();
            
            _logger.Log("block_loaded");
        }
    }

    public void PlaceBlock(BlockPresenterBase block)
    {
        if (!block)
            return;

        if (_trashService.MayTrash(block))
        {
            _trashService.Trash(block);
            _logger.Log("block_trashed");
            return;
        }
        
        if (!_placementService.CanPlace(block,_model.Tower))
        {
            block.DestroyBlock();
            return;
        }

        _placementService.Place(block, _model.Tower);
        _model.AddBlock(block);
        
        _logger.Log("block_placed");
        
        UpdateBlockListState();
    }

    public void SetBlock(BlockPresenterBase block)
    {
        if (!block)
            return;
        
        if(_currentInteractingBlock != null)
            _currentInteractingBlock.DestroyBlock();
        
        _currentInteractingBlock = block;
        _currentInteractingBlock?.StartDrag();
        
        _logger.Log("block_taken");
        
        if (!_model.Contains(block))
            return;
        
        var blocks = _model.GetUpperBlockRange(block);
        _model.RemoveUpperBlockRange(block);

        foreach (var blockItem in blocks)
        {
            PlaceBlock(blockItem);
        }
        
        UpdateBlockListState();
    }

    private void TryGetBlock(Vector3 pos)
    {
        var hit = Physics2D.Raycast(pos, Vector3.zero);
        if (hit.collider!= null && hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer(BLOCK_LAYER_NAME)) && hit.collider.TryGetComponent(out BlockPresenterBase block) && block != null)
        {
            SetBlock(block);
        }
    }

    private void MoveBlock(Vector3 pos)
    {
        _currentInteractingBlock?.HandleDrag(pos);
    }

    private void ReleaseBlock(Vector3 pos)
    {
        _currentInteractingBlock?.StopDrag();
        PlaceBlock(_currentInteractingBlock);
        _currentInteractingBlock = null;
        
        //_logger.Log("block_released");
    }
    
    private void UpdateBlockListState()
    {
        bool isTowerFull = _placementService.IsTowerFull(_model.Tower);
        if (isTowerFull)
        {
            _view.DisableBlockList();
        }
        else
        {
            _view.EnableBlockList();
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        Dictionary<string, string> localization = new()
        {
            { "block_placed", "Block has been placed!" },
            { "block_taken", "Block has been taken!" },
            { "block_released", "Block has been released!" },
            { "block_trashed", "Block has been trashed!" },
            { "block_loaded", "Blocks has been loaded!" }
        };
        
        _logger.LoadLocalization(localization);
    }

    private void OnApplicationQuit()
    {
        _saveService.Save(_model.Tower);
    }

    private void OnDestroy()
    {
        _blockTouchSystem.OnPointerDown -= TryGetBlock;
        _blockTouchSystem.OnPointerMove -= MoveBlock;
        _blockTouchSystem.OnPointerUp -= ReleaseBlock;
    
        if (Instance == this)
        {
            Instance = null;
        }
    }
}