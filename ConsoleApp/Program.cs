using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Terminal.Gui;
using MainClassLib;
using Microsoft.Data.Sqlite;
using ServiceLib;
using System.Xml.Serialization;

namespace ConsoleApp
{
    class Program
    {
        static string serializedFilePath = "../ConsoleApp/serialized.xml";
        static User mainUser;

        static User curUser;
        static Answer curAnswer;
        static Question curQuestion;

        static ListView curView;

        static List<User> curUsers;
        static List<Answer> curAnswers;
        static List<Question> curQuestions;

        static Window mainWindow;
        static bool correctInput;

        /*static UserRepository userRepository;
        static AnswerRepository answerRepository;
        static QuestionsRepository questionRepository;*/

        static MenuBar menu;

        static Label curTxt;
        static DateTime start;
        static DateTime end;
        static string path;

        const int pageSize = 10;
        static int currentPage = 1;
        static void Main()
        {
            string dataPath = @"../data/new";
            SqliteConnection connection = new SqliteConnection($"Data Source={dataPath}");
            if (!File.Exists(dataPath))
            {
                Console.Write("Check DB");
                return;
            }
            /* userRepository = new UserRepository(connection);
             answerRepository = new AnswerRepository(connection);
             questionRepository = new QuestionsRepository(connection);*/
            Application.Init();
            menu = new MenuBar(new MenuBarItem[] {
           new MenuBarItem ("_File", new MenuItem [] {
               new MenuItem ("_MainPage", "", ToMain),
               new MenuItem("_Export", "", ShowExport),
               new MenuItem("_Import", "", ToMain),
               new MenuItem("_Exit", "", ShowLogin),

           }),
       });
            ShowLogin();
            //GenerateData.GenerateDatabase(20, 100, 50, new DateTime(20, 10, 10), new DateTime(22, 10, 10));
            //ImportExport.Export(new DateTime(20, 10, 10), new DateTime(22, 10, 10), @"C:\Users\Myhasik\projects\test.xml", questionRepository);*/
        }
        public static byte[] Serialize<T>(T data, string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            System.IO.File.WriteAllText(filePath, "");
            System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath);
            ser.Serialize(writer, data);
            writer.Close();
            string text = System.IO.File.ReadAllText(filePath);
            byte[] bytes = Encoding.ASCII.GetBytes(text);


            return bytes;
        }
        public static T Deserialize<T>(string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(filePath);
            T value = (T)ser.Deserialize(reader);
            reader.Close();
            return value;
        }

