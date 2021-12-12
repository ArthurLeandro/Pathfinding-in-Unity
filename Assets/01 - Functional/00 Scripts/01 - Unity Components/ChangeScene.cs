using UnityEngine;
public class ChangeScene : MonoBehaviour
{
	void Update()
	{
		if (Input.anyKeyDown)
			UnityEngine.SceneManagement.SceneManager.LoadScene("Level Select");
	}
}