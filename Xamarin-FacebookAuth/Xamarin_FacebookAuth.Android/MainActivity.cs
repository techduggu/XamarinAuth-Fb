using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Xamarin_FacebookAuth.Authentication;
using Xamarin_FacebookAuth.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Xamarin_FacebookAuth.Droid
{
    [Activity (Label = "Xamarin_FacebookAuth.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IFacebookAuthenticationDelegate
	{
        private FacebookAuthenticator _auth;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _auth = new FacebookAuthenticator(Configuration.ClientId, Configuration.Scope, this);

            var facebookLoginButton = FindViewById<Button>(Resource.Id.facebookLoginButton);
            facebookLoginButton.Click += OnFacebookLoginButtonClicked;

            AppCenter.Start("f0e84c43-eb80-41fe-83af-b86ac30313e5", typeof(Analytics), typeof(Crashes));
        }

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            // Display the activity handling the authentication
            var authenticator = _auth.GetAuthenticator();
            var intent = authenticator.GetUI(this);
            StartActivity(intent);
        }

        public async void OnAuthenticationCompleted(FacebookOAuthToken token)
        {
            // Retrieve the user's email address
            var facebookService = new FacebookService();
            var email = await facebookService.GetEmailAsync(token.AccessToken);

            // Display it on the UI
            var facebookButton = FindViewById<Button>(Resource.Id.facebookLoginButton);
            facebookButton.Text = $"Connected with {email}";
        }

        public void OnAuthenticationCanceled()
        {
            new AlertDialog.Builder(this)
                           .SetTitle("Authentication canceled")
                           .SetMessage("You didn't completed the authentication process")
                           .Show();
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new AlertDialog.Builder(this)
                           .SetTitle(message)
                           .SetMessage(exception?.ToString())
                           .Show();
        }
    }
}


