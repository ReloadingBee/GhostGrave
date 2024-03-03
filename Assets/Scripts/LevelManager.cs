using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TMP_Text waveText;
    public AudioClip waveStartSound;
    public AudioClip waveEndSound;
    public AudioClip wavesClearedSound;

    public async void AnnounceWave(int wave)
    {
        waveText.text = $"Wave {wave + 1} started!";
        AudioSystem.Play(waveStartSound);
        await new WaitForSeconds(2f);
        waveText.text = "";
    }

    public async void WaveEnd(int wave)
    {
        waveText.text = $"Wave {wave + 1} ended!";
        AudioSystem.Play(waveEndSound);
        await new WaitForSeconds(2f);
        waveText.text = "";
    }

    public async void ClearWaves()
    {
        waveText.text = $"All waves cleared!";
        AudioSystem.Play(wavesClearedSound);
        await new WaitForSeconds(2f);
        waveText.text = "";
    }
}
