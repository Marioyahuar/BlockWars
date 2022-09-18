using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _highlightedColor;
    [SerializeField] private SpriteRenderer _renderer;
    public int idSpot;
    public GameObject _selectedEffect;
    public GameObject actionSprite;
    private Color _auxColor;
    private Sprite _auxSprite, _hover;

    public void Init(Color color)
    {
        _baseColor = color;
        _renderer.color = color;
    }

    public void Init(Sprite sprite, Sprite hover)
    {
        //_baseColor = color;
        _renderer.sprite = sprite;
        _hover = hover;
    }

    void OnMouseEnter()
    {
        //_auxColor = _renderer.color;
        //_renderer.color = _highlightedColor;
        _auxSprite = _renderer.sprite;
        _renderer.sprite = _hover;
    }

    void OnMouseExit()
    {
        //_renderer.color = _auxColor;
        _renderer.sprite = _auxSprite;
    }

    void OnMouseDown()
    {
        if (!UIManager.Instance.blockPanelActive()) { Debug.Log("No se puede seleccionar"); return; }
        if(WorldRenderer.Instance.tilePicked)
        {
            WorldRenderer.Instance.tilePicked._selectedEffect.SetActive(false);
        }
        WorldManager.Instance.selectedTile = WorldManager.Instance.GetTileAtPosition(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        WorldManager.Instance.PickTile();
        WorldRenderer.Instance.tilePicked = this;
        WorldRenderer.Instance.tilePicked._selectedEffect.SetActive(true);

    }

   
}