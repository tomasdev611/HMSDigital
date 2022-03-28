using Microsoft.AppCenter.Crashes;
using MobileApp.Assets;
using MobileApp.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Private Properties

        private string _pageTitle = "";

        #endregion

        #region Public Properties

        public event PropertyChangedEventHandler PropertyChanged;

        public object NavigationBackParameter { get; set; }

        /// <summary>
        /// Page Title for the View
        /// </summary>
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Protected Properties

        protected INavigationService _navigationService;

        protected IToastMessage _toastMessageService;

        #endregion

        protected BaseViewModel()
        {
            _navigationService = App.NavigationService;
            _toastMessageService = DependencyService.Get<IToastMessage>();
        }

        #region Public Methods

        /// <summary>
        /// Initialize the ViewModel
        /// </summary>
        /// <param name="data"></param>
        public virtual void InitializeViewModel(object data = null)
        {

        }

        /// <summary>
        /// Destroy the current ViewModel
        /// </summary>
        public virtual void DestroyViewModel()
        {

        }


        /// <summary>
        /// Add Back Parameters
        /// </summary>
        /// <param name="obj"></param>
        public virtual void AddParameters(object obj)
        {
            NavigationBackParameter = obj;
        }

        /// <summary>
        /// Report Crash to AppCenter
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="properties"></param>
        public void ReportCrash(Exception exception, Dictionary<string, string> properties = null)
        {
            if(properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add("Environment", AppConfiguration.Environment);

            Crashes.TrackError(exception, properties);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// MVVM Property Changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
