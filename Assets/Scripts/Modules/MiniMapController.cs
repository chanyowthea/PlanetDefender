using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public static MiniMapController Instance { private set; get; }

    public float _WorldWidth = 100;
    public float _WorldHeight = 100;
    [SerializeField] Transform _Floor;
    [SerializeField] RectTransform _Frame;
    RectTransform _RectTf;
    float _MinGap = 0.01f;
    // x:[-0.5f, 0.5f]

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _RectTf = this.transform as RectTransform;
        var bottomLeft = Camera.main.WorldToScreenPoint(new Vector3(-_WorldWidth / 2f, -_WorldHeight / 2f, 0));
        var topRight = Camera.main.WorldToScreenPoint(new Vector3(_WorldWidth / 2f, _WorldHeight / 2f, 0));
        Vector3 v3 = topRight - bottomLeft;
        // frame indicates the screen and map indicates the world map. 
        float wRatio = Screen.width / v3.x;
        float hRatio = Screen.height / v3.y;
        var map = (this.transform as RectTransform).rect.size;
        _Frame.sizeDelta = new Vector2(wRatio * map.x, hRatio * map.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null)
        {
            return;
        }

        if (!RectTransformUtility.RectangleContainsScreenPoint(_RectTf, eventData.position))
        {
            return;
        }

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.pressPosition, null, out pos))
        {
            pos = ResetOutRangePos(pos);
            var p = TransToWorldPoint(pos);
            p.z = Camera.main.transform.position.z;
            Camera.main.transform.position = p;
            _Frame.transform.localPosition = pos;
        }
    }

    public Vector3 TransToWorldPoint(Vector2 pos)
    {
        var map = (this.transform as RectTransform).rect.size;
        float xRatio = pos.x / map.x;
        float yRatio = pos.y / map.y;
        return new Vector3(_WorldWidth * xRatio, _WorldHeight * yRatio, 0);
    }

    public Vector3 WorldToRectPoint(Vector2 pos)
    {
        float xRatio = pos.x / _WorldWidth;
        float yRatio = pos.y / _WorldHeight;
        var map = (this.transform as RectTransform).rect.size;
        return new Vector3(map.x * xRatio, map.y * yRatio, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerDrag.transform as RectTransform, eventData.position, null, out pos))
        {
            pos = ResetOutRangePos(pos);
            var p = TransToWorldPoint(pos);
            p.z = Camera.main.transform.position.z;
            Camera.main.transform.position = p;
            _Frame.transform.localPosition = pos;
        }
    }

    Vector2 ResetOutRangePos(Vector2 pos)
    {
        var w = _Frame.rect.width / 2;
        var h = _Frame.rect.height / 2;
        var worldWidth = _RectTf.rect.width;
        var worldHeight = _RectTf.rect.height;
        if (pos.x <= -worldWidth / 2 + w)
        {
            pos.x = -worldWidth / 2 + w;
        }
        else if (pos.x >= worldWidth / 2 - w)
        {
            pos.x = worldWidth / 2 - w;
        }
        if (pos.y <= -worldHeight / 2 + h)
        {
            pos.y = -worldHeight / 2 + h;
        }
        else if (pos.y >= worldHeight / 2 - h)
        {
            pos.y = worldHeight / 2 - h;
        }
        return pos;
    }

    public void SetPos(Vector3 worldPos)
    {
        _Frame.transform.localPosition = WorldToRectPoint(worldPos);
    }
}
