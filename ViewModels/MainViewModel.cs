using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using loopaScan.ViewModels.Base;
using loopaScan.Models;
using loopaScan.Infrastructure.Commands;
using loopaScan.Views.Windows;
using loopaScan.Infrastructure;

namespace loopaScan.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        #region Constructor
        public MainViewModel()
        {
            Directories.CreateDirectoriesOnStartup();

            SessionController = new SessionController();
            AllSessions = SessionController.GetSessions();
            Scaner = new Scaner();

            StartScanCountUpdater();

            CreateSessionCommand = new RelayCommand(OnCreateSessionCommandExecuted, CanCreateSessionCommandExecute);
            DeleteSessionCommand = new RelayCommand(OnDeleteSessionCommandExecuted, CanDeleteSessionCommandExecute);
            WindowCloseCommand = new RelayCommand(OnWindowCloseCommandExecuted);
            RunScanCommand = new RelayCommand(OnRunScanCommandExecuted, CanRunScanCommandExecute);
            StopScanCommand = new RelayCommand(OnStopScanCommandExecuted, CanStopScanCommandExecute);
        }
        #endregion

        #region Models
        private readonly SessionController SessionController;
        public Scaner Scaner;
        #endregion

        #region StatusIsVisible
        private string _StatusIsVisible = "Collapsed";
        public string StatusIsVisible
        {
            get => _StatusIsVisible;
            set => Set(ref _StatusIsVisible, value);
        }
        #endregion

        #region CanChangeSession
        private bool _CanChangeSession = true;
        public bool CanChangeSession
        {
            get => _CanChangeSession;
            set => Set(ref _CanChangeSession, value);
        }
        #endregion

        #region CurrentSession
        private Session _CurrentSession;
        public Session CurrentSession
        {
            get => _CurrentSession;
            set
            {
                Set(ref _CurrentSession, value);
                if (value != null) StatusIsVisible = "Visible";
                else StatusIsVisible = "Collapsed";
            }
        }
        #endregion

        #region AllSessions
        private ObservableCollection<Session> _AllSessions;
        public ObservableCollection<Session> AllSessions
        {
            get => _AllSessions;
            set => Set(ref _AllSessions, value);
        }
        #endregion


        #region Commands

        #region CreateSessionCommand
        public ICommand CreateSessionCommand { get; }
        private bool CanCreateSessionCommandExecute(object p) => !Scaner.IsScanning;
        private void OnCreateSessionCommandExecuted(object p)
        {
            CreateSessionWindow createSessionWindow = new CreateSessionWindow();
            if (createSessionWindow.ShowDialog() == true)
            {
                Session session = createSessionWindow.Session;
                CurrentSession = session;
                AllSessions.Add(CurrentSession);
                CurrentSession.Save();
            }
        }
        #endregion

        #region DeleteSessionCommand
        public ICommand DeleteSessionCommand { get; }
        private bool CanDeleteSessionCommandExecute(object p) => p is Session session && AllSessions.Contains(session);
        private void OnDeleteSessionCommandExecuted(object p)
        {
            if (!(p is Session session)) return;
            if (session.Delete())
            {
                AllSessions.Remove(session);
            }
        }
        #endregion

        #region WindowClosecommand
        public ICommand WindowCloseCommand { get; }
        private void OnWindowCloseCommandExecuted(object p)
        {
            SessionController.SaveSessions(AllSessions);
        }
        #endregion

        #region Scaner

        #region RunScanCommand
        public ICommand RunScanCommand { get; }
        private bool CanRunScanCommandExecute(object p) => Scaner != null && !Scaner.IsScanning && CurrentSession != null;
        private void OnRunScanCommandExecuted(object p)
        {
            Scaner = new Scaner();
            Scaner.LoadSession(CurrentSession);
            Scaner.RunScan();
            if (Scaner.IsScanning) CanChangeSession = false;
        }
        #endregion

        #region StopScanCommand
        public ICommand StopScanCommand { get; }
        private bool CanStopScanCommandExecute(object p) => Scaner != null && Scaner.IsScanning;
        private void OnStopScanCommandExecuted(object p)
        {
            Scaner.StopScan();
            CanChangeSession = true;
        }
        #endregion

        #endregion


        #endregion

        #region StartScanCountUpdater
        private double _ScanProgressBar;
        public double ScanProgressBar
        {
            get => _ScanProgressBar;
            set => Set(ref _ScanProgressBar, value);
        }
        private async void StartScanCountUpdater()
        {
            while (true)
            {
                if (CurrentSession != null)
                {
                    ScanProgressBar = (double)CurrentSession.ScannedIPsCount / (double)CurrentSession.IPsCount * 100;
                    OnPropertyChanged("CurrentSession");
                    OnPropertyChanged("ScanProgressBar");

                    if (CurrentSession.ScannedIPsCount == CurrentSession.IPsCount)
                        OnStopScanCommandExecuted(null);
                }
                await Task.Delay(250);
            }
        }
        #endregion


    }
}
