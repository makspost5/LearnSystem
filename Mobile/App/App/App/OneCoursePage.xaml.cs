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
	public partial class OneCoursePage : ContentPage
	{

        public SubjectCourseModel course { get; set; }
        
        public List<SubjectSection> subjectSections { get; set; }

        public OneCoursePage (int id)
		{
			InitializeComponent ();
            getSections(id);
		}

        private async void CV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var s = CV.SelectedItem as SubjectSection;
            CV.SelectedItem = null;
            if (s != null)
                await Navigation.PushAsync(new SubjectSectionPage(s.SubjectSectionID));
        }

        public async void getSections(int courseId)
        {
            var request = Settings.connection + "api/SubjectSectionsByCourse/"+courseId;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Settings.Token);


            HttpResponseMessage response = await client.GetAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            subjectSections = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubjectSection>>(result);

            CV.BindingContext = this;
            CV.ItemsLayout = new ListItemsLayout(ItemsLayoutOrientation.Vertical);
            CV.SetBinding(ItemsView.ItemsSourceProperty, "subjectSections");
        }
    }
}