using Placehold.Keyboard.Hook;
using Placehold.Template.Events.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Placehold.Template.Events
{
    public class FileEvent : BaseEvent
    {
        private readonly TemplateManager templateManager;
        private readonly string[] imageFiles = { "jpg", "bmp", "gif", "png" };

        public FileEvent(TemplateManager templateManager) : base(templateManager) 
        {
            this.templateManager = templateManager;
        }

        public override void OnCaptured(object sender, TemplateTriggerHookEvent e)
        {
            if (e.Complete)
            {
                return;
            }

            var file = templateManager.GetFile(e.CapturedString);
            if (file == null)
            {
                e.Complete = false;
                return;
            }

            e.EarseAmount += file.Name.Length;

            Thread.Sleep(300);
            templateManager.Earse(e.WindowId, e.EarseAmount);

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

            templateManager.Paste();

            base.OnCaptured(sender, e);
        }
    }
}
