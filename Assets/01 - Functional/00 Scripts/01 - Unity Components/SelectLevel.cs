using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
	public Dropdown typeOfAlgo;

	public void OnSelectNewLevel()
	{
		Mode mode = (Mode)typeOfAlgo.value;
		Texture2D mapData = this.GetComponentsInChildren<RawImage>()[0].texture as Texture2D;
		MapDataStore.algoType = mode;
		MapDataStore.textureMapGloball = mapData;
		SceneManager.LoadScene("SampleScene");
	}
}
