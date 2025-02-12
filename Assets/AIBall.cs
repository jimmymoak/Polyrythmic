using TMPro;
using UnityEngine;

public class AIBall : MonoBehaviour
{
    public float bounceHeight = 2f;
    public float speedMultiplyer = 1f;
    public int pitch = 0;
    public float amplitude = 0.1f;
    public float decaySpeed = 5f;

    private float secondsABeat;
    private Vector2 initialPos;
    private float lastSinValue = 0f; // Store last sine value
    private const float epsilon = 0.05f; // Small threshold for detecting ground hit

    private TextMeshProUGUI counter;

    void Awake()
    {
        counter = GetComponentInChildren<TextMeshProUGUI>();

        if (counter == null)
        {
            Debug.LogError($"AIBall '{name}' is missing a TextMeshProUGUI component!");
        }
        else
        {
            counter.text = "0";
        }

        secondsABeat = BPMManager.Instance.GetBeatInterval();
        initialPos = transform.position;
    }

    void Update()
    {
        // Compute the time value (this remains unchanged)
        float timeInBeats = (Time.time / (secondsABeat / speedMultiplyer)) % 2;

        // Compute the sine value
        float sinValue = Mathf.Sin(timeInBeats * Mathf.PI);

        // Use the absolute value for your bounce height calculations
        float absoluteSinValue = Mathf.Abs(sinValue);
        transform.position = new Vector2(transform.position.x, initialPos.y + (absoluteSinValue * bounceHeight));

        // Detect zero crossing (either upward or downward)
        // Mathf.Sign returns 1 for positive numbers, -1 for negatives, and 0 for exactly 0.
        if (Mathf.Sign(lastSinValue) != Mathf.Sign(sinValue))
        {
            PlayBounceSound();
        }

        // Store the current sine value for the next frame
        lastSinValue = sinValue;
    }



    void PlayBounceSound()
    {
        float frequency = CalculateFrequency(pitch);

        if (PluckSynth.Instance != null)
        {
            if (int.TryParse(counter.text, out int currentCount))
            {
                counter.text = (currentCount + 1).ToString();
            }
            else
            {
                Debug.LogWarning($"Invalid counter value on '{name}', resetting to 0.");
                counter.text = "1";
            }

            PluckSynth.Instance.PlayPluck(frequency, amplitude, decaySpeed);
        }
        else
        {
            Debug.LogWarning("PluckSynth not found in scene!");
        }
    }

    float CalculateFrequency(int semitoneShift)
    {
        return 440f * Mathf.Pow(2f, semitoneShift / 12f);
    }
}
