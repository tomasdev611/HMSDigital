using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Interface
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task NavigateToAsync<TView>() where TView : Page;
        Task NavigateToAsync<TView>(object parameter) where TView : Page;
        Task NavigateToAsync(Type viewType);
        Task NavigateToAsync(Type viewType, object parameter);
        Task ChangeDetailPageAsync<TView>() where TView : Page;
        Task ChangeDetailPageAsync<TView>(object parameter) where TView : Page;
        Task ChangeDetailPageAsync(Type viewType);
        Task ChangeDetailPageAsync(Type viewType, object parameter);
        Task PushPopupAsync<TView>() where TView : Page;
        Task PushPopupAsync<TView>(object parameter) where TView : Page;
        Task PushPopupAsync(Type viewType);
        Task PushPopupAsync(Type viewType, object parameter);
        Task PushModalAsync<TView>() where TView : Page;
        Task PushModalAsync<TView>(object parameter) where TView : Page;
        Task PushModalAsync(Type viewType);
        Task PushModalAsync(Type viewType, object parameter);
        Task PopModalAsync();
        Task NavigateBackAsync();
        Task PopPopupAsync();
        Task ClearBackStack();
        Task RemoveLastFromBackStackAsync();
        Task PopToRootAsync();
        Page GetMainPage();
    }
}