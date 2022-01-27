using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

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

        public string resourceType
        {
            get
            {
                return "QuestionnaireResponse";
            }
            set
            {
                resourceType = "QuestionnaireResponse";
            }
        }

        //public const string questionnaire = "http://203.64.84.213:8080/fhir/Questionnaire/6902";
        public string questionnaire
        {
            get
            {
                return "Questionnaire/6902";
            }
            set
            {
                questionnaire = "Questionnaire/6902";
            }
        }
        public Reference subject { get; set; }
        public string authored { get; set; }
        public Item[] item { get; set; }
        //public const string status = "completed";
        public string status
        {
            get
            {
                return "completed";
            }
            set
            {
                status = "completed";
            }
        }
    }

    public class QuestionnaireResponse2
    {
        public QuestionnaireResponse2()
        {

        }

        public string resourceType
        {
            get
            {
                return "QuestionnaireResponse";
            }
            set
            {
                resourceType = "QuestionnaireResponse";
            }
        }

        //public const string questionnaire = "http://203.64.84.213:8080/fhir/Questionnaire/6902";
        public string questionnaire
        {
            get
            {
                return "Questionnaire/6902";
            }
            set
            {
                questionnaire = "Questionnaire/6902";
            }
        }
        public Reference subject { get; set; }
        public string authored { get; set; }
        //   public Item[] item { get; set; }
        //public const string status = "completed";
        public string status
        {
            get
            {
                return "completed";
            }
            set
            {
                status = "completed";
            }
        }
    }
    public partial class Question : System.Web.UI.Page
    {
        static readonly int SIZE = 40;
        static int itemLength = 0;
        static readonly int[] randNumbers = new int[SIZE];
        static int curIdx = 0;
        static dynamic json;
        static string[] answers = new string[SIZE];
        static readonly QuestionnaireResponse resp = new QuestionnaireResponse();
        static QuestionnaireResponse2 resp2 = new QuestionnaireResponse2();

        //static List<Item> listItem = new List<Item>(SIZE);
        static Item[] listItem = new Item[SIZE];

        protected void Post(string respJSON)
        {
            //resp2.resourceType = resp.resourceType;
            //resp2.questionnaire = resp.questionnaire;
            //resp2.status = resp.status;
            //resp2.authored = resp.authored;
            //resp2.subject = resp.subject;
            //string respJSON2 = JsonConvert.SerializeObject(resp2);


            string url = "http://203.64.84.213:8080/fhir/QuestionnaireResponse/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";

            byte[] byteArray = Encoding.UTF8.GetBytes(respJSON);//要發送的字串轉為byte[]

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }

            //發出Request
            string responseStr = "";
            using (WebResponse response = request.GetResponse())
            {

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseStr = reader.ReadToEnd();
                }

            }

            //輸出Server端回傳字串
            Response.Write("<br><br><br>" + responseStr);
            Console.WriteLine(responseStr);
        }
        protected void CreateQuestionnaireResponse()
        {
            Reference subject = new Reference()
            {
                reference = "Patient/" + Session["id"].ToString(),
                type = "Patient",
                display = Session["name"].ToString()

            };
            resp.subject = subject;

            resp.authored = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            resp.item = listItem;

            // Create JSON Object
            string respJSON = JsonConvert.SerializeObject(resp);
            //Response.Write(respJSON);
            Post(respJSON);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["id"] = "5674"; //person id=5674
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
                    int rand;
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
            int itemIndex = randNumbers[curIdx] - 1;
            var qItem = json["item"][itemIndex];

            Item item = new Item
            {
                linkId = (itemIndex + 1).ToString(),
                text = qItem.text
            };

            // START: Get student answer
            List<FHIRAnswer> list = new List<FHIRAnswer>();

            FHIRAnswer a = new FHIRAnswer();
            if (RadOptions.SelectedItem != null)
            {
                a.valueString = RadOptions.SelectedItem.Text;
            }
            else
            {
                a.valueString = "---";
            }
            list.Add(a);
            item.answer = list;
            listItem[itemIndex] = item;
            // END: Get student answer

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
            else
            {
                PanelQA.Visible = false;
                CreateQuestionnaireResponse();
            }
        }

        private void DisplayQuestion()
        {
            int itemIndex = randNumbers[curIdx] - 1;
            var qItem = json["item"][itemIndex];

            // Get question image (if any)
            if (qItem.definition != null)
            {
                Image1.Visible = true;
                Image1.ImageUrl = qItem.definition;
            }
            else
            {
                Image1.Visible = false;
            }

            // Get question item
            LblQuestion.Text = (curIdx + 1).ToString() + ". " + qItem.text;

            // Get & display answer options
            string[] options = new string[qItem["answerOption"].Count];
            for (int i = 0; i < qItem["answerOption"].Count; i++)
            {
                options[i] = qItem["answerOption"][i].valueString;
            }
            RadOptions.DataSource = options;
            RadOptions.DataBind();
        }

    }
}