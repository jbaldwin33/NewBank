using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ServerConfiguration
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      var viewModel = new MainViewModel();
      var window = new MainWindow
      {
        DataContext = viewModel
      };
      window.Show();
    }
  }
}
