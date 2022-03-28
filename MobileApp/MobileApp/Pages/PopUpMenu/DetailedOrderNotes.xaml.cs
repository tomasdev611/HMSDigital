using System.Collections.Generic;
using MobileApp.Models;
using MobileApp.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace MobileApp.Pages.PopUpMenu
{
    public partial class DetailedOrderNotes : PopupPage
    {
        private readonly BaseViewModel _viewModel;

        private List<NoteResponse> _notes;

        public DetailedOrderNotes(List<NoteResponse> notes)
        {
            _notes = notes;
            _viewModel = new DetailedOrderNotesViewModel();
            this.BindingContext = _viewModel;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel(_notes);
        }
    }
}