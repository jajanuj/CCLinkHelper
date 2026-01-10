using System;
using System.IO;
using System.Xml.Serialization;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Services
{
   public static class SettingsPersistence
   {
      #region Public Methods

      public static ControllerSettings Load(string name = null)
      {
         string target = GetPath(name);
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

      public static void Save(ControllerSettings settings, string name = null)
      {
         string target = GetPath(name);
         var serializer = new XmlSerializer(typeof(ControllerSettings));
         using (var sw = new StreamWriter(target))
         {
            serializer.Serialize(sw, settings);
         }
      }

      public static bool Exists(string name = null)
      {
         return File.Exists(GetPath(name));
      }

      #endregion

      #region Private Methods

      private static string GetPath(string name)
      {
         if (string.IsNullOrEmpty(name))
         {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");
         }

         if (!name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
         {
            name += ".xml";
         }

         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
      }

      #endregion
   }
}