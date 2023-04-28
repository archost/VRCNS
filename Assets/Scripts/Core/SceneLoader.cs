using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance = null;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private TextMeshProUGUI loadingText;

    private AsyncOperation currentLoading = null;

    private float progress = 0f;

    private float totalProgress = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public static void LoadScene(int sceneIndex) => instance.SceneLoad(sceneIndex);

    private async void SceneLoad(int sceneIndex)
    {
        Scene s = SceneManager.GetActiveScene();
        totalProgress = 2f;
        progress = 0f;
        progressBar.fillAmount = 0f;
        
        currentLoading = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        loadingScreen.SetActive(true);

        do
        {
            await Task.Delay(100);
            progress = currentLoading.progress;
        } while (!currentLoading.isDone);

        SceneManager.UnloadSceneAsync(s);

        if (Initialization.Instance != null)
        {
            do
            {
                await Task.Delay(100);
                progress = currentLoading.progress + Initialization.Instance.Progress;
            } while (!Initialization.Instance.IsDone);
            Initialization.Instance = null;
        }
        else
        {
            progress = totalProgress;
        }

        currentLoading = null;
        loadingScreen.SetActive(false);        
    }

    public static void LoadSceneSilent(int sceneIndex) => instance.SceneLoadSilent(sceneIndex);

    private void SceneLoadSilent(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void Update()
    {

        if (currentLoading != null)
        {
            //Debug.Log(fillTarget);
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, progress / totalProgress, 3 * Time.deltaTime);
            loadingText.text = $"Загрузка... {Mathf.RoundToInt(progress / totalProgress * 100)}%";
        }
    }
}