using System;

namespace BankServer
{
  protected DefaultTextBox()
  {
    var style = new Style(typeof(TextBox)) { BasedOn = FindResource("MahApps.Styles.TextBox") as Style };
    style.Setters.Add(new Setter(PaddingProperty, new Thickness(0)));
    style.Setters.Add(new Setter(MarginProperty, new Thickness(0)));
    Style = style;
    HorizontalAlignment = HorizontalAlignment.Left;
    VerticalAlignment = VerticalAlignment.Center;
    Padding = new Thickness(5, 5, 0, 0);
    Height = StyleConstants.Heights.TextBox;
  }

  public DefaultTextBlock()
  {
    HorizontalAlignment = HorizontalAlignment.Left;
    Margin = new Thickness(0, 0, 0, 10);
    FontSize = 15;
  }

  static DefaultTextBlock()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(DefaultTextBlock), new FrameworkPropertyMetadata(typeof(DefaultTextBlock)));
  }


  public ListViewAddButton()
  {
    Style = FindResource("MahApps.Styles.Button.Flat") as Style;
    HorizontalAlignment = HorizontalAlignment.Center;
    VerticalAlignment = VerticalAlignment.Center;
    Padding = new Thickness(0);
    Margin = new Thickness(0);
    MaxHeight = 40;
    MinHeight = 40;
    MaxWidth = 40;
    MinWidth = 40;
    Height = 40;
    Width = 40;
    Content = new PackIconMaterial { Kind = PackIconMaterialKind.Plus, Height = 16, Width = 16 };
  }

  public const int ExtraSmall = 60;
  public const int Small = 125;
  public const int Medium = 375;
  public const int Large = 500;
  public const int ExtraLarge = 625;

  public ToggleSwitch()
  {
    Style = FindResource("MahApps.Styles.ToggleSwitch.Win10") as Style;
  }

  //[ServiceContract(Name = "IRemotingService")]
  //public interface IRemotingService
  //{
  //  [OperationContract(Name = "GetUser")]
  //  User GetUser(Guid id);
  //  [OperationContract(Name = "GetUsers")]
  //  User[] GetUsers(Guid[] id);
  //  [OperationContract(Name = "GetAccount")]
  //  Account GetAccount(Guid id);
  //  [OperationContract(Name = "GetAccounts")]
  //  Account[] GetAccounts(Guid[] id);
  //}
}
