using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour {

	private AsyncOperation async;
	public GameObject[] loadingImages; 

	// Use this for initialization
	void Start() 
	{	
		DisplayRandomLoadingImages (); 
		StartCoroutine (LoadTheScene()); 
	}

	IEnumerator LoadTheScene()
	{
		async = Application.LoadLevelAsync("DefendTheBase"); 
		while (!async.isDone)
		{ 
			yield return 0;
		}
	}

	void DisplayRandomLoadingImages()
	{
		foreach (var item in loadingImages) {
			item.SetActive(false); 
		}

		loadingImages[Random.Range (0,loadingImages.Length - 1)].SetActive(true); 
	}
}
