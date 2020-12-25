using Placehold.Keyboard.Hook;
using Placehold.Plugin;
using System;
using System.Linq;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FilePlugin
{
    public class Main : IPlaceholdEvent
    {
        private readonly string[] imageFiles = { "jpg", "bmp", "gif", "png" };

        public void OnCaptured(object sender, TemplateTriggerHookEvent e)
        {
            if (e.Complete)
            {
                return;
            }

            var file = e.TemplateManager.GetFile(e.CapturedString);
            if (file == null)
            {
                e.Complete = false;
                return;
            }

            e.EarseAmount += file.Name.Length;

            Thread.Sleep(300);
            e.TemplateManager.Earse(e.WindowId, e.EarseAmount);

            var fileName = Path.GetFileName(file.Path);
            var fileInfo = new FileInfo(file.Path);
            var path = Path.Combine(fileInfo.Directory.FullName, fileName);

            if (imageFiles.Contains(fileName.ToLower().Split(".")[1]))
            {
                Clipboard.SetImage(new BitmapImage(new Uri(path)));
            }
            else
            {
                StringCollection fileCollection = new StringCollection();
                fileCollection.Add(path);
                Clipboard.SetFileDropList(fileCollection);
            }

            e.TemplateManager.Paste();
            e.Complete = true;
        }
    }
}
