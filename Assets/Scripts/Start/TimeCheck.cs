using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeCheck : MonoBehaviour
{
    void OnEnable()
    {
        TimeProgress.OnYear2050Reached += HandleYear2050Reached;
    }

    void OnDisable()
    {
        TimeProgress.OnYear2050Reached -= HandleYear2050Reached;
    }

    void HandleYear2050Reached()
    {
        SceneManager.LoadScene("End");
    }
}