        static void InitMainWindow()
        {
            Toplevel top = Application.Top;
            Rect pageFrame = new Rect(4, 8, top.Frame.Width, 10);
            curView = new ListView(pageFrame, new List<Answer>());
            mainWindow = new Window(mainUser.name);
            Button myQuestions = new Button(1, 3, "My questions");
            myQuestions.Clicked += ShowQuestionsOfUser;
            Button myAnswers = new Button(1, 4, "My answers");
            myAnswers.Clicked += ShowAnswersOfUser;
            Button allUsers = new Button(1, 5, "All users");
            allUsers.Clicked += ShowAllUsers;
            Button allquest = new Button(1, 6, "All questions");
            allquest.Clicked += ShowAllQuestions;
            Button createQuestion = new Button(1, 7, "Create question");
            createQuestion.Clicked += CreateQuestion;
            mainWindow.Add(myAnswers, myQuestions, allUsers, createQuestion, allquest);
            top.Add(mainWindow, menu);
            Application.Run();
        }
        static void ShowExport()
        {
            Label startL = new Label(1, 5, "Start:");
            TextField start = new TextField(1, 6, 15, "");
            Label endL = new Label(1, 7, "End:");
            TextField end = new TextField(1, 8, 50, "");
            start.TextChanging += OnStartDataChanged;
            end.TextChanging += OnEndDataChanged;
            Button acceptButton = new Button(1, 2, "Export");
            Button cancelButton = new Button(1, 3, "Cancel");
            Button selectPath = new Button(1, 4, "Select path");
            selectPath.Clicked += SelectDirectory;
            acceptButton.Clicked += GenerateExport;
            cancelButton.Clicked += ToMain;
            Window win = new Window("Export");
            win.Add(acceptButton, cancelButton, start, startL, endL, end, selectPath);
            Application.Top.RemoveAll();
            Application.Top.Add(win);
            Application.Run();
        }
        static void GenerateExport()
        {
            ImportExport.Export(start, end, path);
        }
        static void OnStartDataChanged(TextChangingEventArgs args)
        {
            if (!DateTime.TryParse(args.NewText.ToString(), out start))
            {
                start = new DateTime();
            }
        }
        static void OnEndDataChanged(TextChangingEventArgs args)
        {
            if (!DateTime.TryParse(args.NewText.ToString(), out end))
            {
                start = new DateTime();
            }
        }
        static void SelectDirectory()
        {
            OpenDialog dialog = new OpenDialog("Select path", "Open?")
            {
                CanChooseFiles = false,
                CanChooseDirectories = true,
            }
            ;
            // dialog.DirectoryPath = ...

            Application.Run(dialog);

            if (!dialog.Canceled)
            {
                path = dialog.FilePath.ToString();
            }

        }
        static void ShowLogin()
        {
            mainUser = null;
            curUser = new User();
            Label userLabel = new Label(4, 5, "Username");
            TextField username = new TextField(4, 6, 10, "");
            Label passLabel = new Label(4, 7, "Password");
            TextField password = new TextField(4, 8, 10, "");
            Button login = new Button(4, 9, "Log in");
            Button register = new Button(4, 10, "To register");
            Window window = new Window("Login");
            password.TextChanging += OnPasswordTextChange;
            username.TextChanging += OnUsernameTextChange;
            login.Clicked += TryLogin;
            register.Clicked += ShowRegister;
            window.Add(userLabel, username, passLabel, password, login, register);
            Application.Top.RemoveAll();
            Application.Top.Add(window);
            Application.Run();
        }
        static void ShowRegister()
        {
            curUser = new User();
            Label userLabel = new Label(4, 5, "Username");
            TextField username = new TextField(4, 6, 10, "");
            Label passLabel = new Label(4, 7, "Password");
            TextField password = new TextField(4, 8, 10, "");
            Button login = new Button(4, 10, "To login");
            Button register = new Button(4, 9, "Register");
            Window window = new Window("Register");
            password.TextChanging += OnPasswordTextChange;
            username.TextChanging += OnUsernameTextChange;
            login.Clicked += ShowLogin;
            register.Clicked += TryRegister;
            window.Add(userLabel, username, passLabel, password, login, register);
            Application.Top.RemoveAll();
            Application.Top.Add(window);
            Application.Run();
        }
        static void OnPasswordTextChange(TextChangingEventArgs args)
        {
            curUser.password = args.NewText.ToString();
        }
        static void OnUsernameTextChange(TextChangingEventArgs args)
        {
            curUser.name = args.NewText.ToString();
        }
        static void TryRegister()
        {
            mainUser = Authentication.AcceptRegistration(curUser.name, curUser.password);
            if (mainUser != null)
            {
                curUser = mainUser;
                Serialize<User>(curUser, serializedFilePath);
                RemoteService.RemoteServiceCommand("InserUser$" + mainUser);
                Deserialize<int>(serializedFilePath);
                // userRepository.Insert(mainUser);
                InitMainWindow();
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Username already exists", "Ok");
            }
        }
        static void TryLogin()
        {
            mainUser = Authentication.AcceptLogin(curUser.name, curUser.password);
            if (mainUser != null)
            {
                curUser = mainUser;
                InitMainWindow();
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Wrong username or password", "Ok");
            }
        }
        static void OnAnswerClick(ListViewItemEventArgs args)
        {
            curAnswer = (Answer)args.Value;
            ShowCurAnswer();
        }
        static void OnQuestionClick(ListViewItemEventArgs args)
        {
            curQuestion = (Question)args.Value;
            ShowCurQuestion();
        }
        static void OnUserClick(ListViewItemEventArgs args)
        {
            curUser = (User)args.Value;
            ShowCurUser();
        }

