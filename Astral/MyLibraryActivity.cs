
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Astral
{
    [Activity(Label = "My Library")]
    public class MyLibraryActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.MyLibrary);

            string[] mySongList = Resources.GetStringArray(Resource.Array.my_song_list);
            string[] myLibraryList = Resources.GetStringArray(Resource.Array.my_library_list);
            var myLibraryButton = FindViewById<Button>(Resource.Id.myLibraryButton);
            var myPlayerButton = FindViewById<Button>(Resource.Id.myPlayerButton);

            ListView listView = FindViewById<ListView>(Resource.Id.menu_list);

            ArrayAdapter<String> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, myLibraryList);

            listView.Adapter = adapter;

            myLibraryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MyLibraryActivity));
                StartActivity(intent);
            };

            myPlayerButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

            listView.OnItemClickListener = new ListOnClickListener(mySongList,listView);
            //Console.WriteLine(listView.SelectedItemPosition;
        }

        public class ListOnClickListener: Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private string[] mySongList;
            private ListView listView;

            public ListOnClickListener(string[] mysonglist, ListView listview)
            {
                mySongList = mysonglist;
                listView = listview;
            }

            public void OnItemClick (AdapterView parent, View view, int position, long id)
            {
                if (id == 3)
                {
                    ArrayAdapter<String> adapter = new ArrayAdapter<string>(Android.App.Application.Context, Android.Resource.Layout.SimpleListItem1, mySongList);

                    listView.Adapter = adapter;
                }
            }
        }
    }
}
