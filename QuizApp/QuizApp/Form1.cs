using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace QuizApp
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            extract convert = new extract();
            convert.array_of_groupboxes[0].Visible = true;
        }


    }

    public class question_and_answer

    {
        public string Module { get; set; }

        public int Difficulty { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string[] Possibilities { get; set; }
    }//question and answer class

    public class extract
    {
        public List<GroupBox> array_of_groupboxes = new List<GroupBox>();

        public generate_labels generate_label;

        String[] easyQuestions = new String[10];
        String[] mediumQuestions = new String[10];
        String[] hardQuestions = new String[10];

        List<question_and_answer> easy_questions = new List<question_and_answer>();
        List<question_and_answer> medium_questions = new List<question_and_answer>();
        List<question_and_answer> hard_questions = new List<question_and_answer>();



        public extract()
        {
            List<question_and_answer> values = null;
            using (StreamReader r = new StreamReader("file.json"))
            {
                string json = r.ReadToEnd();
                values = JsonConvert.DeserializeObject<List<question_and_answer>>(json);
            }



            // TESTING 123

            try
            {
                foreach (question_and_answer value in values)

                {
                    switch (value.Difficulty)
                    {
                        case 1:
                            easy_questions.Add(value);
                            break;
                        case 2:
                            medium_questions.Add(value);
                            break;
                        case 3:
                            hard_questions.Add(value);
                            break;
                    }
                    Debug.WriteLine(easy_questions);

                    generate_label = new generate_labels(value);
                    array_of_groupboxes.Add(generate_label.return_gbox());
                }
            }
            catch
            {
                Label EmptyLabel = new Label();
                EmptyLabel.Text = "json file empty";
                Application.OpenForms["Form1"].Controls.Add(EmptyLabel);
            }
        }
    }

    public class generate_labels

    {
        public static Point points = new Point(10, 10);

        GroupBox gb = new GroupBox();
        Label question = new Label();
        Label difficulty = new Label();
        Button btn_submit = new Button();
        private RadioButton selectedrb = new RadioButton();

        Form myform = Application.OpenForms["Form1"];
        string correct_answer = "";

        public void button_create(Point point)
        {
            btn_submit.Text = "Submit";
            btn_submit.Location = new Point(point.X, point.Y + 40);
            btn_submit.Click += new EventHandler(this.btn_submit_click);
            gb.Controls.Add(btn_submit);
        }

        public void radioButton_CheckedChanged(object sender, EventArgs e) // Object which has raised an event
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }


            else if (rb.Checked)
            {

                selectedrb = rb;
            }
        }



        public generate_labels(question_and_answer q_and_a)

        {

            question.Text = q_and_a.Question;
            question.Location = new Point(10, 10);
            question.Size = new Size(question.Size.Width + 100, question.Size.Height);

            difficulty.Text = (q_and_a.Difficulty).ToString();
            difficulty.Location = new Point(250, 10);
            difficulty.Size = new Size(10, 10);

            Point radiobutton_point = new Point(10, 10);
            Size gb_size = new Size(myform.Size.Width, myform.Size.Height);


            foreach (string possibility in q_and_a.Possibilities)
            {
                RadioButton rb = new RadioButton();
                rb.Name = possibility;
                rb.Text = possibility;
                rb.Location = new Point(radiobutton_point.X, radiobutton_point.Y + 20);
                radiobutton_point = new Point(radiobutton_point.X, radiobutton_point.Y + 20);
                gb.Controls.Add(rb);
                rb.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            }

            correct_answer = q_and_a.Answer;

            gb.Size = gb_size;
            button_create(radiobutton_point);
            gb.Controls.Add(question);
            gb.Controls.Add(difficulty);
            gb.Visible = false;

            myform.Controls.Add(gb);



        }

        private void btn_submit_click(object sender, EventArgs e)
        {


            if (selectedrb.Text == "")
            {

                MessageBox.Show("Please Select a Result");
            }


            else if (selectedrb.Text == correct_answer)
            {
                MessageBox.Show("Correct");

            }
            else
            {

                MessageBox.Show("InCorrect");
            }



        }

        public GroupBox return_gbox()
        {
            return gb;
        }

    }







}
