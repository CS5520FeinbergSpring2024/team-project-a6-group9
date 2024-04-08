using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public bool openDoors = true;

    public LoadBar loadBar;

    private Animation anim;

    void Start()
    {
      anim = this.GetComponent<Animation>();

      if(openDoors)
      {
        anim.Play("OpenDoors");
      }
    }

    public void LoadLevel(int sceneIndex)
    {
      anim.Play("CloseDoors");
      StartCoroutine(LoadLevelAsync(sceneIndex));
    }

    IEnumerator LoadLevelAsync(int sceneIndex)
    {
      yield return new WaitForSeconds(0.5f);

      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

      while(!operation.isDone)
      {
        float progress = Mathf.Clamp01(operation.progress / 0.9f);
        loadBar.progress = 1 - progress;
        yield return null;
        loadBar.saveRotation();
      }
    }
}
