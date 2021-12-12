using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pathfinder : MonoBehaviour
{
	#region Properties

	#region Attributes
	Node startNode;
	Node goalNode;
	Graph graph;
	GraphView graphView;
	PriorityQueue<Node> frontierNodes;
	List<Node> exploredNodes;
	List<Node> pathNodes;
	[SerializeField] Color startNodesColor = Color.green;
	[SerializeField] Color goalNodesColor = Color.red;
	[SerializeField] Color frontierNodesColor = Color.magenta;
	[SerializeField] Color exploredNodesColor = Color.gray;
	[SerializeField] Color pathNodeColor = Color.cyan;
	public bool isComplete = false;
	[SerializeField] bool showIteration = true;
	[SerializeField] bool showColors = true;
	[SerializeField] bool exitOnGoal = true;
	int iterations = 0;
	[SerializeField] Mode modeOfPathfinder = Mode.GreedyBestFirst;

	#endregion
	#region Getters & Setters

	#endregion
	#endregion
	#region Behaviours
	#region Life Cycle Hooks

	#endregion
	#region Procedures
	public void Init(Graph _graph, GraphView _graphView, Node _start, Node _goal)
	{
		if (_start == null || _graph == null || _graphView == null || _goal == null)
		{
			Debug.LogWarning("PATHFINDER Init error: missing component(s)!");
			return;
		}
		if (_start.Type == NodeType.BLOCKED || _goal.Type == NodeType.BLOCKED)
		{
			Debug.LogWarning("PATHFINDER Init error: start and goal nodes must be unblocked!");
			return;
		}
		this.modeOfPathfinder = MapDataStore.algoType;
		this.graph = _graph;
		this.graphView = _graphView;
		this.startNode = _start;
		this.goalNode = _goal;
		ShowColors(graphView, startNode, goalNode);
		this.frontierNodes = new PriorityQueue<Node>();
		frontierNodes.Enqueue(_start);
		exploredNodes = new List<Node>();
		pathNodes = new List<Node>();
		for (int x = 0; x < graph.Width; x++)
		{
			for (int y = 0; y < graph.Height; y++)
			{
				graph.Nodes[x, y].Reset();
			}
		}
		this.isComplete = false;
		this.iterations = 0;
		startNode.DistanceTraveled = 0;
	}
	public void ShowColors(GraphView _graphView, Node _start, Node _goal, bool lerpColor = false, float lerpValue = 0.5f)
	{
		if (_graphView == null || _start == null || _goal == null)
		{
			return;
		}
		if (this.frontierNodes != null)
		{
			_graphView.ColorNodes(frontierNodes.ToList(), frontierNodesColor, lerpColor, lerpValue);
		}
		if (this.exploredNodes != null)
		{
			_graphView.ColorNodes(exploredNodes, exploredNodesColor, lerpColor, lerpValue);
		}
		if (this.pathNodes != null && pathNodes.Count > 0)
		{
			graphView.ColorNodes(this.pathNodes, pathNodeColor, lerpColor, lerpValue * 2f);
		}
		NodeView startNodeView = graphView.NodeViews[_start.XIndex, _start.YIndex];
		if (startNodeView != null)
		{
			startNodeView.ColorNode(this.startNodesColor);
		}
		NodeView goalNodeView = graphView.NodeViews[_goal.XIndex, _goal.YIndex];
		if (goalNodeView != null)
		{
			goalNodeView.ColorNode(this.goalNodesColor);
		}
	}
	public void ShowColors(bool lerpColor = false, float lerpValue = 0.5f)
	{
		ShowColors(this.graphView, this.startNode, this.goalNode, lerpColor, lerpValue);
	}

	public IEnumerator SearchRoutine(float timeStep = 0.1f)
	{
		float timeStart = Time.time;
		yield return null;
		while (!isComplete)
		{
			if (frontierNodes.Count > 0)
			{
				Node currentNode = frontierNodes.Dequeue();
				this.iterations++;
				if (!exploredNodes.Contains(currentNode))
				{
					exploredNodes.Add(currentNode);
				}
				if (this.modeOfPathfinder == Mode.BreadthFirstSearch)
				{

					ExpandFrontierBreadthFirst(currentNode);
				}
				else if (modeOfPathfinder == Mode.Dijkstra)
				{
					ExpandFrontierDijkstra(currentNode);
				}
				else if (modeOfPathfinder == Mode.GreedyBestFirst)
				{
					ExpandFrontierGreedyBestFirst(currentNode);
				}
				else
				{
					ExpandFrontierAStar(currentNode);
				}
				if (this.frontierNodes.Contains(this.goalNode))
				{
					this.pathNodes = GetPathNodes(this.goalNode);
					if (exitOnGoal)
					{
						isComplete = true;
						Debug.Log("PATHFINDER mode: " + modeOfPathfinder.ToString() + " path length = " + this.goalNode.DistanceTraveled.ToString());
					}
				}
				if (showIteration)
				{
					if (showColors)
					{
						ShowColors(true, 0.5f);
					}
					yield return new WaitForSeconds(timeStep);
				}
			}
			else
			{
				isComplete = true;
				Debug.Log("PATHFINDER mode: " + modeOfPathfinder.ToString() + " path length = " + this.goalNode.DistanceTraveled.ToString());
			}
		}
		ShowColors(true, 0.5f);
		Debug.Log("PATHFINDER SearchRoutine: elapsed time = " + (Time.realtimeSinceStartup - timeStart).ToString() + " seconds");
	}
	public void ExpandFrontierBreadthFirst(Node _node)
	{
		if (_node != null)
		{
			for (int i = 0; i < _node.Neighbors.Count; i++)
			{
				if (!exploredNodes.Contains(_node.Neighbors[i]) && !frontierNodes.Contains(_node.Neighbors[i]))
				{
					float distanceToNeighbor = this.graph.GetNodeDistance(_node, _node.Neighbors[i]);
					float newDistanceTraveled = distanceToNeighbor + _node.DistanceTraveled + (int)_node.Type;
					_node.Neighbors[i].DistanceTraveled = newDistanceTraveled;
					_node.Neighbors[i].Previous = _node;
					_node.Neighbors[i].Priority = exploredNodes.Count;
					frontierNodes.Enqueue(_node.Neighbors[i]);
				}
			}
		}
	}
	public void ExpandFrontierDijkstra(Node _node)
	{
		if (_node != null)
		{
			for (int i = 0; i < _node.Neighbors.Count; i++)
			{
				if (!exploredNodes.Contains(_node.Neighbors[i]))
				{
					float distanceToNeighbor = this.graph.GetNodeDistance(_node, _node.Neighbors[i]);
					float newDistanceTraveled = distanceToNeighbor + _node.DistanceTraveled + (int)_node.Type;
					if (float.IsPositiveInfinity(_node.Neighbors[i].DistanceTraveled) || newDistanceTraveled < _node.Neighbors[i].DistanceTraveled)
					{
						_node.Neighbors[i].Previous = _node;
						_node.Neighbors[i].DistanceTraveled = newDistanceTraveled;
					}
					if (!this.frontierNodes.Contains(_node.Neighbors[i]))
					{
						_node.Neighbors[i].Priority = _node.Neighbors[i].DistanceTraveled;
						frontierNodes.Enqueue(_node.Neighbors[i]);
					}
				}
			}
		}
	}
	public void ExpandFrontierGreedyBestFirst(Node _node)
	{
		if (_node != null)
		{
			for (int i = 0; i < _node.Neighbors.Count; i++)
			{
				if (!exploredNodes.Contains(_node.Neighbors[i]) && !frontierNodes.Contains(_node.Neighbors[i]))
				{
					float distanceToNeighbor = this.graph.GetNodeDistance(_node, _node.Neighbors[i]);
					float newDistanceTraveled = distanceToNeighbor + _node.DistanceTraveled + (int)_node.Type;
					_node.Neighbors[i].DistanceTraveled = newDistanceTraveled;
					_node.Neighbors[i].Previous = _node;
					if (this.graph != null)
					{
						_node.Neighbors[i].Priority = this.graph.GetNodeDistance(_node.Neighbors[i], this.goalNode);
					}
					frontierNodes.Enqueue(_node.Neighbors[i]);
				}
			}
		}
	}
	public void ExpandFrontierAStar(Node _node)
	{
		if (_node != null)
		{
			for (int i = 0; i < _node.Neighbors.Count; i++)
			{
				if (!exploredNodes.Contains(_node.Neighbors[i]))
				{
					float distanceToNeighbor = this.graph.GetNodeDistance(_node, _node.Neighbors[i]);
					float newDistanceTraveled = distanceToNeighbor + _node.DistanceTraveled + (int)_node.Type;
					if (float.IsPositiveInfinity(_node.Neighbors[i].DistanceTraveled) || newDistanceTraveled < _node.Neighbors[i].DistanceTraveled)
					{
						_node.Neighbors[i].Previous = _node;
						_node.Neighbors[i].DistanceTraveled = newDistanceTraveled;
					}
					if (!this.frontierNodes.Contains(_node.Neighbors[i]) && this.graph != null)
					{
						float distanceToGoal = this.graph.GetNodeDistance(_node.Neighbors[i], this.goalNode);
						//fscore =g score + h score
						_node.Neighbors[i].Priority = _node.Neighbors[i].DistanceTraveled + distanceToGoal;
						frontierNodes.Enqueue(_node.Neighbors[i]);
					}
				}
			}
		}
	}
	#endregion
	#region Functions
	public List<Node> GetPathNodes(Node _endNode)
	{
		List<Node> path = new List<Node>();
		if (_endNode == null)
		{
			return path;
		}
		path.Add(_endNode);
		Node currentNode = _endNode.Previous;
		while (currentNode != null)
		{
			path.Insert(0, currentNode);
			currentNode = currentNode.Previous;
		}
		return path;
	}
	#endregion
	#endregion
}