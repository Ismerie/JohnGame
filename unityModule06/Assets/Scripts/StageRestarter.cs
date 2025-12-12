using UnityEngine.SceneManagement;

public static class StageRestarter
{
    public static void RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
