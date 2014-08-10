using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using MediaBrowser.Library.Configuration;
using MediaBrowser.Library.Logging;

namespace Xenon.Fonts
{
    public class FontManager
    {
        private readonly string _fontFilePath = Path.Combine(ApplicationPaths.AppPluginPath, @"Xenon\TTFFonts");
        //When changing these dont forget to change the resource file down below.
        private const string Font1 = "SegeoWP.TTF";
        private const string Font2 = "SegeoWP-Black.TTF";
        private const string Font3 = "SegeoWP-Light.TTF";
        private const string Font4 = "Urban Elegance.TTF";
        private const string Font5 = "InterOne.TTF";
        private const string Font6 = "InterBold.TTF";
        private const string Font7 = "impact.TTF";
        private const string Font8 = "BebasNeue.TTF";

        private bool AreFontsAccessible()
        {
            string f1Path = Path.Combine(_fontFilePath, Font1);
            string f2Path = Path.Combine(_fontFilePath, Font2);
            string f3Path = Path.Combine(_fontFilePath, Font3);
            string f4Path = Path.Combine(_fontFilePath, Font4);
            string f5Path = Path.Combine(_fontFilePath, Font5);
            string f6Path = Path.Combine(_fontFilePath, Font6);
            string f7Path = Path.Combine(_fontFilePath, Font7);
            string f8path = Path.Combine(_fontFilePath, Font8);
            if ((File.Exists(f1Path) && File.Exists(f2Path)) && (File.Exists(f3Path) && File.Exists(f4Path)) && (File.Exists(f5Path) && File.Exists(f6Path)) && (File.Exists(f7Path) && File.Exists(f8path)))
            {
                Logger.ReportInfo("The Required Xenon Fonts are installed.");
                return true;
            }
            Logger.ReportError("The Required Xenon Fonts are not installed / not accessible.");
            return false;
        }

        private static void ExtractResource(string filename, Stream resourceFile)
        {
            using (new BinaryReader(resourceFile))
            {
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    byte[] buffer = new byte[resourceFile.Length];
                    resourceFile.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, buffer.Length);
                    Logger.ReportInfo("  Resource extracted: " + filename);
                }
            }
        }

        public PrivateFontCollection FontCollection()
        {
            Logger.ReportInfo("Check if default fonts are installed...");
            if (!this.AreFontsAccessible())
            {
                this.InstallFonts();
            }
            PrivateFontCollection fonts = new PrivateFontCollection();
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font1));
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font2));
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font3));
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font4));
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font5));
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font6));
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font7));
            fonts.AddFontFile(Path.Combine(_fontFilePath, Font8));
            return fonts;
        }

        private void InstallFonts()
        {
            try
            {
               string path = _fontFilePath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fontInstall = Path.Combine(path, Font1);
                if (!File.Exists(fontInstall))
                {
                    using (Stream stream = new MemoryStream(Resources.SegoeWP))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, stream);
                    }
                }
                fontInstall = Path.Combine(path, Font2);
                if (!File.Exists(fontInstall))
                {
                    using (Stream fontStream = new MemoryStream(Resources.SegoeWP_Black))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, fontStream);
                    }
                }
                fontInstall = Path.Combine(path, Font3);
                if (!File.Exists(fontInstall))
                {
                    using (Stream fontStream = new MemoryStream(Resources.SegoeWP_Light))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, fontStream);
                    }
                }
                fontInstall = Path.Combine(path, Font4);
                if (!File.Exists(fontInstall))
                {
                    using (Stream fontStream = new MemoryStream(Resources.Urban_Elegance))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, fontStream);
                    }
                }

                fontInstall = Path.Combine(path, Font5);
                if (!File.Exists(fontInstall))
                {
                    using (Stream fontStream = new MemoryStream(Resources.InterOne))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, fontStream);
                    }
                }

                fontInstall = Path.Combine(path, Font6);
                if (!File.Exists(fontInstall))
                {
                    using (Stream fontStream = new MemoryStream(Resources.InterBold))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, fontStream);
                    }
                }

                fontInstall = Path.Combine(path, Font7);
                if (!File.Exists(fontInstall))
                {
                    using (Stream fontStream = new MemoryStream(Resources.impact))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, fontStream);
                    }
                }

                fontInstall = Path.Combine(path, Font8);
                if (!File.Exists(fontInstall))
                {
                    using (Stream fontStream = new MemoryStream(Resources.BebasNeue))
                    {
                        Logger.ReportInfo("Extracting resource... ");
                        ExtractResource(fontInstall, fontStream);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.ReportException("Cannot extract necessary resources", exception);
            }
        }
    }
}
