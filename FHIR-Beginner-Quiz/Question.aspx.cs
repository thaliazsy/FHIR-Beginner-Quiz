using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;

namespace FHIR_Beginner_Quiz
{

    public class FHIRAnswer
    {
        public string valueString { get; set; }
    }

    public class Item
    {
        public string linkId { get; set; }
        public string text { get; set; }
        public List<FHIRAnswer> answer { get; set; }
    }

    public class Reference
    {
        public string reference { get; set; }
        public string type { get; set; }
        public string display { get; set; }
    }

    public class QuestionnaireResponse
    {
        public QuestionnaireResponse()
        {

        }

        public const string resourceType = "QuestionnaireResponse";

        public const string questionnaire = "http://203.64.84.213:8080/fhir/Questionnaire/6902";
        public Reference subject { get; set; }
        public string authored { get; set; }
        public List<Item> item { get; set; }
    }

    public partial class Question : System.Web.UI.Page
    {
        static int SIZE = 40;
        static int itemLength = 0;
        static int[] randNumbers = new int[SIZE];
        static int curIdx = 0;
        static dynamic json;
        static string[] answers = new string[SIZE];
        static QuestionnaireResponse resp = new QuestionnaireResponse();
        static List<Item> listItem = new List<Item>(SIZE);
        static Item[] listIt = new Item[SIZE];

        protected void CreateQuestionnairerResponse()
        {
            Reference subject = new Reference()
            {
                reference = "Patient/" + Session["id"].ToString(),
                type = "Patient",
                display = Session["name"].ToString()

            };
            resp.subject = subject;

            resp.authored = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

            for (int i = 0; i < answers.Length; i++)
            {
                Item item = new Item();
                item.linkId = (i + 1).ToString();
                item.text = json["item"][i].text;

                // Check multiple answer
                if (answers[i].StartsWith("Multiple"))
                {
                    string[] ans = answers[i].Split('#');
                    for (int j = 1; j < ans.Length; j++)
                    {
                        FHIRAnswer a = new FHIRAnswer();
                        a.valueString = ans[i].Substring(1);
                        item.answer.Add(a);
                    }
                }
                else
                {
                    FHIRAnswer a = new FHIRAnswer();
                    a.valueString = answers[i];
                    item.answer.Add(a);
                }
            }

            // Create JSON Object
            string respJSON = JsonConvert.SerializeObject(resp);
            Response.Write(respJSON);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["id"] = "5674";
            Session["name"] = "test105";
            Session["username"] = "test105@gmail.com";


            if (Session["username"] == null)
            {
                Response.Redirect("http://203.64.84.150:51888/user4/course/FHIRbeginner_IT/login.html");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    LblWelcome.Text = "Hello, " + Session["name"].ToString() + "!<br><br>";

                    // START: GET Questionnaire
                    var request = (HttpWebRequest)WebRequest.Create("http://203.64.84.213:8080/fhir/Questionnaire/6902");
                    request.Accept = "application/json";
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    // END: GET Questionnaire

                    // Convert response to json
                    json = JsonConvert.DeserializeObject(responseString);

                    itemLength = json["item"].Count;

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
            if (curIdx == itemLength - 1 && BtnNext.Text == "Submit")
            {
                PanelQA.Visible = false;
                // SUBMIT EXAM
                for (int i = 0; i < itemLength; i++)
                {
                    Response.Write((i + 1) + ". " + answers[i] + "<br>");
                }
                CreateQuestionnairerResponse();
            }
            else
            {
                int itemIndex = randNumbers[curIdx] - 1;
                var qItem = json["item"][itemIndex];

                Item item = new Item();
                item.linkId = (itemIndex + 1).ToString();
                item.text = qItem.text;

                // START: Get student answer
                string substr = "(Multiple answer)";
                if (LblQuestion.Text.Contains(substr))
                {
                    List<FHIRAnswer> list = new List<FHIRAnswer>();
                    foreach (ListItem i in CheckBoxOptions.Items)
                    {
                        if (i.Selected)
                        {
                            FHIRAnswer a = new FHIRAnswer();
                            a.valueString = i.Text;
                            list.Add(a);
                        }
                    }
                    item.answer = list;
                }
                else
                {
                    List<FHIRAnswer> list = new List<FHIRAnswer>();
                    FHIRAnswer a = new FHIRAnswer();
                    if (RadOptions.SelectedItem != null)
                    {

                        a.valueString = RadOptions.SelectedItem.Text;
                    }
                    else
                    {
                        a.valueString = "----------------------------------";
                    }
                    list.Add(a);
                    item.answer = list;
                }
                // END: Get student answer
                listIt[itemIndex] = item;
                //listItem.Insert(itemIndex, item);

                // To next question
                if (curIdx < itemLength - 1)
                {
                    curIdx++;
                    DisplayQuestion();
                    if (curIdx == itemLength - 1)
                    {
                        BtnNext.Text = "Submit";
                    }
                }
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