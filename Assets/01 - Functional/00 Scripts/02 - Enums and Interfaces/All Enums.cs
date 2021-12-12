public enum NodeType
{
	OPEN = 0,
	BLOCKED = 1,
	LIGHTTERRAIN = 2,
	MEDIUMTERRAIN = 3,
	HEAVYTERRAIN = 5,
	GOAL = 6,
	START = 7
}

public enum Mode
{
	BreadthFirstSearch = 3,
	Dijkstra = 2,
	GreedyBestFirst = 0,
	AStar = 1
}