using UnityEngine;

public class NodeView : MonoBehaviour {
  #region Properties

  #region Attributes
  [SerializeField] GameObject tile;
  [SerializeField][Range (0, 0.5f)] float borderSize = 0.15f;
  #endregion
  #region Getters & Setters

  #endregion
  #endregion
  #region Behaviours
  #region Life Cycle Hooks
  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake () {
    this.tile = this.transform.GetChild (0).gameObject;
  }
  #endregion
  #region Procedures
  public void Init (Node _node) {
    if (tile != null) {
      gameObject.name = "Node(" + _node.XIndex + " , " + _node.YIndex + ")";
      gameObject.transform.position = _node.Position;
      tile.transform.localScale = new Vector3 (1f - this.borderSize, 1f - this.borderSize, 1f);
    }
  }
  public void ColorNode (Color _color, GameObject _go) {
    if (_go != null) {
      Renderer goRenderer = _go.GetComponent<Renderer> ();
      if (goRenderer != null) {
        goRenderer.material.color = _color;
      }
    }
  }
  public void ColorNode (Color _color) {
    ColorNode (_color, tile);
  }
  #endregion
  #region Functions

  #endregion
  #endregion
}