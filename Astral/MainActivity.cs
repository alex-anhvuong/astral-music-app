using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using Android.OS;
using Android.Media;
using Android.Util;
using Android.Views;

namespace Astral
{
    [Activity(Label = "Astral", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        //int count = 1;
        bool isPlaying;
        protected MediaPlayer player;
        int indexOfCurrentSong = 0;

        //**
        //*
        //  Assign a song to the player
        //*
        //**  
        public void StartPlayer(string fileName)
        {
            //  player = player ?? MediaPlayer.Create(this, Resource.Raw.shapeofyou);
            player = new MediaPlayer();
            AssetManager asset = this.Assets;
            var afd = asset.OpenFd(fileName);

            player.SetDataSource(afd);
            player.Prepare();
            player.Start();
        }

        //**
        //*
        //  Autoplay the next song
        //*
        //** 
        protected void OnSongCompletion(string[] songList)
        {
            player.Completion += delegate {
                if (indexOfCurrentSong >= songList.Length - 1) return;
                indexOfCurrentSong++;
                StartPlayer(songList[indexOfCurrentSong]);
            };
        }

        //**

        //*
        //  When play button is clicked.
        //  We either play a new song/resume to the old song Or pause the player
        //*
        //** 
        protected void OnPlayButton(Button playButton, string[] songList)
        {
            //  Log.Debug("astral-app", indexOfCurrentSong.ToString());

            if (!isPlaying) //  PLAY
            {
                //  PLAY A NEW SONG
                //  or
                //  RESUME THE SONG if the player is not null
                if (player == null) StartPlayer(songList[indexOfCurrentSong]);
                else player.Start();
                OnSongCompletion(songList);

                //  Switch the status of isPlaying and the icon of playButtonx
                isPlaying = true;
                playButton.SetBackgroundResource(Resource.Drawable.pause);
            }
            else    //  PAUSE
            {
                player.Pause();
                isPlaying = false;
                playButton.SetBackgroundResource(Resource.Drawable.play);

            }
        }

        //**
        //*
        //  The function of either fastforward button or rewind button
        //  - Change the status of the "playButton"
        //  - Play a new song from the song list (if applicable)
        //  - Choose the previous or next song based on the value of "i" (-1/1)
        //  - The "condition" determines whether we are at the two ends of the list
        //*
        //**
        protected void OnFastforwardOrRewind(Button playButton, string[] songList, Func<Boolean> condition, int i)  
        {
            //  STOP THE CURRENT SONG
            //  Release the resource that player currently holding
            //  player must be null after that
            if (player != null) player.Release();
            player = null;

            //  If we run out of song (we are at the beginning of the list or the end of the list)
            //  Switch the status of isPlaying and the icon of playButton 
            if (condition())
            {
                isPlaying = false;
                playButton.SetBackgroundResource(Resource.Drawable.play);
                return;
            }

            //  CALL A NEW SONG if the player is playing
            indexOfCurrentSong += i;
            if (isPlaying) 
            {
                StartPlayer(songList[indexOfCurrentSong]);
                OnSongCompletion(songList);
            }
        }     

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.navigationBar);
            SetActionBar(toolbar);
            ActionBar.Title = "";

            string[] songList = Resources.GetStringArray(Resource.Array.song_list);

            if (savedInstanceState != null)
            {
                //Console.WriteLine(isPlaying);
                isPlaying = savedInstanceState.GetBoolean("is_playing", false);
            }

            var playButton = FindViewById<Button>(Resource.Id.playButton);
            var fastForwardButton = FindViewById<Button>(Resource.Id.fastforwardButton);
            var rewindButton = FindViewById<Button>(Resource.Id.rewindButton);
            var myLibraryButton = FindViewById<Button>(Resource.Id.myLibraryButton);

            playButton.Click += (sender, e) => OnPlayButton(playButton, songList);
            fastForwardButton.Click += (sender, e) => OnFastforwardOrRewind(playButton, songList, () => indexOfCurrentSong >= songList.Length - 1, 1);
            rewindButton.Click += (sender, e) => OnFastforwardOrRewind(playButton, songList, () => indexOfCurrentSong <= 0, -1);

            myLibraryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MyLibraryActivity));
                StartActivity(intent);
            };
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            //Console.WriteLine(isPlaying);
            outState.PutBoolean("is_playing", isPlaying);
            base.OnSaveInstanceState(outState);
        }

        

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.bottom_navigation_bar, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}
    }
}

