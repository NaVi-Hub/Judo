using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyLittleClub
{
    [Service]
    public class MyService : Service
    {
        //MUSIC
        MediaPlayer mediaPlayer;
        //MUSIC
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            mediaPlayer = MediaPlayer.Create(this, Resource.Raw.BGmusic);
            mediaPlayer.SetVolume((float)0.015, (float)0.015);
            mediaPlayer.Start();
            return StartCommandResult.Sticky;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            mediaPlayer.Stop();
            mediaPlayer.Release();
            mediaPlayer = null;
        }
    }
}