using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapSystem : MonoBehaviour
{
    public int totalLaps = 3; // Jumlah total putaran yang diperlukan untuk menyelesaikan balapan
    public Text lapText; // UI Text untuk menampilkan informasi lap
    public GameObject finishText; 
    
    private int currentLap = 0;

    void Start()
    {
        UpdateLapUI();
    }

    void OnTriggerEnter(Collider other)
    {
        // Memeriksa apakah objek yang masuk ke dalam trigger adalah objek kendaraan (sesuaikan tag atau komponen lain jika perlu)
        if (other.CompareTag("Player"))
        {
            CompleteLap();
        }
    }

    // Panggil metode ini saat balapan selesai satu lap
    void CompleteLap()
    {
        currentLap++;

        // Periksa apakah balapan sudah selesai
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