        static void ShowCurAnswer()
        {
            curUser = null;
            curQuestion = null;
            RemoteService.RemoteServiceCommand("GetAnswerAuthor$" + curAnswer.id);
            curAnswer.user = /*userRepository.GetAnswerAuthor(curAnswer.id); */ Deserialize<User>(serializedFilePath);
            RemoteService.RemoteServiceCommand("GetQByAnswer$" + curAnswer.id);
            curAnswer.question = /*questionRepository.GetByAnswer(curAnswer.id);*/ Deserialize<Question>(serializedFilePath);
            Label answerT = new Label(4, 7, "Answer text:");
            TextView body = new TextView(new Rect(4, 8, 15, 10));
            body.Text = curAnswer.body;
            Button toQuestion = new Button(4, 12, "To question");
            toQuestion.Clicked += ShowQuestionOfAnswer;
            Window window = new Window(curAnswer.title);
            Label author = new Label(4, 2, "Author: " + curAnswer.user.name);
            author.Clicked += ShowAuthorOfAnswer;
            RemoteService.RemoteServiceCommand("GetAllUserAns$" + mainUser.id);
            bool isMainUserAnswer = /*answerRepository.GetAllUserAns(mainUser.id)*/ Deserialize<List<Answer>>(serializedFilePath).FindIndex(x => x.id == curAnswer.id) != -1;
            if (isMainUserAnswer || mainUser.isModerator)
            {
                Button deleteAnswer = new Button(4, 3, "Delete answer");
                deleteAnswer.Clicked += DeleteAnswer;
                window.Add(deleteAnswer);
            }
            if (isMainUserAnswer)
            {
                Button updateAnswer = new Button(4, 4, "Update answer");
                updateAnswer.Clicked += UpdateAnswer;
                window.Add(updateAnswer);
            }
            window.Add(body, author, toQuestion);

            Application.Top.RemoveAll();
            Application.Top.Add(window, menu);
            Application.Run();
        }

