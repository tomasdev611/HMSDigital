using System.Collections.Generic;
using System.Windows.Input;
using MobileApp.Models;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class DetailedOrderNotesViewModel : BaseViewModel
    {
        public DetailedOrderNotesViewModel() : base()
        {

        }

        private List<NoteResponse> _notes;

        public List<NoteResponse> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        public override void InitializeViewModel(object data = null)
        {
            Notes = data as List<NoteResponse>;
        }

        private ICommand _proceedOrderFullfillCommand;

        public ICommand ProceedOrderFullfillCommand
        {
            get
            {
                return _proceedOrderFullfillCommand ?? (_proceedOrderFullfillCommand = new Command(NavigateBackForOrderFullfillment));
            }
        }

        public void NavigateBackForOrderFullfillment()
        {
            _navigationService.PopPopupAsync();
        }
    }
}