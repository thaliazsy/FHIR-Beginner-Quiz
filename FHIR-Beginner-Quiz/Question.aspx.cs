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
        static int SIZE = 40;
        static int itemLength = 0;
        static int[] randNumbers = new int[SIZE];
        static int curIdx = 0;
        static dynamic json;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = "test01";
            if (Session["username"] == null)
            {
                Response.Redirect("http://203.64.84.150:51888/user4/course/FHIRbeginner_IT/login.html");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    LblWelcome.Text = "Hello, " + Session["username"].ToString() + "!<br><br>";

                    //START: GET Questionnaire
                    var request = (HttpWebRequest)WebRequest.Create("http://203.64.84.213:8080/fhir/Questionnaire/6902");
                    request.Accept = "application/json";
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    //END: GET Questionnaire

                    //Convert response to json
                    json = JsonConvert.DeserializeObject(responseString);

                    itemLength = json["item"].Count;

                    //START: populate questions array
                    //for (int i = 0; i < itemLength; i++)
                    //{
                    //    Response.Write(json["item"][i].text + "<br>");
                    //}
                    //END: populate questions array

                    // START: random number for questions
                    for (int i = 0; i < randNumbers.Length; i++)
                    {
                        randNumbers[i] = i + 1;
                    }
                    Random rd = new Random();
                    int rand = 0;
                    int temp;
                    for (int i = 0; i < SIZE; i++)
                    {
                        rand = rd.Next(1, SIZE - i);
                        temp = randNumbers[rand];
                        randNumbers[rand] = randNumbers[SIZE - 1 - i];
                        randNumbers[SIZE - 1 - i] = temp;
                    }
                    // END: random number for questions


                    DisplayQuestion();
                }
            }
        }

        protected void BtnNext_Click(object sender, EventArgs e)
        {
            if (curIdx < itemLength)
            {
                curIdx++;
                DisplayQuestion();
                if (curIdx == itemLength)
                {
                    BtnNext.Text = "Submit";
                }
            }
            else if (curIdx == itemLength)
            {
                //SUBMIT EXAM

            }

        }

        private void DisplayQuestion()
        {
            int itemIndex = randNumbers[curIdx] - 1;
            var qItem = json["item"][itemIndex];

            // Get question item
            LblQuestion.Text = (curIdx + 1).ToString() + ". " + qItem.text;

            // Get answer options
            string[] options = new string[qItem["answerOption"].Count];
            for (int i = 0; i < qItem["answerOption"].Count; i++)
            {
                options[i] = qItem["answerOption"][i].valueString;
            }

            // Single answer or multiple answer
            string substr = "(Multiple answer)";
            if (LblQuestion.Text.Contains(substr))
            {
                RadOptions.Visible = false;
                CheckBoxOptions.DataSource = options;
                CheckBoxOptions.DataBind();
                CheckBoxOptions.Visible = true;
            }
            else
            {
                CheckBoxOptions.Visible = false;
                RadOptions.DataSource = options;
                RadOptions.DataBind();
                RadOptions.Visible = true;
            }

        }
    }
}