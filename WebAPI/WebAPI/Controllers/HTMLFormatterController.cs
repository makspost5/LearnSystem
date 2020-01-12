using System;
using System.IO;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class HTMLFormatterController : ApiController
    {
        [Route("HTMLFormatter")]
        [HttpPost]
        [Authorize]
        public string GetFormattedHTML([FromBody] CustomModel model)
        {
            return FormatHtml(model.html);
        }

        public class CustomModel
        {
            public string html { get; set; }
        }

        private static string FormatHtml(string content)
        {
            string original = content;                                                              //1

            int tab = 0;
            int adjustment = 0;

            int openIndex = 0;
            int closeIndex = 0;
            int slashIndex = 0;
            int n = 0;
            int b = 0;
            int e = 0;

            string openChar = "<";
            string slashChar = "/";
            string closeChar = ">";

            string snippet;

            try
            {
                using (StringWriter writer = new StringWriter())
                {
                    while (b > -1 && n > -1)                                                        //2
                    {
                        openIndex = content.IndexOf(openChar, n);
                        slashIndex = content.IndexOf(slashChar, n);
                        closeIndex = content.IndexOf(closeChar, n);
                        adjustment = 0;

                        b = n;
                        if (openIndex > -1 && openIndex < closeIndex && openIndex == n)             //3
                        {
                            e = closeIndex;                                                         //4
                            adjustment = 2;
                        }
                        else
                        {
                            e = openIndex - 1;                                                      //5
                        }

                        if (b == openIndex && b + 1 == slashIndex)                                  //6
                        {
                            tab -= 2;                                                               //7
                            adjustment = 0;
                        }

                        if ((slashIndex + 1) == closeIndex && closeIndex == e)                      //8
                        {
                            adjustment = 0;                                                         //9
                        }
                        
                        int length = (e - b + 1);
                        if (length < 0)                                                             //10
                        {
                            snippet = content.Substring(b);                                         //11
                        }
                        else
                        {
                            snippet = content.Substring(b, (e - b + 1));                            //12
                        }
                        
                        if (snippet == "<br>" || snippet == "<hr>")                                 //13
                        {
                            adjustment = 0;                                                         //14
                        }
                        
                        if (!string.IsNullOrEmpty(snippet.Trim()))                                  //15
                        {
                            writer.Write(Environment.NewLine);                                      //16
                            if (tab > 0)                                                            //17
                            {
                                writer.Write(new String('\t', tab / 2));                            //18
                            }

                            writer.Write(snippet);                                                  //19
                        }

                        tab += adjustment;

                        n = e + 1;

                    }

                    return writer.ToString();                                                       //20
                }
            }
            catch
            {
                return original;
            }
        }
    }
}