        static void ShowCurUser()
        {
            if (curUser.name != mainUser.name)
            {
                curAnswer = null;
                curQuestion = null;
                Button myQuestions = new Button(1, 3, "User questions");
                myQuestions.Clicked += ShowQuestionsOfUser;
                Button myAnswers = new Button(1, 4, "User answers");
                myAnswers.Clicked += ShowAnswersOfUser;
                Window win = new Window(curUser.name);
                win.Add(myAnswers, myQuestions);
                Application.Top.RemoveAll();
                Application.Top.Add(win, menu);
                Application.Run();
            }
            else
            {
                ToMain();
            }
        }
        static void ShowCurQuestion()
        {
            correctInput = true;
            curUser = null;
            curAnswer = null;
            RemoteService.RemoteServiceCommand("GetQuestionAuthor$" + curQuestion.id);
            curQuestion.user = /*userRepository.GetQuestionAuthor(curQuestion.id)*/ Deserialize<User>(serializedFilePath);
            RemoteService.RemoteServiceCommand("GetMainAnswer$" + curQuestion.id);
            curQuestion.mainAnswer = /*answerRepository.GetMainAnswer(curQuestion.id);*/ Deserialize<Answer>(serializedFilePath);
            Label answerT = new Label(4, 7, "Question text:");
            TextView body = new TextView(new Rect(4, 8, Application.Top.Frame.Width, 10))
            {
                ReadOnly = true
            };
            int rowLength = 30;
            if (curQuestion.body.Length < rowLength)
                body.Text = curQuestion.body;
            else
            {
                string parsedBody = curQuestion.body;
                for (int i = 0; i < curQuestion.body.Length / rowLength; i++)
                {
                    parsedBody = parsedBody.Insert((i + 1) * rowLength, "\r\n");
                }
                body.Text = parsedBody;
            }
            Window window = new Window(curQuestion.title);
            Label author = new Label(4, 2, "Author: " + curQuestion.user.name);
            Button getAnswers = new Button(4, 3, "To answers");
            Button addAnswer = new Button(4, 4, "Add answer");
            Label mainAnwer = new Label(4, 7, curQuestion.mainAnswer != null ? "Main answer id: " + curQuestion.mainAnswer.id : "No main answers");
            addAnswer.Clicked += CreateAnswer;
            getAnswers.Clicked += ShowAnswersOfQuestion;
            author.Clicked += ShowAuthorOfQuestion;
            RemoteService.RemoteServiceCommand("GetAllQuestionsByUser$" + mainUser.id);
            bool isMainUserQuestion =/* questionRepository.GetAllByUser(mainUser.id)*/Deserialize<List<Question>>(serializedFilePath).FindIndex(x => x.id == curQuestion.id) != -1;
            if (isMainUserQuestion || mainUser.isModerator)
            {
                Button deleteQuestion = new Button(4, 5, "Delete question");
                deleteQuestion.Clicked += DeleteQuestion;
                window.Add(deleteQuestion);
            }
            if (isMainUserQuestion)
            {
                Button updateQuestion = new Button(4, 6, "Update question");
                updateQuestion.Clicked += UpdateQuestion;
                window.Add(updateQuestion);
            }
            window.Add(body, author, getAnswers, addAnswer, mainAnwer);
            Application.Top.RemoveAll();
            Application.Top.Add(window, menu);
            Application.Run();
        }
        static void ShowAuthorOfAnswer()
        {
            curUser = curAnswer.user;
            ShowCurUser();
        }
        static void ShowAuthorOfQuestion()
        {
            curUser = curQuestion.user;
            ShowCurUser();
        }
        static void ShowQuestionOfAnswer()
        {
            RemoteService.RemoteServiceCommand("GetQByAnswer$" + curAnswer.id);
            curQuestion = /*questionRepository.GetByAnswer(curAnswer.id);*/Deserialize<Question>(serializedFilePath);
            ShowCurQuestion();
        }
        static void ShowAnswersOfUser()
        {
            RemoteService.RemoteServiceCommand("GetAllUserAns$" + curUser.id);
            curAnswers = /*answerRepository.GetAllUserAns(curUser.id);*/ Deserialize<List<Answer>>(serializedFilePath);
            curQuestions = null;
            curUsers = null;
            InitiateListWindow<Answer>(curAnswers, "Answers of user " + curUser.name);
        }
        static void ShowQuestionsOfUser()
        {
            RemoteService.RemoteServiceCommand("GetAllQuestionsByUser$" + curUser.id);
            curQuestions = /*questionRepository.GetAllByUser(curUser.id);*/ Deserialize<List<Question>>(serializedFilePath);
            curAnswers = null;
            curUsers = null;
            InitiateListWindow<Question>(curQuestions, "Questions of user " + curUser.name);
        }
        static void ShowAnswersOfQuestion()
        {
            RemoteService.RemoteServiceCommand("GetByQuestionId$" + curQuestion.id);
            curAnswers = /*answerRepository.GetByQuestionId(curQuestion.id);*/ Deserialize<List<Answer>>(serializedFilePath);
            curQuestions = null;
            curUsers = null;
            InitiateListWindow<Answer>(curAnswers, "Answers for question " + curQuestion.id);
        }
        static void ShowAllUsers()
        {
            curQuestions = null;
            curAnswers = null;
            RemoteService.RemoteServiceCommand("GetAllUsers$");
            curUsers = /*userRepository.GetAll();*/ Deserialize<List<User>>(serializedFilePath);
            InitiateListWindow<User>(curUsers, "All users");
        }
        static void ShowAllQuestions()
        {
            RemoteService.RemoteServiceCommand("GetAllQuestions$");
            curQuestions = /*questionRepository.GetAll();*/ Deserialize<List<Question>>(serializedFilePath);
            curAnswers = null;
            curUsers = null;
            InitiateListWindowWithLooking<Question>(curQuestions, "All question");
        }


