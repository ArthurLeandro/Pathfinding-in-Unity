using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
  #region Properties

  #region Attributes
  [SerializeField] Node[, ] nodes;
  [SerializeField] List<Node> walls = new List<Node> ();
  int[, ] mapData;
  int width;
  int height;

  [SerializeField] static readonly Vector2[] allDirections = {
    new Vector2 (0, 1),
    new Vector2 (1, 1),
    new Vector2 (1, 0),
    new Vector2 (1, -1),
    new Vector2 (0, -1),
    new Vector2 (-1, -1),
    new Vector2 (-1, 0),
    new Vector2 (-1, 1)
  };
  #endregion
  #region Getters & Setters
  public Node[, ] Nodes {
    get {
      return this.nodes;
    }
    set {
      this.nodes = value;
    }
  }
  public int Width {
    get {
      return this.width;
    }
    set {
      this.width = value;
    }
  }
  public int Height {
    get {
      return this.height;
    }
    set {
      this.height = value;
    }
  }
  #endregion
  #endregion
  #region Behaviours
  #region Life Cycle Hooks

  #endregion
  #region Procedures
  public void Init (int[, ] _mapData) {
    this.mapData = _mapData;
    this.width = mapData.GetLength (0);
    this.height = mapData.GetLength (1);
    nodes = new Node[width, height];
    for (int y = 0; y < this.height; y++) {
      for (int x = 0; x < this.width; x++) {
        NodeType type = (NodeType) this.mapData[x, y];
        Node newNode = new Node (x, y, type);
        nodes[x, y] = newNode;
        newNode.Position = new Vector3 (x, 0, y);
        if (type == NodeType.BLOCKED) {
          walls.Add (newNode);
        }
      }
    }
    for (int y = 0; y < height; y++) {
      for (int x = 0; x < width; x++) {
        if (nodes[x, y].Type != NodeType.BLOCKED) {
          nodes[x, y].Neighbors = GetNeighbors (x, y);
        }
      }
    }
  }
  #endregion
  #region Functions
  public bool IsWitinBounds (int _x, int _y) {
    return (_x >= 0 && _x < this.width && _y >= 0 && _y < this.height);
  }
  public List<Node> GetNeighbors (int _x, int _y, Node[, ] _nodeArray, Vector2[] _direction) {
    List<Node> neighborNodes = new List<Node> ();
    foreach (Vector2 dir in _direction) {
      int newX = _x + (int) dir.x;
      int newY = _y + (int) dir.y;
      if (IsWitinBounds (newX, newY) && _nodeArray[newX, newY] != null && _nodeArray[newX, newY].Type != NodeType.BLOCKED) {
        neighborNodes.Add (_nodeArray[newX, newY]);
      }
    }
    return neighborNodes;
  }
  public List<Node> GetNeighbors (int _x, int _y) {
    return GetNeighbors (_x, _y, nodes, allDirections);
  }
  public float GetNodeDistance (Node _source, Node _target) {
    int distanceX = Mathf.Abs (_source.XIndex - _target.XIndex);
    int distanceY = Mathf.Abs (_source.YIndex - _target.YIndex);
    int min = Mathf.Min (distanceX, distanceY);
    int max = Mathf.Max (distanceX, distanceY);
    int diagonalSteps = min;
    int straightSteps = max - min;
    return (1.4f * diagonalSteps + straightSteps);
  }
  public int GetManhattanDistance (Node _source, Node _target) {
    int distanceX = Mathf.Abs (_source.XIndex - _target.XIndex);
    int distanceY = Mathf.Abs (_source.YIndex - _target.YIndex);
    return (distanceX+distanceY);
  }
  #endregion
  #endregion
}