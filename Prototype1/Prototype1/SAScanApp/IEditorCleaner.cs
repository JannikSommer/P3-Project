using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SAScanApp {
    public interface IEditorCleaner {
        void CleanText(Editor editor);
    }
}
