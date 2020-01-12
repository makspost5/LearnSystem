using App.Helpers;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CoursesPage : ContentPage
	{
        public List<SubjectCourseModel> Courses { get; set; }

        public CoursesPage ()
		{
			InitializeComponent ();
            getPosts();
		}

        private async void CV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = CV.SelectedItem as SubjectCourseModel;
            CV.SelectedItem = null;
            if (c != null)
                await Navigation.PushAsync(new OneCoursePage(c.Id));
        }

        public async void getPosts()
        {
            var request = Settings.connection + "api/Pupil/Mobile/SubjectCourses";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Settings.Token);


            HttpResponseMessage response = await client.GetAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            Courses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubjectCourseModel>>(result);

            CV.BindingContext = this;
            CV.ItemsLayout = new ListItemsLayout(ItemsLayoutOrientation.Vertical);
            CV.SetBinding(ItemsView.ItemsSourceProperty, "Courses");
        }
    }
}