        static void CreateAnswer()
        {
            curAnswer = new Answer();
            curAnswer.user = mainUser;
            curAnswer.question = curQuestion;
            curAnswer.time = DateTime.Now;
            TextField body = new TextField(1, 6, 50, "");
            //TextField id = new TextField(1, 8, 50, "");
            body.TextChanging += OnAnswerChange;
            if (curAnswer.title == null)
                curAnswer.title = "answer tp q:" + curAnswer.question.id;
            Button acceptButton = new Button(1, 2, "Create");
            Button cancelButton = new Button(1, 3, "Cancel");
            acceptButton.Clicked += TryAcceptAnswerCreation;
            cancelButton.Clicked += ShowCurQuestion;
            Window win = new Window("Create answer");
            win.Add(body, acceptButton, cancelButton);
            Application.Top.RemoveAll();
            Application.Top.Add(win);
            Application.Run();
        }
        static void TryAcceptAnswerCreation()
        {
            int resultButtonIndex = MessageBox.Query(
              "Accept",
              "Accept changes?",
              "Yes", "No", "Cancel");
            if (resultButtonIndex == 0)
                AcceptAnswerCreation();
            else if (resultButtonIndex == 1)
                ShowCurQuestion();
        }
        static void AcceptAnswerCreation()
        {
            Serialize<Answer>(curAnswer, serializedFilePath);
            RemoteService.RemoteServiceCommand("InsertANswer$" + curAnswer);
            curAnswer.id = /*answerRepository.Insert(curAnswer);*/
            Deserialize<int>(serializedFilePath);
            ShowCurAnswer();
        }
        static void OnAnswerChange(TextChangingEventArgs args)
        {
            curAnswer.body = args.NewText.ToString();
        }


