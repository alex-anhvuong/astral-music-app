using System.IO;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using Android.Util;


namespace Astral
{
    [Activity(Label = "Astral", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected MediaPlayer player = new MediaPlayer();
        public void StartPlayer(string fileName)
        {
            player.Reset();
            player.SetDataSource("Assets/shapeofyou.mp3");
            player.Prepare();
            player.Start();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            var playButton = FindViewById<Button>(Resource.Id.playButton);

            playButton.Click += (sender, e) =>
            {
                StartPlayer("shapeofyou.mp3");
            };
        }


    }
}

