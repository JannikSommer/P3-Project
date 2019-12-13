using System;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
using SAScanApp.Droid;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Android;
using Android.App;
using Model;


[assembly: Xamarin.Forms.Dependency(typeof(EditorCleaner))]
namespace SAScanApp.Droid {

    public class EditorCleaner : IEditorCleaner {

        public void CleanText(Editor editor) {
            editor.Text = string.Empty;
        }





    }
}