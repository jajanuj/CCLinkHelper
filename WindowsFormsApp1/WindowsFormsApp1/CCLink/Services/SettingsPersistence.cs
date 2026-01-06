using System;
using System.IO;
using System.Xml.Serialization;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Services
{
   public static class SettingsPersistence
   {
      private static readonly string DefaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");

      public static ControllerSettings Load(string path = null)
      {
         string target = path ?? DefaultPath;
         if (!File.Exists(target))
         {
            return null;
         }

         try
         {
            var serializer = new XmlSerializer(typeof(ControllerSettings));
            using (var fs = new FileStream(target, FileMode.Open))
            {
               return (ControllerSettings)serializer.Deserialize(fs);
            }
         }
         catch
         {
            return null;
         }
      }

      public static void Save(ControllerSettings settings, string path = null)
      {
         string target = path ?? DefaultPath;
         var serializer = new XmlSerializer(typeof(ControllerSettings));
         using (var sw = new StreamWriter(target))
         {
            serializer.Serialize(sw, settings);
         }
      }

      public static bool Exists(string path = null)
      {
         return File.Exists(path ?? DefaultPath);
      }
   }
}
