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
	public partial class SubjectSectionPage : ContentPage
	{
        public List<SectionBlockModel> sectionBlocks { get; set; }
        public string Count { get; set; }

        public SubjectSectionPage (int id)
		{
			InitializeComponent ();
            getSectionBlocks(id);
		}

        private async void getSectionBlocks(int id)
        {
            var request = Settings.connection + "api/SectionBlocks/" + id;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Settings.Token);


            HttpResponseMessage response = await client.GetAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            sectionBlocks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SectionBlockModel>>(result);
            Count = sectionBlocks.Count.ToString();

            CV.BindingContext = this;
            CV.ItemsLayout = new ListItemsLayout(ItemsLayoutOrientation.Vertical);
            CV.SetBinding(ItemsView.ItemsSourceProperty, "sectionBlocks");

            foreach (SectionBlockModel sbm in sectionBlocks)
            {

                sbm.Position += 1;
                if (sbm.isPassed) sbm.Color = "LightGreen"; else sbm.Color = "LightGray";
            }
        }

        private async void CV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = CV.SelectedItem as SectionBlockModel;
            CV.SelectedItem = null;
            if (c != null)
                await Navigation.PushAsync(new OneBlockPage(c.SectionBlockID));
        }
    }
}