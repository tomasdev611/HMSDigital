using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.ViewModels
{
    public class RouteLookupViewModel : BaseViewModel
    {
        public RouteLookupViewModel() : base()
        {

        }

        public override void InitializeViewModel(object data = null)
        {
            SiteDetails = (SiteDetail)data;
        }

        private SiteDetail _siteDetails;

        public SiteDetail SiteDetails
        {
            get
            {
                return _siteDetails;
            }
            set
            {
                _siteDetails = value;
                OnPropertyChanged();
            }
        }
    }
}
