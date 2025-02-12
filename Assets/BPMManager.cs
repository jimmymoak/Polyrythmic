using UnityEngine;

public class BPMManager : MonoBehaviour
{
    public static BPMManager Instance { get; private set; }

    public float BPM = 120f; // Default beats per minute
    private float secondsABeat; // Time per beat

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateBPM();
    }

    public void UpdateBPM(float newBPM = -1)
    {
        if (newBPM > 0) BPM = newBPM;
        secondsABeat = 60f / BPM; // Convert BPM to beat time in seconds
    }

    public float GetBeatInterval()
    {
        return secondsABeat;
    }
}
