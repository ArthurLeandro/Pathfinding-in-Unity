using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MapData : MonoBehaviour {
  #region Properties

  #region Attributes 
  string resourcePath = "MapData";
  [SerializeField] string fileName = " ";
  int width = 10;
  int height = 5;
  [SerializeField] TextAsset textAsset;
  [SerializeField] Texture2D textureMap;
  [SerializeField] Color32 openColor = Color.white;
  [SerializeField] Color32 blockedColor = Color.black;
  [SerializeField] Color32 lightTerrainColor = new Color32 (124, 194, 78, 255);
  [SerializeField] Color32 mediumTerrainColor = new Color32 (252, 255, 52, 255);
  [SerializeField] Color32 heavyTerrainColor = new Color32 (255, 129, 12, 255);
  static Dictionary<Color32, NodeType> terrainLookupTable = new Dictionary<Color32, NodeType> ();
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
  void Awake () {
    if (this.textureMap == null) {
      this.textureMap = Resources.Load (resourcePath + "/" + fileName) as Texture2D;
    } else if (this.textAsset == null) {
      this.textAsset = Resources.Load (resourcePath + "/" + fileName) as TextAsset;
    }
    SetupLookupTable ();
  }
  #endregion
  #region Procedures
  public int[, ] MakeMap () {
    List<string> lines = new List<string> ();
    if (textureMap != null) {
      Debug.Log ("It's gonna load Map from texture.");
      lines = GetMapFromTexture (this.textureMap);
    } else {
      Debug.Log ("It's gonna load Map from text.");
      lines = GetTextFromFile (this.textAsset);
    }
    SetDimensions (lines);
    int[, ] map = new int[this.width, this.height];
    for (int y = 0; y < this.height; y++) {
      for (int x = 0; x < this.width; x++) {
        if (lines[y].Length > x) {
          map[x, y] = (int) Char.GetNumericValue (lines[y][x]);
        }
      }
    }
    return map;
  }
  public void SetDimensions (List<string> _textLines) {
    height = _textLines.Count;
    foreach (string line in _textLines) {
      if (line.Length > width) {
        width = line.Length;
      }
    }
  }
  public void SetupLookupTable () {
    terrainLookupTable.Add (openColor, NodeType.OPEN);
    terrainLookupTable.Add (blockedColor, NodeType.BLOCKED);
    terrainLookupTable.Add (lightTerrainColor, NodeType.LIGHTTERRAIN);
    terrainLookupTable.Add (mediumTerrainColor, NodeType.MEDIUMTERRAIN);
    terrainLookupTable.Add (heavyTerrainColor, NodeType.HEAVYTERRAIN);
  }
  #endregion
  #region Functions
  public List<string> GetTextFromFile (TextAsset _textAsset) {
    List<string> lines = new List<string> ();
    if (_textAsset != null) {
      string textData = _textAsset.text;
      string[] delimeters = { "\r\n", "\n" };
      lines.AddRange (textData.Split (delimeters, System.StringSplitOptions.None));
      lines.Reverse ();
    } else {
      Debug.Log ("MapData GetTextFromFile Error: invalid TextAsset");
    }
    return lines;
  }
  public List<string> GetTextFromFile () {
    if (textAsset == null) {
      textAsset = (TextAsset) Resources.Load (resourcePath + "/Maze");
    }
    return GetTextFromFile (this.textAsset);
  }
  public List<string> GetMapFromTexture (Texture2D _texture) {
    List<string> lines = new List<string> ();
    for (int y = 0; y < _texture.height; y++) {
      string newLine = "";
      for (int x = 0; x < _texture.width; x++) {
        Color pixelColor = _texture.GetPixel (x, y);
        if (terrainLookupTable.ContainsKey (pixelColor)) {
          NodeType nodeType = terrainLookupTable[pixelColor];
          int nodeTypeNum = (int) nodeType;
          newLine += nodeTypeNum;
        } else {
          newLine += '0';
        }
      }
      lines.Add (newLine);
    }
    return lines;
  }
  public List<string> GetMapFromTexture () {
    if (textureMap == null) {
      textureMap = (Texture2D) Resources.Load (resourcePath + "/" + fileName);
    }
    return GetMapFromTexture (this.textureMap);
  }
  public static Color GetColorFromNodeType (NodeType _type) {
    if (terrainLookupTable.ContainsValue (_type)) {
      Color colorKey = terrainLookupTable.FirstOrDefault (x => x.Value == _type).Key;
      return colorKey;
    }
    return Color.white;
  }
  #endregion
  #endregion
}