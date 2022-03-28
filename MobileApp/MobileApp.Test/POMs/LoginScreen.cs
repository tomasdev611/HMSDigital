using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MobileApp.Test.POMs
{
    public class LoginScreen : BasePage
    {
        private readonly Query _usernameField;
        private readonly Query _passwordField;
        private readonly Query _logInButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            iOS = x => x.Id("hmstest-login-image_sign_in")
        };

        public LoginScreen()
        {
            if (OniOS)
            {
                _logInButton = x => x.Marked("hmstest-login-btn_login");
                _usernameField = x => x.Marked("hmstest-login-entry_username");
                _passwordField = x => x.Marked("hmstest-login-entry_password");
            }
        }

        internal LoginScreen EnterCredentials(string username, string password)
        {
            app.Tap(_usernameField);
            app.EnterText(username);

            app.Tap(_passwordField);
            app.EnterText(password);

            return this;
        }

        internal LoginScreen ConfirmLogIn()
        {
            app.DismissKeyboard();
            app.Tap(_logInButton);
            return this;
        }
    }
}