        static void CreateQuestion()
        {
            curQuestion = new Question();
            curQuestion.user = mainUser;
            curQuestion.start = DateTime.Now;
            Label titleL = new Label(1, 5, "Title:");
            TextField title = new TextField(1, 6, 15, "");
            Label bodyL = new Label(1, 7, "Body:");
            TextField body = new TextField(1, 8, 50, "");
            body.TextChanging += OnQuestionChange;
            title.TextChanging += OnTitleQuestionChange;
            Button acceptButton = new Button(1, 2, "Create");
            Button cancelButton = new Button(1, 3, "Cancel");
            acceptButton.Clicked += TryAcceptQuestionCreation;
            cancelButton.Clicked += ToMain;
            Window win = new Window("Create question");
            win.Add(body, acceptButton, cancelButton, title, titleL, bodyL);
            Application.Top.RemoveAll();
            Application.Top.Add(win);
            Application.Run();
        }
        static void TryAcceptQuestionCreation()
        {
            int resultButtonIndex = MessageBox.Query(
              "Accept",
              "Accept changes?",
              "Yes", "No", "Cancel");
            if (resultButtonIndex == 0)
                AcceptQuestionCreation();
            else if (resultButtonIndex == 1)
                ToMain();
        }
        static void AcceptQuestionCreation()
        {
            Serialize<Question>(curQuestion, serializedFilePath);
            RemoteService.RemoteServiceCommand("InsertQuestion$" + curQuestion);
            curQuestion.id = //questionRepository.Insert(curQuestion);
            Deserialize<int>(serializedFilePath);
            ShowCurQuestion();
        }
        static void OnQuestionChange(TextChangingEventArgs args)
        {
            curQuestion.body = args.NewText.ToString();
        }
        static void OnTitleQuestionChange(TextChangingEventArgs args)
        {
            curQuestion.title = args.NewText.ToString();
        }
        static void OnMainAnswerId(TextChangingEventArgs args)
        {
            int id;
            if (args.NewText.ToString() == "")
            {
                curQuestion.mainAnswer = null;
            }
            else if (!int.TryParse(args.NewText.ToString(), out id))
            {
                curQuestion.mainAnswer = null;
                correctInput = false;
            }
            else
            {
                RemoteService.RemoteServiceCommand("GetAnswerById$" + id);
                Answer answer = //answerRepository.GetById(id);
                Deserialize<Answer>(serializedFilePath);
                if (answer != null)
                {
                    RemoteService.RemoteServiceCommand("GetQByAnswer$" + answer.id);
                    if (/*questionRepository.GetByAnswer(answer.id)*/Deserialize<Question>(serializedFilePath).id == curQuestion.id)
                    {
                        correctInput = true;
                        RemoteService.RemoteServiceCommand("GetAnswerById$" + id);
                        curQuestion.mainAnswer = //answerRepository.GetById(id);
                        Deserialize<Answer>(serializedFilePath);
                    }
                    else
                    {
                        correctInput = false;
                    }
                }
                else
                {
                    correctInput = false;
                }
            }
        }
        static void DeleteQuestion()
        {
            int resultButtonIndex = MessageBox.ErrorQuery(
              "Delete",
              "Delete question?",
              "Yes", "No");
            if (resultButtonIndex == 0)
            {
                RemoteService.RemoteServiceCommand("DeleteQuestionById$" + curQuestion.id);
                //questionRepository.DeleteById(curQuestion.id);
                Deserialize<int>(serializedFilePath);
                RemoteService.RemoteServiceCommand("DeleteAllAnswersOfQuestion" + curQuestion.id);
                Deserialize<int>(serializedFilePath);
                //answerRepository.DeleteAllAnswersOfQuestion(curQuestion.id);
                curQuestion = null;
                ToMain();
            }
        }
        static void DeleteAnswer()
        {
            int resultButtonIndex = MessageBox.ErrorQuery(
              "Delete",
              "Delete answer?",
              "Yes", "No");
            if (resultButtonIndex == 0)
            {
                RemoteService.RemoteServiceCommand("DeleteAnswerById" + curQuestion.id);
                Deserialize<int>(serializedFilePath);
                // answerRepository.DeleteById(curAnswer.id);
                curAnswer = null;
                ToMain();
            }
        }

