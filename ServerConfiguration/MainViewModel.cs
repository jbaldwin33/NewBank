using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServerShared;
using MVVMFramework.Localization;

namespace ServerConfiguration
{
  public class MainViewModel : ViewModelBase
  {
    private static readonly string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NewBank", "database", "ServerConfiguration.xml");
    private bool useSqlite;
    private bool useSqlServer;
    private RelayCommand saveCommand;

    public MainViewModel()
    {
      if (File.Exists(filename))
        LoadFromFile();
    }

    private void LoadFromFile()
    {
      var serializer = new XmlSerializer(typeof(ConfigurationModel));
      using var stream = new StreamReader(filename);
      var model = serializer.Deserialize(stream) as ConfigurationModel;
      useSqlite = model.UseSqlite;
      useSqlServer = model.UseSqlServer;
    }

    public string SaveLabel => "Save";

    public bool UseSqlite
    {
      get => useSqlite;
      set => Set(ref useSqlite, value);
    }

    public bool UseSqlServer
    {
      get => useSqlServer;
      set => Set(ref useSqlServer, value);
    }

    public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);

    private void SaveCommandExecute()
    {
      var serializer = new XmlSerializer(typeof(ConfigurationModel));
      Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NewBank", "database"));
      var stream = File.Create(filename);
      var model = new ConfigurationModel
      {
        UseSqlite = useSqlite,
        UseSqlServer = useSqlServer
      };
      serializer.Serialize(stream, model);
      MessageBox.Show("Changes saved", new InformationTranslatable(), MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private bool SaveCommandCanExecute() => true;
  }
}
