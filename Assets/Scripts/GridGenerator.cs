using UnityEngine;

public class GridGenerator : MonoBehaviour {

    // ref ==> reference
    // pref ==> prefab
    // num ==> number
    // vert ==> vertical
    // hor ==> horizontal
    // tex ==> texture
    // coord ==> coordinate
    // calc ==> calculate

    [Header("Game Objects")]
    [SerializeField] private GameObject _refSpriteHolders;
    [SerializeField] private GameObject _placeHolders;
    [SerializeField] private GameObject _gridPref;
    

    [Header("Grid Information")]
    [SerializeField] private float _numOfVertGrids, _numOfHorGrids;

    [Header("How many coordinate units are between horizontal grids?")]
    [SerializeField] private float _horSpaceUnit;

    [Header("How many coordinate units are between vertical grids?")]
    [SerializeField] private float _vertSpaceUnit;

    // ---------------------------------------------

    [Header("How many coordinate units is the upper frame thickness?")]
    [SerializeField] private float _upperFrameThickness;

    [Header("How many coordinate units is the lower frame thickness?")]
    [SerializeField] private float _lowerFrameThickness;

    [Header("How many coordinate units is the left frame thickness?")]
    [SerializeField] private float _leftFrameThickness;

    [Header("How many coordinate units is the right frame thickness?")]
    [SerializeField] private float _rightFrameThickness;

    private float _canvasWidth, _canvasHeight, _canvasPixelsPerUnit, _canvasScaleX, _canvasScaleY;
    private Vector3 _canvasCoord;

    private void Start() {
        
        Sprite canvasSprite = _refSpriteHolders.GetComponent<SpriteRenderer>().sprite;
        Texture canvasTex = canvasSprite.texture;
        Vector3 canvasScale = _refSpriteHolders.transform.localScale;

        _canvasCoord = _refSpriteHolders.transform.position;
        _canvasWidth = canvasTex.width;
        _canvasHeight = canvasTex.height;
        _canvasPixelsPerUnit = canvasSprite.pixelsPerUnit;

        _canvasScaleX = canvasScale.x;
        if(_canvasScaleX == 0) _canvasScaleX = 1;

        _canvasScaleY = canvasScale.y;
        if(_canvasScaleY == 0) _canvasScaleY = 1;

        if(_numOfVertGrids < 0) _numOfVertGrids = Mathf.Abs(_numOfVertGrids);
        if(_numOfHorGrids < 0) _numOfHorGrids = Mathf.Abs(_numOfHorGrids);

        if(_upperFrameThickness < 0 || _upperFrameThickness * _canvasPixelsPerUnit >= _canvasHeight) _upperFrameThickness = 0;
        if(_lowerFrameThickness < 0 || _lowerFrameThickness * _canvasPixelsPerUnit >= _canvasHeight) _lowerFrameThickness = 0;
        if((_upperFrameThickness + _lowerFrameThickness) * _canvasPixelsPerUnit >= _canvasHeight) {
            _upperFrameThickness = 0;
            _lowerFrameThickness = 0;
        }

        if(_leftFrameThickness < 0 || _leftFrameThickness * _canvasPixelsPerUnit >= _canvasWidth) _leftFrameThickness = 0;
        if(_rightFrameThickness < 0 || _rightFrameThickness * _canvasPixelsPerUnit >= _canvasWidth) _rightFrameThickness = 0;
        if((_leftFrameThickness + _rightFrameThickness) * _canvasPixelsPerUnit >= _canvasWidth) {
            _leftFrameThickness = 0;
            _rightFrameThickness = 0;
        }

        CreateGrid();

    }

    private void CreateGrid() {

        Vector3 centerOfGrid = new (_canvasCoord.x - ((_rightFrameThickness - _leftFrameThickness) / 2f), _canvasCoord.y - ((_upperFrameThickness - _lowerFrameThickness) / 2f), 0);

        float widthOfGridTable = _canvasWidth * Mathf.Abs(_canvasScaleX) - (_leftFrameThickness + _rightFrameThickness) * _canvasPixelsPerUnit;
        float heightOfGridTable = _canvasHeight * Mathf.Abs(_canvasScaleY) - (_upperFrameThickness + _lowerFrameThickness) * _canvasPixelsPerUnit;

        if(_horSpaceUnit < 0 || _horSpaceUnit * _canvasPixelsPerUnit * (_numOfHorGrids - 1f) >= widthOfGridTable) _horSpaceUnit = 0;
        if(_vertSpaceUnit < 0 || _vertSpaceUnit * _canvasPixelsPerUnit * (_numOfVertGrids - 1f) >= heightOfGridTable) _vertSpaceUnit = 0;

        float consPosX = (widthOfGridTable / _canvasPixelsPerUnit - (_numOfHorGrids - 1f) * _horSpaceUnit) / 2f;
        float consPosY = (heightOfGridTable / _canvasPixelsPerUnit - (_numOfVertGrids - 1f) * _vertSpaceUnit) / 2f;

        float consGridScX = ((widthOfGridTable / _canvasPixelsPerUnit) - (_numOfHorGrids - 1f) * _horSpaceUnit) / _numOfHorGrids;
        float consGridScY = ((heightOfGridTable / _canvasPixelsPerUnit) - (_numOfVertGrids - 1f) * _vertSpaceUnit) / _numOfVertGrids;

        for (int i = 0; i < _numOfVertGrids; i++)
        {
            float posY;
            posY = centerOfGrid.y + consPosY * ((_numOfVertGrids - 1f - i * 2f) / _numOfVertGrids) + ((_numOfVertGrids - 1f) / 2f - i) * _vertSpaceUnit;

            for (int j = 0; j < _numOfHorGrids; j++)
            {
                float posX;
                posX = centerOfGrid.x - consPosX * ((_numOfHorGrids - 1f - j * 2f) / _numOfHorGrids) - ((_numOfHorGrids - 1f) / 2f - j) * _horSpaceUnit;

                GameObject grid = Instantiate(_gridPref) as GameObject;
                grid.gameObject.transform.localScale = new Vector2(consGridScX, consGridScY);
                grid.name = "index" + (i * _numOfVertGrids + j);
                grid.transform.position = new Vector3(posX, posY, 0);
                grid.transform.parent = _placeHolders.transform;
            }
        }
    }
    
}