using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PluckSynth : MonoBehaviour
{
    public static PluckSynth Instance { get; private set; }

    private AudioSource audioSource;
    private float sampleRate = 44100f;

    private class SynthVoice
    {
        public float frequency;
        public float phase;
        public float amplitude;
        public float decayRate;

        public SynthVoice(float freq, float initialAmplitude, float decay)
        {
            frequency = freq;
            phase = 0f;
            amplitude = initialAmplitude;
            decayRate = decay;
        }
    }

    private List<SynthVoice> activeVoices = new List<SynthVoice>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayPluck(float newFrequency, float amplitude, float decaySpeed)
    {
        activeVoices.Add(new SynthVoice(newFrequency, amplitude, decaySpeed)); // Add a new note
        audioSource.Play();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (activeVoices.Count == 0) return;

        float timeStep = 1f / sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            float sample = 0f;

            for (int v = activeVoices.Count - 1; v >= 0; v--) // Loop through all voices
            {
                SynthVoice voice = activeVoices[v];

                voice.phase += voice.frequency * 2 * Mathf.PI * timeStep;
                sample += Mathf.Sin(voice.phase) * voice.amplitude; // Sum all voices

                if (voice.phase > Mathf.PI * 2)
                    voice.phase -= Mathf.PI * 2;

                voice.amplitude *= Mathf.Exp(-voice.decayRate * timeStep);

                if (voice.amplitude < 0.0001f) // Remove finished voices
                {
                    activeVoices.RemoveAt(v);
                }
            }

            // Normalize to avoid clipping
            sample = Mathf.Clamp(sample, -1.0f, 1.0f);

            for (int j = 0; j < channels; j++)
            {
                data[i + j] = sample;
            }
        }
    }
}
