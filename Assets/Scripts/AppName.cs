using TMPro;
using UnityEngine;

public class AppName : MonoBehaviour
{
    [SerializeField] private string _text;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private GameObject _prefab;

    private TextMeshPro _textMesh;

    private void Start()
    {
        if (_textMesh == null) _textMesh = Instantiate(_prefab).GetComponent<TextMeshPro>();
        _textMesh.text = _text;
        _textMesh.transform.position = transform.position + (Vector3)_offset;
    }
    private void Update()
    {
        _textMesh.transform.position = transform.position + (Vector3)_offset;
    }
}
