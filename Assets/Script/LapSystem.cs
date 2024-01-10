using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapSystem : MonoBehaviour
{
    public int totalLaps = 3; 
    public Text lapText; 
    public GameObject finishText; 
    
    private int currentLap = 0;

    void Start()
    {
        UpdateLapUI();
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            CompleteLap();
        }
    }

    
    void CompleteLap()
    {
        currentLap++;

        
        if (currentLap > totalLaps)
        {
            RaceFinished();
        }
        else
        {
            UpdateLapUI();
        }
    }

    void UpdateLapUI()
    {
        if (lapText != null)
        {
            lapText.text = "Lap: " + currentLap + "/" + totalLaps;
        }
    }

    void RaceFinished()
    {
        Debug.Log("test");
        // Implementasi logika saat balapan selesai, contohnya menampilkan pesan kemenangan atau memulai putaran baru
        Time.timeScale = 0f;
        if (finishText != null)
        {
            finishText.SetActive(true);
        }
    }
    
}
