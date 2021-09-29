using System.Windows.Input;

using loopaScan.ViewModels.Base;
using loopaScan.Models;
using loopaScan.Infrastructure.Commands;
using loopaScan.Views.Windows;

namespace loopaScan.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private string _ForTests;
        public string ForTests { get => _ForTests; set => Set(ref _ForTests, value); }
        #region Constructor
        public MainViewModel()
        {
            CreateSessionCommand = new RelayCommand(OnCreateSessionCommandExecuted, CanCreateSessionCommandExecute);
        }
        #endregion

        #region CurrentSession
        private Session _CurrentSession;
        public Session CurrentSession
        {
            get => _CurrentSession;
            set => Set(ref _CurrentSession, value);
        }
        #endregion

        #region Commands

        #region CreateSessionCommand
        public ICommand CreateSessionCommand { get; }
        private bool CanCreateSessionCommandExecute(object p)
        {
            return true;
        }
        private void OnCreateSessionCommandExecuted(object p)
        {
            var createSessionWindow = new CreateSessionWindow();
            if (createSessionWindow.ShowDialog() == true)
            {
                Session session = new Session
                {
                    //Name = createSessionWindow.SessionName,
                    //FileName = createSessionWindow.SessionFileName
                };
            }
        }
        #endregion

        #endregion
    }
}
