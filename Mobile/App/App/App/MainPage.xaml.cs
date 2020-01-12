using App.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using Pets.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
           // CheckSigned();
        }

        private async void CheckSigned()
        {
            if (Settings.UserName != "" && Settings.Token != "")
            {
                await Navigation.PushAsync(new CoursesPage());
            }
        }

        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            var client = new HttpClient();

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
            nvc.Add(new KeyValuePair<string, string>("password", SignInPassword.Text));
            nvc.Add(new KeyValuePair<string, string>("username", SignInEmail.Text));

            var content = new FormUrlEncodedContent(nvc);

            HttpResponseMessage response = await client.PostAsync(Settings.connection + "token", content);


            var result = await response.Content.ReadAsStringAsync();

            var obj = JObject.Parse(result);

            Settings.Token = obj["access_token"].ToString();
            Settings.UserName = obj["userName"].ToString();
            Settings.Password = SignInPassword.Text;

            Application.Current.Properties["token"] = obj["access_token"];

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
               // await DisplayAlert("Результат", "вход выполнен успешно!", "OK");

                await Navigation.PushAsync(new CoursesPage()); // - здесь будет переход на страницу курсов
            } else await DisplayAlert("Результат", result, "OK");

        }

        private void SignInSwitchShowPassword_Toggled(object sender, ToggledEventArgs e)
        {
            SignInPassword.IsPassword = !SignInPassword.IsPassword;
        }
    }
}
