using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayAlongDetailLoader : MonoBehaviour
{

    public SongInformation[] SongPlaylist;

    public TextMeshProUGUI songTitleValueTMP;
    public TextMeshProUGUI songArtistValueTMP;
    public TextMeshProUGUI songTempoTMP;
    public TextMeshProUGUI songKeyTMP;
    public TextMeshProUGUI songDifficultyTMP;
    public TextMeshProUGUI songLengthTMP;
    public TextMeshProUGUI songLengthTMP2;
    public Image AlbumArt;
    public AudioSource currDrumTrack;
    public AudioSource currBalancedTrack;



    //Online calculator to convert time to seconds (need seconds for calculating progress bar).
    //https://www.calculatorsoup.com/calculators/time/time-to-decimal-calculator.php


    public void loadSong(int id)
    {
        songTitleValueTMP.text = SongPlaylist[id].songName;
        songArtistValueTMP.text = SongPlaylist[id].songArtist;
        songTempoTMP.text = SongPlaylist[id].songBPM.ToString();
        songKeyTMP.text = SongPlaylist[id].songKey;
        songDifficultyTMP.text = SongPlaylist[id].songDifficulty;

        TimeSpan time = TimeSpan.FromSeconds(SongPlaylist[id].songLengthSec);
        songLengthTMP.text = time.ToString(@"m\:ss");
        songLengthTMP2.text = time.ToString(@"m\:ss");

        AlbumArt.sprite = SongPlaylist[id].AlbumCover;
        currDrumTrack.clip = SongPlaylist[id].drumTrack;
        currBalancedTrack.clip = SongPlaylist[id].rebalancedTrack;

    }


    



    // Start is called before the first frame update
    void Start()
    {
        loadSong(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //Defines region elements used for texture
    [System.Serializable]
    public struct SongInformation
    {
        public string songName;
        public string songArtist;
        public int songBPM;
        public string songKey;
        public string songDifficulty;
        public int songLengthSec;
        public Sprite AlbumCover;
        public AudioClip rebalancedTrack;
        public AudioClip drumTrack;
    }

}
