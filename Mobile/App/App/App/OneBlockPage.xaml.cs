using App.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FormsRadioButton;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Models
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OneBlockPage : CarouselPage
    {
        List<ContentPage> pages = new List<ContentPage>(0);

        List<QuestionMobileModel> questions = new List<QuestionMobileModel>();

        public OneBlockPage(int id)
        {
            InitializeComponent();
            getPages(id);
        }

        private async void getPages(int id)
        {
            var request = Settings.connection + "api/QuestionsBySectionBlockId/Mobile/" + id;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Settings.Token);


            HttpResponseMessage response = await client.GetAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            questions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionMobileModel>>(result);

            foreach (var q in questions)
            {
                var qrequest = Settings.connection + "api/TheoryBody/" + q.QuestionID;
                HttpResponseMessage qresponse = await client.GetAsync(qrequest);

                var qresult = await qresponse.Content.ReadAsStringAsync();

                var t = Newtonsoft.Json.JsonConvert.DeserializeObject<Theory>(qresult);

                q.TheoryBody = t.body;
            }

            //CV.BindingContext = this;
            //CV.ItemsLayout = new ListItemsLayout(ItemsLayoutOrientation.Vertical);
            //CV.SetBinding(ItemsView.ItemsSourceProperty, "sectionBlocks");

            foreach (QuestionMobileModel q in questions)
            {
                var webView = new WebView();
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = q.TheoryBody;
                webView.HorizontalOptions = LayoutOptions.FillAndExpand;
                webView.VerticalOptions = LayoutOptions.FillAndExpand;
                webView.Source = htmlSource;

                var questionLabel = new Label();
                questionLabel.Text = q.Body;

                var radio = new RadioItems();


                for (int i = 0; i < q.Answer.Count(); i++)
                radio.Add(new RadioItem { Text = q.Answer[0].ToString(), Toggled = false });


                var stack = new StackLayout();
                stack.Children.Add(questionLabel);


                webView.IsVisible = true;
                pages.Add(new ContentPage
                {
                    Content = webView,
                    Padding = 0
                });

                //pages.Add(new ContentPage
                //{
                //    Content = questionLabel,
                //    Padding = 0
                //});


            }

            foreach (var p in pages)
            {
                p.IsVisible = true;
                cPCarouselPage.Children.Add(p);
            }

        }

    }
}