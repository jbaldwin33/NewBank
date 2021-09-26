using System;
using System.IO;
using System.Xml.Serialization;

namespace ServerShared
{
  public class ConfigurationModel
  {
    private static readonly string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NewBank", "database", "ServerConfiguration.xml");

    public bool UseSqlite { get; set; }
    public bool UseSqlServer { get; set; }


    public ConfigurationModel LoadConfiguration()
    {
      var serializer = new XmlSerializer(typeof(ConfigurationModel));
      using var stream = new StreamReader(filename);
      return serializer.Deserialize(stream) as ConfigurationModel;
    }
  }
}
