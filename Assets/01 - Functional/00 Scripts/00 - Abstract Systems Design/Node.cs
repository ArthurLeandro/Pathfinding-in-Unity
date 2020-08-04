using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node : IComparable<Node> {

  #region Properties

  #region Attributes
  NodeType nodeType = NodeType.OPEN;
  int xIndex = -1;
  int yIndex = -1;
  float distanceTraveled = Mathf.Infinity;
  Vector3 position;
  List<Node> neighbors = new List<Node> ();
  Node previousNode = null;
  float priority;
  #endregion
  #region Getters & Setters
  public Vector3 Position {
    get {
      return this.position;
    }
    set {
      this.position = value;
    }
  }
  public NodeType Type {
    get {
      return nodeType;
    }
    set { this.nodeType = value; }
  }
  public List<Node> Neighbors {
    get {
      return this.neighbors;
    }
    set {
      this.neighbors = value;
    }
  }
  public int XIndex {
    get {
      return this.xIndex;
    }
    set {
      this.xIndex = value;
    }
  }
  public int YIndex {
    get {
      return this.yIndex;
    }
    set {
      this.yIndex = value;
    }
  }
  public float DistanceTraveled {
    get {
      return this.distanceTraveled;
    }
    set {
      this.distanceTraveled = value;
    }
  }
  public float Priority {
    get {
      return this.priority;
    }
    set {
      this.priority = value;
    }
  }
  public Node Previous {
    get {
      return this.previousNode;
    }
    set {
      this.previousNode = value;
    }
  }
  #endregion
  #endregion
  #region Behaviours

  #region Procedures
  public void Reset () {
    this.previousNode = null;
  }

  #endregion
  #region Functions
  public Node (int _xIndex, int _yIndex, NodeType _type) {
    this.xIndex = _xIndex;
    this.yIndex = _yIndex;
    this.nodeType = _type;
  }

  public int CompareTo (Node _node) {
    if (this.priority < _node.Priority) {
      return -1;
    } else if (this.priority > _node.priority) {
      return 1;
    } else {
      return 0;
    }
  }
  #endregion
  #endregion

}