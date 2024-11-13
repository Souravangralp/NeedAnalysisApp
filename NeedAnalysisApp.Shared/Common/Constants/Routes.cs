namespace NeedAnalysisApp.Shared.Common.Constants;

public static class Routes
{
    public const string BaseUrl = "https://localhost:7028/api";

    public static class Message 
    {
        public const string GetAll = "api/messages?senderId={0}&receiverId={1}";

        public const string Send = "api/messages";

        public const string MarkRead = "api/messages/{0}/markRead/senderId:{1}/receiverId:{2}";

        public const string MarkReadAll = "api/messages/markReadAll/senderId:{0}/receiverId:{1}";
    }

    public static class Assessment
    {
        public const string Create = "api/assessment";

        public const string GetAll = "api/assessment";

        public const string Get = "api/assessment/{0}";

        public const string Update = "api/assessment";

        public const string Delete = "api/assessment/{0}";

        public static class ScoreCategory 
        {
            public const string Create = "api/assessment/scoreCategories/add";

            public const string GetAll = "api/assessment/scoreCategories";

            public const string Get = "api/assessment/scoreCategories/{0}";
            
            public const string Update = "api/assessment/scoreCategories/update";

            public const string CreateSingle = "api/assessment/scoreCategories/add-new";

        }

        public static class User 
        {
            public const string Get = "api/assessment/user/{0}";

            public const string Assign = "api/assessment/assign/{0}/{1}";
        }
    }

    public static class File 
    {
        public const string Create = "api/files";
    }

    public static class Industry
    {
        public const string Create = "api/industries";

        public const string GetAll = "api/industries";

        public const string Update = "api/industries";

        public const string Delete = "api/industries/{0}";
    }

    public static class User
    {
        public const string GetAll = "api/users";

        public const string GetAllWithRole = "api/users?role={0}";

        public const string GetWithId = "api/users/{0}";
    }
}
