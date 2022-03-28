using System;
using System.Collections.Generic;
using MobileApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using MobileApp.Models;

namespace MobileApp.Pages.PopUpMenu
{
    public partial class RouteLookup : PopupPage
    {
        private BaseViewModel _viewModel;

        private SiteDetail _siteDetails;
        public RouteLookup(SiteDetail siteDetails)
        {
            _siteDetails = siteDetails;
            _viewModel = new RouteLookupViewModel();
            InitializeComponent();
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel(_siteDetails);
        }

        protected override void OnDisappearing()
        {
            _viewModel.DestroyViewModel();
        }
    }
}