        static void UpdateQuestion()
        {
            Label titleL = new Label(1, 5, "Title:");
            TextField title = new TextField(1, 6, 15, curQuestion.title);
            Label bodyL = new Label(1, 7, "Body:");
            TextField body = new TextField(1, 8, 50, curQuestion.body);
            Label qIdL = new Label(1, 9, "Main question id:");
            TextField qId = new TextField(1, 10, 4, curQuestion.mainAnswer == null ? "" : curQuestion.mainAnswer.id.ToString());
            qId.TextChanging += OnMainAnswerId;
            body.TextChanging += OnQuestionChange;
            title.TextChanging += OnTitleQuestionChange;
            Button acceptButton = new Button(1, 2, "Update");
            Button cancelButton = new Button(1, 3, "Cancel");
            acceptButton.Clicked += TryAcceptQuestionUpdate;
            cancelButton.Clicked += ShowCurQuestion;
            Window win = new Window("Update question");
            win.Add(body, acceptButton, cancelButton, qId, qIdL, bodyL, title, titleL);
            Application.Top.RemoveAll();
            Application.Top.Add(win);
            Application.Run();
        }
        static void TryAcceptQuestionUpdate()
        {
            int resultButtonIndex = MessageBox.Query(
              "Accept",
              "Accept changes?",
              "Yes", "No", "Cancel");
            if (resultButtonIndex == 0)
                AcceptQuestionUpdate();
            else if (resultButtonIndex == 1)
                ShowCurQuestion();
        }
        static void AcceptQuestionUpdate()
        {
            if (correctInput)
            {
                Serialize<Question>(curQuestion, serializedFilePath);
                RemoteService.RemoteServiceCommand("UpdateQuestion$" + curQuestion);
                Deserialize<int>(serializedFilePath);
                //questionRepository.UpdateQuestion(curQuestion);
                ShowCurQuestion();
            }
            else
            {
                MessageBox.ErrorQuery(
               "Error",
               "Wrong question id",
               "Ok");
            }
        }
        static void UpdateAnswer()
        {
            Label titleL = new Label(1, 5, "Title:");
            TextField title = new TextField(1, 6, 15, curAnswer.title);
            Label bodyL = new Label(1, 7, "Body:");
            TextField body = new TextField(1, 8, 50, curAnswer.body);
            body.TextChanging += OnAnswerChange;
            title.TextChanging += OnAnswerTitleChange;
            Button acceptButton = new Button(1, 2, "Create");
            Button cancelButton = new Button(1, 3, "Cancel");
            acceptButton.Clicked += TryAcceptAnswerUpdate;
            cancelButton.Clicked += ShowCurAnswer;
            Window win = new Window("Update answer");
            win.Add(body, acceptButton, cancelButton, bodyL, title, titleL);
            Application.Top.RemoveAll();
            Application.Top.Add(win);
            Application.Run();
        }
        static void OnAnswerTitleChange(TextChangingEventArgs args)
        {
            curAnswer.title = args.NewText.ToString();
        }
        static void TryAcceptAnswerUpdate()
        {
            int resultButtonIndex = MessageBox.Query(
              "Accept",
              "Accept changes?",
              "Yes", "No", "Cancel");
            if (resultButtonIndex == 0)
                AcceptAnswerUpdate();
            else if (resultButtonIndex == 1)
                ShowCurAnswer();
        }
        static void AcceptAnswerUpdate()
        {
            Serialize<Answer>(curAnswer, serializedFilePath);
            RemoteService.RemoteServiceCommand("UpdateAnswer$" + curAnswer);
            Deserialize<int>(serializedFilePath);
            //answerRepository.UpdateAnswer(curAnswer);
            ShowCurAnswer();
        }
        static void InitiateListWindow<T>(List<T> curList, string windowName)
        {
            currentPage = 1;
            Toplevel top = Application.Top;
            top.RemoveAll();

            Button nextPage = new Button(10, 2, "->");
            nextPage.Clicked += NextPage;
            Button prevPage = new Button(4, 2, "<-");
            prevPage.Clicked += PrevPage;
            curView = new ListView(new Rect(4, 8, Application.Top.Frame.Width, 10), new List<T>());
            string page = currentPage.ToString();
            curTxt = new Label(20, 2, page);
            curView.SetSource(GetPage<T>(curList));
            Button back = new Button(4, 3, "");
            if (typeof(T) == typeof(Answer))
            {
                curView.OpenSelectedItem += OnAnswerClick;
            }
            else if (typeof(T) == typeof(Question))
            {
                curView.OpenSelectedItem += OnQuestionClick;
            }
            else if (typeof(T) == typeof(User))
            {
                curView.OpenSelectedItem += OnUserClick;
            }

            if (curQuestion != null)
            {
                back.Text = "To question";
                back.Clicked += ShowCurQuestion;
            }
            else if (curUser != null)
            {
                back.Text = "To user";
                back.Clicked += ShowCurUser;
            }
            Window list = new Window(windowName);

            top.Add(list, menu, curView, nextPage, prevPage, back, curTxt);

            Application.Run();
        }
        static void InitiateListWindowWithLooking<T>(List<T> curList, string windowName)
        {
            curQuestion = new Question();
            currentPage = 1;
            Toplevel top = Application.Top;
            top.RemoveAll();
            Button nextPage = new Button(10, 2, "->");
            nextPage.Clicked += NextPage;
            Button prevPage = new Button(4, 2, "<-");
            TextField searchTxt = new TextField(4, 4, 15, "");
            string page = currentPage.ToString();
            curTxt = new Label(20, 2, page);
            Button searchBtn = new Button(4, 6, "search");
            searchBtn.Clicked += FindQuestions;
            searchTxt.TextChanging += OnSearchText;
            prevPage.Clicked += PrevPage;
            curView = new ListView(new Rect(4, 8, Application.Top.Frame.Width, 10), new List<T>());
            curView.SetSource(GetPage<T>(curList));
            Button back = new Button(4, 3, "");
            if (typeof(T) == typeof(Answer))
            {
                curView.OpenSelectedItem += OnAnswerClick;
            }
            else if (typeof(T) == typeof(Question))
            {
                curView.OpenSelectedItem += OnQuestionClick;
            }
            else if (typeof(T) == typeof(User))
            {
                curView.OpenSelectedItem += OnUserClick;
            }

            /*if (curQuestion != null)
            {
                back.Text = "To question";
                back.Clicked += ShowCurQuestion;
            }*/
            if (curUser != null)
            {
                back.Text = "To user";
                back.Clicked += ShowCurUser;
            }
            Window list = new Window(windowName);

            top.Add(list, menu, curView, nextPage, prevPage, back, searchBtn, searchTxt, curTxt);

            Application.Run();
        }
        static void OnSearchText(TextChangingEventArgs args)
        {
            curQuestion.body = args.NewText.ToString();
        }

