using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable<T> {

  #region Properties

  #region Attributes
  List<T> data;
  #endregion
  #region Getters & Setters
  public List<T> Data {
    get => this.data;
    set => this.data = value;
  }
  public int Count{
    get=>data.Count;
  }
  #endregion
  #endregion
  #region Behaviours
  #region Life Cycle Hooks

  #endregion
  #region Procedures
  public void Enqueue (T _item) {
    data.Add (_item);
    int childIndex = data.Count - 1;
    while (childIndex > 0) {
      int parentIndex = (childIndex - 1) / 2;
      if (data[childIndex].CompareTo (data[parentIndex]) >= 0) {
        break;
      }
      T temporary = data[childIndex];
      data[childIndex] = data[parentIndex];
      data[parentIndex] = temporary;
      childIndex = parentIndex;
    }
  }
  public T Dequeue () {
    int lastIndex = data.Count - 1;
    T frontItem = data[0];
    data[0] = data[lastIndex];
    data.RemoveAt (lastIndex);
    lastIndex--;
    int parentIndex = 0;
    while (true) {
      int childIndex = parentIndex * 2 + 1;
      if (childIndex > lastIndex) {
        break;
      }
      int rightChild = childIndex + 1;
      if (rightChild <= lastIndex && data[rightChild].CompareTo (data[childIndex]) < 0) {
        childIndex = rightChild;
      }
      if (data[parentIndex].CompareTo (data[childIndex]) <= 0) {
        break;
      }
      T temporary = data[parentIndex];
      data[parentIndex] = data[childIndex];
      data[childIndex] = temporary;
      parentIndex = childIndex;
    }
    return frontItem;
  }

  #endregion
  #region Functions
  public PriorityQueue () {
    this.Data = new List<T> ();
  }
  public T Peak () {
    T frontItem = data[0];
    return frontItem;
  }
  public bool Contains (T _item) {
    return data.Contains(_item);
  }
  public List<T> ToList(){
    return data;
  }
}
#endregion
#endregion