using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (Graph))]
public class GraphView : MonoBehaviour {
  #region Properties

  #region Attributes
  [SerializeField] GameObject nodeViewPrefab;
  [SerializeField] NodeView[,] nodeViews;
  // [SerializeField] Color baseColor = Color.white;
  // [SerializeField] Color wallColor = Color.black;
  #endregion
  #region Getters & Setters
  public NodeView[,] NodeViews{
    get{
      return this.nodeViews;
    }
    set{
      this.nodeViews = value;
    }
  }
  #endregion
  #endregion
  #region Behaviours
  #region Life Cycle Hooks

  #endregion
  #region Procedures
  public void Init(Graph _graph){
    if(_graph==null){
      Debug.LogError("GraphView no graph initialize!");
      return;
    }
    nodeViews = new NodeView[_graph.Width,_graph.Height];
    foreach (Node n in _graph.Nodes)
    {
      GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
      NodeView nodeView = instance.GetComponent<NodeView>();
      if(nodeView!=null){
        nodeView.Init(n);
        nodeViews[n.XIndex,n.YIndex] = nodeView;
        Color originalColor = MapData.GetColorFromNodeType(n.Type);
        nodeView.ColorNode(originalColor);
      }
    }
  }
  public void ColorNodes(List<Node> _nodes, Color color, bool _lerpColor = false, float lerpValue = 0.5f){
    foreach (Node n  in _nodes){
      if(n!=null){
        NodeView nodeView = nodeViews[n.XIndex,n.YIndex];
        Color newColor = color;
        if(_lerpColor){
          Color originalColor = MapData.GetColorFromNodeType(n.Type);
          newColor = Color.Lerp(originalColor,newColor,lerpValue);
        }
        if(nodeView!=null){
          nodeView.ColorNode(newColor);
        }
      }
    }
  }
  #endregion
  #region Functions

  #endregion
  #endregion

}