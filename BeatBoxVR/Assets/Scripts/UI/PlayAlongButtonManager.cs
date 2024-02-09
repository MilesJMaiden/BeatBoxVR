using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class PlayAlongButtonManager : MonoBehaviour
{
    public int currentSongID = 0;

    private bool isPaused = false;

    public Button PlayButton;
    public Button PauseButton;

    private PlayAlongDetailLoader loader;
    public TextMeshProUGUI currentAudioTimeTMP;
    public Image ProgressBar;

    // Start is called before the first frame update
    void Start()
    {
        loader = GetComponent<PlayAlongDetailLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (loader.currBalancedTrack.isPlaying)
        {
            setCurrentAudioTime();
        }
    }

    public void Skip ()
    { 

            if (loader.currBalancedTrack.time + 10 <= loader.currBalancedTrack.clip.length)
            {
                loader.currBalancedTrack.time += 10;
                loader.currDrumTrack.time += 10;
            } else
            {
                loader.currBalancedTrack.time = loader.currBalancedTrack.clip.length;
                loader.currDrumTrack.time = loader.currBalancedTrack.clip.length;
            }
        setCurrentAudioTime();
    }

    public void  Rewind()
    {
        if (loader.currBalancedTrack.time - 10 >= 0)
        {
            loader.currBalancedTrack.time -= 10;
            loader.currDrumTrack.time -= 10;
        }
        else
        {
            loader.currBalancedTrack.time = 0;
            loader.currDrumTrack.time = 0;
        }

        setCurrentAudioTime();

    }

    private void setCurrentAudioTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(loader.currBalancedTrack.time);
        currentAudioTimeTMP.text = time.ToString(@"m\:ss");
        //ProgressBar.fillAmount = 0.08f + ((loader.currBalancedTrack.time-0)*2*(.42f-0.08f))/(loader.SongPlaylist[currentSongID].songLengthSec-0);
        //        ProgressBar.fillAmount = (83.333333f * (loader.currBalancedTrack.time- 0.8f)) / (83.333333f * (.92f-0.08f));
        ProgressBar.fillAmount = 0.085f + (((loader.currBalancedTrack.time - 0)*(0.92f - 0.085f)) / (loader.SongPlaylist[currentSongID].songLengthSec - 0));

    }


    public void setCurrentAudioTime(float clickedTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(clickedTime);
        currentAudioTimeTMP.text = time.ToString(@"m\:ss");
        //ProgressBar.fillAmount = 0.08f + ((loader.currBalancedTrack.time-0)*2*(.42f-0.08f))/(loader.SongPlaylist[currentSongID].songLengthSec-0);
        //        ProgressBar.fillAmount = (83.333333f * (loader.currBalancedTrack.time- 0.8f)) / (83.333333f * (.92f-0.08f));
        ProgressBar.fillAmount = 0.085f + (((clickedTime - 0) * (0.92f - 0.085f)) / (loader.SongPlaylist[currentSongID].songLengthSec - 0));

    }

    public void decreaseSongID() {
        currentSongID--;
        if (currentSongID < 0) {
            currentSongID = loader.SongPlaylist.Length-1;
        }
        Debug.Log(currentSongID);

        if (loader.currBalancedTrack.isPlaying)
        {
            loader.currBalancedTrack.Stop();
            loader.currDrumTrack.Stop();
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
            //setCurrentAudioTime();
        }
        loader.currBalancedTrack.time = 0;
        loader.currDrumTrack.time = 0;
        setCurrentAudioTime();

        loader.loadSong(currentSongID);
    }


    public void increaseSongID()
    {
        currentSongID++;
//        Debug.Log(loader.SongPlaylist);
        if (currentSongID >= loader.SongPlaylist.Length)
        {
            currentSongID = 0;
        }
        Debug.Log(currentSongID);
        if(loader.currBalancedTrack.isPlaying)
        {
            loader.currBalancedTrack.Stop();
            loader.currDrumTrack.Stop();
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
           // setCurrentAudioTime();
        }


        loader.currBalancedTrack.time = 0;
        loader.currDrumTrack.time = 0;
        setCurrentAudioTime();

        loader.loadSong(currentSongID);
    }

    public void Play()
    {
        //loader.currBalancedTrack.time
        Debug.Log("Playbutton pressed");
        if (isPaused)
        {
            loader.currBalancedTrack.UnPause();
            loader.currDrumTrack.UnPause();
            isPaused = false;
        } else
        {
            loader.currBalancedTrack.Play();
            loader.currDrumTrack.Play();
        }
        PauseButton.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(false);
    }

    public void Pause()
    {
        loader.currBalancedTrack.Pause();
        loader.currDrumTrack.Pause();
        PauseButton.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(true);
    }
}
