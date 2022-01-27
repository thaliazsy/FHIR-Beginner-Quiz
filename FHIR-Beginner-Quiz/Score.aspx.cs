using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace FHIR_Beginner_Quiz
{
    public partial class Score : System.Web.UI.Page
    {
        static readonly int SIZE = 40;
        static int itemLength = 0;
        static readonly int[] randNumbers = new int[SIZE];
        static double score = 0;
        static dynamic json;
        static dynamic answerkey;
        static readonly QuestionnaireResponse resp = new QuestionnaireResponse();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["quesRespId"] = 6916;

            if (Session["username"] == null || Session["quesRespId"] == null)
            {
                Response.Redirect("http://203.64.84.150:51888/user4/course/FHIRbeginner_IT/login.html");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    LblWelcome.Text = "Hello, " + Session["name"].ToString() + "!<br>" + Session["quesRespId"].ToString() + "<br>";

                    // START: GET QuestionnaireResponse
                    var request = (HttpWebRequest)WebRequest.Create("http://203.64.84.213:8080/fhir/QuestionnaireResponse/" + Session["quesRespId"].ToString());
                    request.Accept = "application/json";

                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    // Convert response to json
                    json = JsonConvert.DeserializeObject(responseString);
                    // END: GET QuestionnaireResponse

                    // START: GET Questionnaire Answer Key
                    request = (HttpWebRequest)WebRequest.Create("http://203.64.84.213:8080/fhir/Questionnaire/6915");
                    request.Accept = "application/json";
                    response = (HttpWebResponse)request.GetResponse();
                    responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    // Convert response to json
                    answerkey = JsonConvert.DeserializeObject(responseString);
                    // END: GET Questionnaire Answer Key

                    itemLength = json["item"].Count;

                    for (int i = 0; i < itemLength; i++)
                    {
                        string myItem = json["item"][i]["answer"][0]["valueString"];
                        string keyItem = answerkey["item"][i]["answerOption"][0]["valueString"];

                        if (myItem == keyItem)
                        {
                            //Response.Write((i + 1) + " correct<br>");
                            score++;
                        }
                        else
                        {
                            //Response.Write((i + 1) + "wrong<br>");
                        }

                        //Response.Write("keyItem: " + keyItem + "<br>");
                        //Response.Write("myItem: " + myItem + "<br><br>");
                    }

                    score = score / (double) 40 * 100;
                    LblScore.Text = score.ToString();
                }
            }
        }
    }
}