using System;
using System.IO;
using System.Collections.Generic;
using Terminal.Gui;
using Microsoft.Data.Sqlite;
<<<<<<< HEAD
using System.Xml.Serialization;
=======
>>>>>>> 7cba30f923206795fb12ece32b2ea17fe5f6c188


namespace ConsoleApp
{
    public class User
    {
        [XmlElement("Id")]
        public int id;
        public string name;
        public string password;
        public bool isModerator;
        public List<Question> questions = new List<Question>();
        public List<Answer> answers = new List<Answer>();
        public User()
        {
            this.id = 0;
            this.name = "";
            this.password = "";
            this.isModerator = false;
            this.questions = null;
            this.answers = null;
        }
        public User(int id, string name, string password, bool isModerator)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.isModerator = isModerator;
        }
        public User(int id, string name, string password, bool isModerator, List<Question> questions, List<Answer> answers)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.isModerator = isModerator;
            this.questions = new List<Question>(questions);
            this.answers = new List<Answer>(answers);
        }
        public override string ToString()
        {
<<<<<<< HEAD
            return $"{id}, {name}";
=======
            return $"{id}, {name}, {password}, {isModerator}";
>>>>>>> 7cba30f923206795fb12ece32b2ea17fe5f6c188
        }
    }

    public class Question
    {
        [XmlElement("Id")]
        public int id;
        [XmlElement("Title")]
        public string title;
        [XmlElement("Body")]
        public string body;
        [XmlElement("Author")]
        public User user;
        [XmlIgnore]
        public List<Answer> listAnswers = new List<Answer>();
<<<<<<< HEAD
        [XmlElement("MainAnswer")]
        public Answer mainAnswer = null;
=======
        //public Question mainAnswer;
>>>>>>> 7cba30f923206795fb12ece32b2ea17fe5f6c188
        public DateTime start;
        public DateTime end = new DateTime();
        public Question()
        {
<<<<<<< HEAD

        }
        public override string ToString()
        {
            return $"{id},{title},{user}";
=======
            this.id = 0;
            this.body = "";
            this.user = null;
            //this.mainAnswer = new Question();
        }
        public Question(int id, string body, User user, Question mainAnswer, DateTime start, DateTime end)
        {
            this.id = id;
            this.body = body;
            this.user.id = user.id;
            //this.mainAnswer = mainAnswer;
            this.start = start;
            this.end = end;
        }
        public Question(int id, string body, Question mainAnswer, DateTime start, DateTime end)
        {
            this.id = id;
            this.body = body;

            //this.mainAnswer = mainAnswer;
            this.start = start;
            this.end = end;
        }
        public override string ToString()
        {
            return $"{id},{body},{user},{start},{end}";
>>>>>>> 7cba30f923206795fb12ece32b2ea17fe5f6c188
        }

    }
    public class Answer
    {
        [XmlElement("Id")]
        public int id;
        [XmlElement("Title")]
        public string title;
        [XmlElement("Body")]
        public string body;
        [XmlElement("Author")]
        public User user;
        public Question question;
        public DateTime time;
        public Answer()
        {
            
        }
         public Answer(int id, string title, string body, DateTime time, int userId)
        {
            this.id = id;
            user = new User();
            user.id = userId;
            this.title = title;
            this.body = body;
            this.time = time;
        }
        public override string ToString()
        {
            return $"{id},{title}";
        }

    }

    class Program
    {
        static User mainUser;

        static User curUser;
        static Answer curAnswer;
        static Question curQuestion;

        static ListView curView;

        static List<User> curUsers;
        static List<Answer> curAnswers;
        static List<Question> curQuestions;
<<<<<<< HEAD

        static Window mainWindow;
        static bool correctInput;

        static UserRepository userRepository;
        static AnswerRepository answerRepository;
        static QuestionsRepository questionRepository;

        static MenuBar menu;


        static DateTime start;
        static DateTime end;
        static string path;

        const int pageSize = 1;
        static int currentPage = 1;
        static void Main()
        {
            string dataPath = @"../data/dataBase.db";
            bool ch = File.Exists(dataPath);
            
            SqliteConnection connection = new SqliteConnection($"Data Source={dataPath}");
        //    connection.Open();
            userRepository = new UserRepository(connection);
            answerRepository = new AnswerRepository(connection);
            questionRepository = new QuestionsRepository(connection);
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
            //ImportExport.Export(new DateTime(20, 10, 10), new DateTime(22, 10, 10), @", questionRepository);
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
            Button createQuestion = new Button(1, 6, "Create question");
            createQuestion.Clicked += CreateQuestion;
            mainWindow.Add(myAnswers, myQuestions, allUsers, createQuestion);
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
            win.Add( acceptButton, cancelButton, start, startL, endL, end, selectPath);
            Application.Top.RemoveAll();
            Application.Top.Add(win);
            Application.Run();
        }
        static void GenerateExport()
        {
            ImportExport.Export(start, end, path, questionRepository);
        }
        static void OnStartDataChanged(TextChangingEventArgs args)
        {
           if(!DateTime.TryParse(args.NewText.ToString(), out start))
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
            mainUser = Authentication.AcceptRegistration(userRepository, curUser.name, curUser.password);
            if(mainUser != null)
            {
                curUser = mainUser;
                userRepository.Insert(mainUser);
                InitMainWindow();
            }
=======

        static Window mainWindow;

        static CheckBox isMainAnswer;

        static UserRepository userRepository;
        static AnswerRepository answerRepository;
        static QuestionsRepository questionRepository;

        const int pageSize = 1;
        static int currentPage = 1;
        
        static void Main()
        {
            string dataPath = @"/home/mariya/Desktop/projects/progbase3/Progbase3/data/dataBase";
            SqliteConnection connection = new SqliteConnection($"Data Source={dataPath}");
            userRepository = new UserRepository(connection);
            answerRepository = new AnswerRepository(connection);
            questionRepository = new QuestionsRepository(connection);
            Application.Init();
            ShowLogin();
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
            Button createQuestion = new Button(1, 6, "Create question");
            createQuestion.Clicked += CreateQuestion;
            mainWindow.Add(myAnswers, myQuestions, allUsers, createQuestion);
            top.Add(mainWindow);
            Application.Run();
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
            mainUser = Authentication.AcceptRegistration(userRepository, curUser.name, curUser.password);
            if(mainUser != null)
            {
                curUser = mainUser;
                userRepository.Insert(mainUser);
                InitMainWindow();
            }
>>>>>>> 7cba30f923206795fb12ece32b2ea17fe5f6c188
            else
            {
                MessageBox.ErrorQuery("Error", "Username already exists", "Ok");
            }
        }
        static void TryLogin()
        {
            mainUser = Authentication.AcceptLogin(userRepository, curUser.name, curUser.password);
            if (mainUser != null)
<<<<<<< HEAD
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
            curAnswer.user = userRepository.GetAnswerAuthor(curAnswer.id);
            curAnswer.question = questionRepository.GetByAnswer(curAnswer.id);
            Label answerT = new Label(4, 7, "Answer text:");
            TextView body = new TextView(new Rect(4, 8, 15, 10));
            body.Text = curAnswer.body;
            Button toQuestion = new Button(4, 12, "To question");
            toQuestion.Clicked += ShowQuestionOfAnswer;
            Window window = new Window(curAnswer.title);
            Label author = new Label(4, 2, "Author: " + curAnswer.user.name);
            author.Clicked += ShowAuthorOfAnswer;

            bool isMainUserAnswer = answerRepository.GetAllUserAns(mainUser.id).FindIndex(x => x.id == curAnswer.id) != -1;
            if (isMainUserAnswer || mainUser.isModerator)
            {
                Button deleteAnswer = new Button(4, 3, "Delete answer");
                deleteAnswer.Clicked += DeleteAnswer;
                window.Add(deleteAnswer);
            }
            if(isMainUserAnswer)
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
            curQuestion.user = userRepository.GetQuestionAuthor(curQuestion.id);
            curQuestion.mainAnswer = answerRepository.GetMainAnswer(curQuestion.id); 
            Label answerT = new Label(4, 7, "Question text:");
            TextView body = new TextView(new Rect(4, 8, Application.Top.Frame.Width, 10))
            {
                ReadOnly = true
            };
            int rowLength = 30; 
            if(curQuestion.body.Length < rowLength)
                body.Text = curQuestion.body;
            else
            {
                string parsedBody = curQuestion.body;
                for(int i = 0; i < curQuestion.body.Length / rowLength; i++)
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

            bool isMainUserQuestion = questionRepository.GetAllByUser(mainUser.id).FindIndex(x => x.id == curQuestion.id) != -1;
            if (isMainUserQuestion || mainUser.isModerator)
            {
                Button deleteQuestion = new Button(4, 5, "Delete question");
                deleteQuestion.Clicked += DeleteQuestion;
                window.Add(deleteQuestion);
            }
            if(isMainUserQuestion)
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
            curQuestion = questionRepository.GetByAnswer(curAnswer.id);
            ShowCurQuestion();
        }
        static void ShowAnswersOfUser()
        {
            curAnswers = answerRepository.GetAllUserAns(curUser.id);
            curQuestions = null;
            curUsers = null;
            InitiateListWindow<Answer>(curAnswers, "Answers of user " + curUser.name);
        }
        static void ShowQuestionsOfUser()
        {
            curQuestions = questionRepository.GetAllByUser(curUser.id);
            curAnswers = null;
            curUsers = null;
            InitiateListWindow<Question>(curQuestions, "Questions of user " + curUser.name);
        }
        static void ShowAnswersOfQuestion()
        {
            curAnswers = answerRepository.GetByQuestionId(curQuestion.id);
            curQuestions = null;
            curUsers = null;
            InitiateListWindow<Answer>(curAnswers, "Answers for question " + curQuestion.id);
        }
        static void ShowAllUsers()
        {
            curQuestions = null;
            curAnswers = null;
            curUsers = userRepository.GetAll();
            InitiateListWindow<User>(curUsers, "All users");
        }

        static void CreateAnswer()
        {
            curAnswer = new Answer();
            curAnswer.user = mainUser;
            curAnswer.question = curQuestion;
            curAnswer.time = DateTime.Now;
            TextField body = new TextField(1, 6, 50, "");
            body.TextChanging += OnAnswerChange;
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
            curAnswer.id = answerRepository.Insert(curAnswer);
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
            curQuestion.id = questionRepository.Insert(curQuestion);
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
            if(args.NewText.ToString() == "")
            {
                curQuestion.mainAnswer = null;
            }
            else if(!int.TryParse(args.NewText.ToString(), out id))
            {
                curQuestion.mainAnswer = null;
                correctInput = false;
            }
            else
            {
                Answer answer = answerRepository.GetById(id);
                if (answer != null)
                {
                    if(questionRepository.GetByAnswer(answer.id).id == curQuestion.id)
                    {
                        correctInput = true;
                        curQuestion.mainAnswer = answerRepository.GetById(id);

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
            if(resultButtonIndex == 0)
            {
                questionRepository.DeleteById(curQuestion.id);
                answerRepository.DeleteAllAnswersOfQuestion(curQuestion.id);
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
                answerRepository.DeleteById(curAnswer.id);
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
            if(correctInput)
            {
                questionRepository.UpdateQuestion(curQuestion);
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
            answerRepository.UpdateAnswer(curAnswer);
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
            curView = new ListView(new Rect(4, 8, Application.Top.Frame.Width, 10), new List<Answer>());
            curView.SetSource(GetPage<T>(curList));
            Button back = new Button(4, 3, "");
            if(typeof(T) == typeof(Answer))
            {
                curView.OpenSelectedItem += OnAnswerClick;
            }
            else if(typeof(T) == typeof(Question))
            {
                curView.OpenSelectedItem += OnQuestionClick;
            }
            else if(typeof(T) == typeof(User))
            {
                curView.OpenSelectedItem += OnUserClick;
            }

            if (curQuestion != null)
            {
                back.Text = "To question";
                back.Clicked += ShowCurQuestion;
            }
            else if(curUser != null)
            {
                back.Text = "To user";
                back.Clicked += ShowCurUser;
            }
            Window list = new Window(windowName);

            top.Add(list, menu, curView, nextPage, prevPage, back);

            Application.Run();
        }

        static void ToMain()
        {
            curUser = mainUser;
            Application.Top.RemoveAll();
            Application.Top.Add(mainWindow);
            //Application.Run();
        }

        static void SetCurrentPage()
        {
            if(curQuestions != null)
            {
                curView.SetSource(GetPage<Question>(curQuestions));
            }
            else if(curAnswers != null)
            {
                curView.SetSource(GetPage<Answer>(curAnswers));
            }
            else if(curUsers != null)
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
            if(curQuestions != null)
            {
                pageNum = GetPageNum<Question>(curQuestions);
            }
            else if(curAnswers != null)
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
=======
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
            curAnswer.user = userRepository.GetAnswerAuthor(curAnswer.id);
            curAnswer.question = questionRepository.GetByAnswer(curAnswer.id);
            Label answerT = new Label(4, 7, "Answer text:");
            TextView body = new TextView(new Rect(4, 8, 15, 10));
            body.Text = curAnswer.body;
            Button toQuestion = new Button(4, 12, "To question");
            toQuestion.Clicked += ShowQuestionOfAnswer;
            Window window = new Window("Answer");
            Label author = new Label(4, 2, "Author: " + curAnswer.user.name);
            author.Clicked += ShowAuthorOfAnswer;

            bool isMainUserAnswer = answerRepository.GetAllUserAns(mainUser.id).FindIndex(x => x.id == curAnswer.id) != -1;
            if (isMainUserAnswer || mainUser.isModerator)
            {
                Button deleteAnswer = new Button(4, 3, "Delete answer");
                deleteAnswer.Clicked += DeleteAnswer;
                window.Add(deleteAnswer);
            }
            if(isMainUserAnswer)
            {
                Button updateAnswer = new Button(4, 4, "Update answer");
                updateAnswer.Clicked += UpdateAnswer;
                window.Add(updateAnswer);
            }
            if(userRepository.GetQuestionAuthor((questionRepository.GetByAnswer(curAnswer.id).id)).name == mainUser.name)
            {
                isMainAnswer = new CheckBox(4, 4, "Selected answer");
                isMainAnswer.Checked = curAnswer.mainAnswer;
                isMainAnswer.Toggled += OnMainAnswerChange;
            }    
            window.Add(body, author, toQuestion);

            Application.Top.RemoveAll();
            Application.Top.Add(window);
            Application.Run();
        }
        static void OnMainAnswerChange(bool previousChecked)
        {
            if(previousChecked)
            {
                curAnswer.mainAnswer = false;
                answerRepository.UpdateAnswer(curAnswer);
            }
            else
            {
                if(answerRepository.GetByQuestionId(curAnswer.question.id).FindIndex(x => x.mainAnswer == true) != -1)
                {
                    isMainAnswer.Checked = false;
                    MessageBox.ErrorQuery(
              "Error",
              "Already selected answer",
              "Ok");
                }
                else
                {
                    curAnswer.mainAnswer = true;
                    answerRepository.UpdateAnswer(curAnswer);
                }
            }
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
                Application.Top.Add(win);
                Application.Run();
            }
            else
            {
                ToMain();
            }
        }
        static void ShowCurQuestion()
        {
            curUser = null;
            curAnswer = null;
            curQuestion.user = userRepository.GetQuestionAuthor(curQuestion.id);
            Label answerT = new Label(4, 7, "Question text:");
            TextView body = new TextView(new Rect(4, 8, Application.Top.Frame.Width, 10))
            {
                ReadOnly = true
            };
            body.Text = curQuestion.body;
            Window window = new Window("Question");
            Label author = new Label(4, 2, "Author: " + curQuestion.user.name);
            Button getAnswers = new Button(4, 3, "To answers");
            Button addAnswer = new Button(4, 4, "Add answer");
            addAnswer.Clicked += CreateAnswer;
            getAnswers.Clicked += ShowAnswersOfQuestion;
            author.Clicked += ShowAuthorOfQuestion;

            bool isMainUserQuestion = questionRepository.GetAllByUser(mainUser.id).FindIndex(x => x.id == curQuestion.id) != -1;
            if (isMainUserQuestion || mainUser.isModerator)
            {
                Button deleteQuestion = new Button(4, 5, "Delete question");
                deleteQuestion.Clicked += DeleteQuestion;
                window.Add(deleteQuestion);
            }
            if(isMainUserQuestion)
            {
                Button updateQuestion = new Button(4, 6, "Update question");
                updateQuestion.Clicked += UpdateQuestion;
                window.Add(updateQuestion);
            }
            window.Add(body, author, getAnswers, addAnswer);
            Application.Top.RemoveAll();
            Application.Top.Add(window);
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
            curQuestion = questionRepository.GetByAnswer(curAnswer.id);
            ShowCurQuestion();
        }
        static void ShowAnswersOfUser()
        {
            curAnswers = answerRepository.GetAllUserAns(curUser.id);
            curQuestions = null;
            curUsers = null;
            InitiateListWindow<Answer>(curAnswers, "Answers of user " + curUser.name);
        }
        static void ShowQuestionsOfUser()
        {
            curQuestions = questionRepository.GetAllByUser(curUser.id);
            curAnswers = null;
            curUsers = null;
            InitiateListWindow<Question>(curQuestions, "Questions of user " + curUser.name);
        }
        static void ShowAnswersOfQuestion()
        {
            curAnswers = answerRepository.GetByQuestionId(curQuestion.id);
            curQuestions = null;
            curUsers = null;
            InitiateListWindow<Answer>(curAnswers, "Answers for question " + curQuestion.id);
        }
        static void ShowAllUsers()
        {
            curQuestions = null;
            curAnswers = null;
            curUsers = userRepository.GetAll();
            InitiateListWindow<User>(curUsers, "All users");
        }

        static void CreateAnswer()
        {
            curAnswer = new Answer();
            curAnswer.user = mainUser;
            curAnswer.question = curQuestion;
            curAnswer.time = DateTime.Now;
            TextField body = new TextField(1, 6, 50, "");
            body.TextChanging += OnAnswerChange;
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
            curAnswer.id = answerRepository.Insert(curAnswer);
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
            TextField body = new TextField(1, 6, 50, "");
            body.TextChanging += OnQuestionChange;
            Button acceptButton = new Button(1, 2, "Create");
            Button cancelButton = new Button(1, 3, "Cancel");
            acceptButton.Clicked += TryAcceptQuestionCreation;
            cancelButton.Clicked += ToMain;
            Window win = new Window("Create question");
            win.Add(body, acceptButton, cancelButton);
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
            curQuestion.id = questionRepository.Insert(curQuestion);
            ShowCurQuestion();
        }
        static void OnQuestionChange(TextChangingEventArgs args)
        {
            curQuestion.body = args.NewText.ToString();
        }

        static void DeleteQuestion()
        {
            int resultButtonIndex = MessageBox.ErrorQuery(
              "Delete",
              "Delete question?",
              "Yes", "No");
            if(resultButtonIndex == 0)
            {
                questionRepository.DeleteById(curQuestion.id);
                answerRepository.DeleteAllAnswersOfQuestion(curQuestion.id);
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
                answerRepository.DeleteById(curAnswer.id);
                curAnswer = null;
                ToMain();
            }
        }

        static void UpdateQuestion()
        {
            TextField body = new TextField(1, 6, 50, curQuestion.body);
            body.TextChanging += OnQuestionChange;
            Button acceptButton = new Button(1, 2, "Update");
            Button cancelButton = new Button(1, 3, "Cancel");
            acceptButton.Clicked += TryAcceptQuestionUpdate;
            cancelButton.Clicked += ShowCurQuestion;
            Window win = new Window("Update question");
            win.Add(body, acceptButton, cancelButton);
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
            questionRepository.UpdateQuestion(curQuestion);
            ShowCurQuestion();
        }
        static void UpdateAnswer()
        {
            TextField body = new TextField(1, 6, 50, curAnswer.body);
            body.TextChanging += OnAnswerChange;
            Button acceptButton = new Button(1, 2, "Create");
            Button cancelButton = new Button(1, 3, "Cancel");
            acceptButton.Clicked += TryAcceptAnswerUpdate;
            cancelButton.Clicked += ShowCurAnswer;
            Window win = new Window("Update answer");
            win.Add(body, acceptButton, cancelButton);
            Application.Top.RemoveAll();
            Application.Top.Add(win);
            Application.Run();
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
            answerRepository.UpdateAnswer(curAnswer);
            ShowCurAnswer();
        }
        static void InitiateListWindow<T>(List<T> curList, string windowName)
        {
            currentPage = 1;
            Toplevel top = Application.Top;
            top.RemoveAll();
            MenuBar menu = new MenuBar(new MenuBarItem[] {
           new MenuBarItem ("_File", new MenuItem [] {
               new MenuItem ("_MainPage", "", ToMain)
           }),
       });
            Button nextPage = new Button(10, 2, "->");
            nextPage.Clicked += NextPage;
            Button prevPage = new Button(4, 2, "<-");
            prevPage.Clicked += PrevPage;
            curView = new ListView(new Rect(4, 8, Application.Top.Frame.Width, 10), new List<Answer>());
            curView.SetSource(GetPage<T>(curList));
            Button back = new Button(4, 3, "");
            if(typeof(T) == typeof(Answer))
            {
                curView.OpenSelectedItem += OnAnswerClick;
            }
            else if(typeof(T) == typeof(Question))
            {
                curView.OpenSelectedItem += OnQuestionClick;
            }
            else if(typeof(T) == typeof(User))
            {
                curView.OpenSelectedItem += OnUserClick;
            }

            if (curQuestion != null)
            {
                back.Text = "To question";
                back.Clicked += ShowCurQuestion;
            }
            else if(curUser != null)
            {
                back.Text = "To user";
                back.Clicked += ShowCurUser;
            }
            Window list = new Window(windowName);

            top.Add(list, menu, curView, nextPage, prevPage, back);

            Application.Run();
        }

        static void ToMain()
        {
            curUser = mainUser;
            Application.Top.RemoveAll();
            Application.Top.Add(mainWindow);
            //Application.Run();
        }

        static void SetCurrentPage()
        {
            if(curQuestions != null)
            {
                curView.SetSource(GetPage<Question>(curQuestions));
            }
            else if(curAnswers != null)
            {
                curView.SetSource(GetPage<Answer>(curAnswers));
            }
            else if(curUsers != null)
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
            if(curQuestions != null)
            {
                pageNum = GetPageNum<Question>(curQuestions);
            }
            else if(curAnswers != null)
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
>>>>>>> 7cba30f923206795fb12ece32b2ea17fe5f6c188
                currentPage--;
                SetCurrentPage();
            }
        }
    }
}