        static void ToMain()
        {
            curUser = mainUser;
            Application.Top.RemoveAll();
            Application.Top.Add(mainWindow);
            //Application.Runallques();
        }
        static void FindQuestions()
        {
            RemoteService.RemoteServiceCommand("GetAllContains$" + curQuestion.body);
            curQuestions =// questionRepository.GetAllContains(curQuestion.body);
            Deserialize<List<Question>>(serializedFilePath);
            InitiateListWindowWithLooking<Question>(curQuestions, "questions");
        }

        static void SetCurrentPage()
        {
            curTxt.Text = currentPage.ToString();
            if (curQuestions != null)
            {
                curView.SetSource(GetPage<Question>(curQuestions));
            }
            else if (curAnswers != null)
            {
                curView.SetSource(GetPage<Answer>(curAnswers));
            }
            else if (curUsers != null)
            {
                curView.SetSource(GetPage<User>(curUsers));
            }
        }
        static List<T> GetPage<T>(List<T> list)
        {
            if (list.Count == 0)
            {
                return null;
            }
            else if (list.Count % pageSize != 0 && currentPage == GetPageNum(list))
            {
                return list.GetRange((currentPage - 1) * pageSize, list.Count % pageSize);
            }
            else
            {
                return list.GetRange((currentPage - 1) * pageSize, pageSize);
            }
        }
        static int GetPageNum<T>(List<T> list)
        {
            if (list.Count % pageSize == 0)
                return list.Count / pageSize;
            else
                return list.Count / pageSize + 1;
        }
        static void NextPage()
        {
            int pageNum = 0;
            if (curQuestions != null)
            {
                pageNum = GetPageNum<Question>(curQuestions);
            }
            else if (curAnswers != null)
            {
                pageNum = GetPageNum<Answer>(curAnswers);
            }
            else if (curUsers != null)
            {
                pageNum = GetPageNum<User>(curUsers);
            }
            if (pageNum != currentPage)
            {
                currentPage = currentPage + 1;
                SetCurrentPage();
            }
        }
        static void PrevPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                SetCurrentPage();
            }
        }
    }
}

