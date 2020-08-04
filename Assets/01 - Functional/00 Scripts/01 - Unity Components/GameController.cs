using UnityEngine;

// [RequireComponent(typeof(Graph)),RequireComponent(typeof(MapData)),RequireComponent(typeof(Pathfinder))]
public class GameController : MonoBehaviour {

  #region Properties

  #region Attributes
  [SerializeField] MapData mapData;
  [SerializeField] Graph graph;
  [SerializeField] Pathfinder pathfinder;
  [SerializeField] int startX = 0;
  [SerializeField] int startY = 0;
  [SerializeField] int goalX = 15;
  [SerializeField] int goalY = 1;
  [SerializeField] float timeStep = 0.1f;

  #endregion
  #region Getters & Setters

  #endregion
  #endregion
  #region Behaviours
  #region Life Cycle Hooks
  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  void Start () {
    if (this.mapData != null && this.graph != null) {
      int[, ] mapInstance = mapData.MakeMap ();
      graph.Init (mapInstance);
      GraphView graphView = graph.gameObject.GetComponent<GraphView> ();
      if (graphView != null) {
        graphView.Init (graph);
      }
      if(graph.IsWitinBounds(startX,startY) && graph.IsWitinBounds(goalX,goalY)&& pathfinder!=null){
        Node startNode = graph.Nodes[startX,startY];
        Node goalNode = graph.Nodes[goalX,goalY];
        pathfinder.Init(graph,graphView,startNode,goalNode);
        StartCoroutine(pathfinder.SearchRoutine(timeStep));
      }
    }
  }
  #endregion
  #region Procedures

  #endregion
  #region Functions

  #endregion
  #endregion
}