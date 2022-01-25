using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
namespace FHIR_Beginner_Quiz
{
    //public class WeatherForecast
    //{
    //    public DateTimeOffset Date { get; set; }
    //    public int TemperatureCelsius { get; set; }
    //    public string Summary { get; set; }
    //    public string SummaryField;
    //    public string[] SummaryWords { get; set; }
    //}
    public partial class Question : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = "test01";
            if (Session["username"] == null)
            {
                Response.Redirect("http://203.64.84.150:51888/user4/course/FHIRbeginner_IT/login.html");
            }
            else
            {
                LblWelcome.Text ="Hello, " + Session["username"].ToString() +"!";
                //get Questionnaire
                var request = (HttpWebRequest)WebRequest.Create("http://203.64.84.213:8080/fhir/Questionnaire/6902");
                request.Accept = "application/json";
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //Convert response to json
                dynamic json = JsonConvert.DeserializeObject(responseString);

                var itemLength = json["item"].Count;
                LblWelcome.Text = json.resourceType;
            }
        }
    }
}