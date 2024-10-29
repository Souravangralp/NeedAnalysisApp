using NeedAnalysisApp.Data.Models;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Data.Models.Common;

namespace NeedAnalysisApp.Shared.Common.Utilities;

public static class DatabaseSeeder
{
    public static List<GeneralLookUp> GetDemoGeneralLookups()
    {
        return new List<GeneralLookUp>()
        {
            new() { Type = "Question_Type", Value="Multiple choice", Description="to add multiple choice question", IsActive = true, IsDeleted= false},
            new() { Type = "Question_Type", Value="True False", Description="to add true false question", IsActive = true, IsDeleted= false},
            new() { Type = "Question_Type", Value="Text", Description="to add question based on text", IsActive = true, IsDeleted= false},
            new() { Type = "Question_Type", Value="Percentage", Description="to add question based on percentage", IsActive = true, IsDeleted= false},
            new() { Type = "Question_Type", Value="Label", Description="This is representing Label", IsActive = true, IsDeleted= false},
            new() { Type = "Section_Type", Value="Who we are", Description="Defines what we are", IsActive = true, IsDeleted= false},
            new() { Type = "Section_Type", Value="What we do", Description="Defines what we do", IsActive = true, IsDeleted= false},
            new() { Type = "Section_Type", Value="How we do it", Description="Defines how we do it", IsActive = true, IsDeleted= false},
            new() { Type = "GenderType", Value="Male", Description="", IsActive = true, IsDeleted= false},
            new() { Type = "GenderType", Value="Female", Description="", IsActive = true, IsDeleted= false},
            new() { Type = "GenderType", Value="Other", Description="", IsActive = true, IsDeleted= false},
            new() { Type = "AssessmentStatusType", Value="Ready", Description="The assessment is ready to begin", IsActive = true, IsDeleted= false},
            new() { Type = "AssessmentStatusType", Value="Pending", Description="The assessment is running pending.", IsActive = true, IsDeleted= false},
            new() { Type = "AssessmentStatusType", Value="Completed", Description="The status of this assessment is completed", IsActive = true, IsDeleted= false},
        };
    }

    public static List<Assessment> GetDemoAssessments()
    {
        return new List<Assessment>
        {
            // Hospitality Assessments
            new Assessment
            {
                Assessment_IndustryID = 1,
                Name = "Quality Assurance in Hospitality",
                Title = "Ensuring Guest Satisfaction",
                Description = "This assessment evaluates the quality assurance processes in hospitality.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    // Who we are (Section ID: 6)
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "Label: Describe our company's mission.",
                        Description = "Outline the mission statement and objectives of the company.",
                        DisplayOrder = 1,
                        Options = new List<Option>()
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What defines our core values?",
                        Description = "Select the values that align with our company's principles.",
                        DisplayOrder = 2,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 1, Value = "Customer focus", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 1, Value = "Profit maximization", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 1, Value = "Innovation", Point = 1, DisplayOrder = 3, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 1, Value = "Efficiency", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "True or False: Our company values diversity and inclusion.",
                        Description = "Assess whether you believe diversity and inclusion are core values.",
                        DisplayOrder = 3,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 2, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 2, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What makes our company unique?",
                        Description = "Provide a brief description of our unique selling proposition.",
                        DisplayOrder = 4,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 6, Value = "Enter you answer", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What percentage of our workforce is dedicated to customer service?",
                        Description = "Estimate the proportion of staff focused on customer service.",
                        DisplayOrder = 5,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 6, Value = "Enter you answer", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    

                    // What we do (Section ID: 7),
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Label: Explain our core services.",
                        Description = "Provide an overview of our key services.",
                        DisplayOrder = 6,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "What is our main service offering?",
                        Description = "Identify our primary service among the listed options.",
                        DisplayOrder = 7,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 6, Value = "Hotel management", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 6, Value = "Event planning", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "Food delivery", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "Tourism services", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "True or False: We offer 24/7 customer support.",
                        Description = "Determine if we provide around-the-clock support for our clients.",
                        DisplayOrder = 8,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 7, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "What services do we provide to our clients?",
                        Description = "List the main services that we offer.",
                        DisplayOrder = 9,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "What percentage of our clients return for repeat services?",
                        Description = "Estimate the percentage of returning clients.",
                        DisplayOrder = 10,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },

                    // How we do it (Section ID: 8),
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "Label: Describe our service delivery process.",
                        Description = "Outline the process we follow to deliver services.",
                        DisplayOrder = 11,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What is our approach to customer feedback?",
                        Description = "Choose the approach that best describes our customer feedback process.",
                        DisplayOrder = 12,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 11, Value = "Proactive response", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 11, Value = "Reactive response", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 11, Value = "Ignore feedback", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 11, Value = "Gather and analyze", Point = 1, DisplayOrder = 4, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "True or False: Training is a key component of our service delivery.",
                        Description = "Evaluate whether training is crucial for our service.",
                        DisplayOrder = 13,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 12, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 12, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "How do we ensure quality in our services?",
                        Description = "Describe the quality assurance measures we have in place.",
                        DisplayOrder = 14,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What percentage of our staff are trained in customer service?",
                        Description = "Estimate the percentage of staff trained in customer service.",
                        DisplayOrder = 15,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    }
                }
            },
            new Assessment
            {
                Assessment_IndustryID = 1,
                Name = "Customer Service Excellence",
                Title = "Delivering Exceptional Service",
                Description = "An assessment designed to gauge the effectiveness of customer service strategies in the hospitality industry.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    // Who we are (Section ID: 6),
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "Label: Explain our commitment to customer service.",
                        Description = "Outline our service commitment and its importance.",
                        DisplayOrder = 1,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What is the cornerstone of our customer service philosophy?",
                        Description = "Select the value that best represents our approach to customer service.",
                        DisplayOrder = 2,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 1, Value = "Empathy", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 1, Value = "Profitability", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 1, Value = "Efficiency", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 1, Value = "Speed", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "True or False: Our staff is trained to handle difficult customers.",
                        Description = "Assess whether our training includes conflict resolution.",
                        DisplayOrder = 3,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 2, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 2, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "Describe how we can improve customer satisfaction.",
                        Description = "Provide suggestions based on your experiences.",
                        DisplayOrder = 4,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What percentage of customers do you believe are satisfied with our service?",
                        Description = "Estimate customer satisfaction based on your observations.",
                        DisplayOrder = 5,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },

                    // What we do (Section ID: 7)
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Label: Define our service standards.",
                        Description = "Provide a brief overview of our service standards.",
                        DisplayOrder = 6,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Which service aspect is prioritized in our operations?",
                        Description = "Choose the aspect that we emphasize the most.",
                        DisplayOrder = 7,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 6, Value = "Personalization", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 6, Value = "Cost reduction", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "Standardization", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "Technology", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "True or False: Customer feedback is actively sought after.",
                        Description = "Determine if we prioritize gathering customer feedback.",
                        DisplayOrder = 8,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 7, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "What methods do we use to enhance our services?",
                        Description = "Detail the techniques we employ to improve our offerings.",
                        DisplayOrder = 9,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "What percentage of service interactions result in positive feedback?",
                        Description = "Estimate the success rate of our service interactions.",
                        DisplayOrder = 10,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },

                    // How we do it (Section ID: 8)
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "Label: Explain our follow-up procedures.",
                        Description = "Outline how we follow up with customers after service delivery.",
                        DisplayOrder = 11,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What strategy do we use to train staff?",
                        Description = "Select the training approach that best reflects our methods.",
                        DisplayOrder = 12,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 11, Value = "Hands-on training", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 11, Value = "Online courses", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 11, Value = "Workshops", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 11, Value = "Self-study", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "True or False: Our service processes are regularly reviewed for improvement.",
                        Description = "Evaluate whether we consistently analyze our service processes.",
                        DisplayOrder = 13,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 12, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 12, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "How do we measure the success of our customer service?",
                        Description = "Describe the metrics we use to evaluate our service effectiveness.",
                        DisplayOrder = 14,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 1,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What percentage of complaints are resolved on first contact?",
                        Description = "Estimate the effectiveness of our initial complaint resolution.",
                        DisplayOrder = 15,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                }
            },

            // IT Assessments
            new Assessment
            {
                Assessment_IndustryID = 2,
                Name = "Cybersecurity Best Practices",
                Title = "Protecting Digital Assets",
                Description = "This assessment focuses on best practices for cybersecurity in IT, ensuring the protection of sensitive information.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    // Who we are (Section ID: 6),
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "Label: Define our data protection strategy.",
                        Description = "Outline the key components of our data protection approach.",
                        DisplayOrder = 1,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What is the primary goal of our cybersecurity policy?",
                        Description = "Identify the main objective of our cybersecurity framework.",
                        DisplayOrder = 2,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 1, Value = "Protect sensitive data", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 1, Value = "Maximize profits", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 1, Value = "Minimize costs", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 1, Value = "Increase network speed", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "True or False: All employees receive cybersecurity training annually.",
                        Description = "Assess whether our training protocols include all employees.",
                        DisplayOrder = 3,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 2, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 2, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What measures do we take to secure sensitive data?",
                        Description = "Describe the actions implemented to protect sensitive information.",
                        DisplayOrder = 4,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What percentage of data breaches are prevented by our security measures?",
                        Description = "Estimate the effectiveness of our current security protocols.",
                        DisplayOrder = 5,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },

                    // What we do (Section ID: 7),
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Label: Outline our incident response plan.",
                        Description = "Summarize the steps taken in our incident response.",
                        DisplayOrder = 6,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Which tool do we primarily use for threat detection?",
                        Description = "Select the primary tool utilized for identifying security threats.",
                        DisplayOrder = 7,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 6, Value = "Intrusion detection system", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 6, Value = "Firewall", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "Antivirus software", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "VPN", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "True or False: Regular audits of our security systems are conducted.",
                        Description = "Evaluate if security audits are part of our protocol.",
                        DisplayOrder = 8,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 7, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "How do we respond to detected threats?",
                        Description = "Detail our response plan for when threats are identified.",
                        DisplayOrder = 9,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "What percentage of threats do we mitigate successfully?",
                        Description = "Estimate our success rate in mitigating threats.",
                        DisplayOrder = 10,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },

                    // How we do it (Section ID: 8),
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "Label: Describe our security training programs.",
                        Description = "Outline the main components of our cybersecurity training.",
                        DisplayOrder = 11,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What best describes our approach to password security?",
                        Description = "Choose the description that best fits our password policies.",
                        DisplayOrder = 12,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 11, Value = "Mandatory use of complex passwords", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 11, Value = "Password reuse allowed", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 11, Value = "Periodic password changes required", Point = 1, DisplayOrder = 3, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 11, Value = "Simple passwords accepted", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "True or False: Two-factor authentication is implemented for all accounts.",
                        Description = "Assess whether two-factor authentication is part of our security measures.",
                        DisplayOrder = 13,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 12, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 12, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "How do we educate employees on phishing threats?",
                        Description = "Describe our training efforts regarding phishing awareness.",
                        DisplayOrder = 14,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 2,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What percentage of employees report phishing attempts?",
                        Description = "Estimate how many employees actively report phishing attempts.",
                        DisplayOrder = 15,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "Enter you answer here", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true }
                        }
                    }
                }
            },
            new Assessment
            {
                Assessment_IndustryID = 2,
                Name = "Software Development Methodologies",
                Title = "Choosing the Right Approach",
                Description = "Evaluate understanding of various software development methodologies, including Agile and Waterfall.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    // Who we are (Section ID: 6)
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What is the primary characteristic of Agile methodology?",
                        Description = "Identify the key feature that defines Agile methodology.",
                        DisplayOrder = 1,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 1, Value = "Iterative development", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 1, Value = "Sequential phases", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 1, Value = "Heavy documentation", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 1, Value = "Fixed scope", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "True or False: Agile allows for changing requirements throughout the development process.",
                        Description = "Assess the flexibility of the Agile methodology.",
                        DisplayOrder = 2,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 2, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 2, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What challenges might arise when using Agile methodology?",
                        Description = "Explain potential obstacles that teams may encounter with Agile.",
                        DisplayOrder = 3,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "What percentage of Agile projects typically fail?",
                        Description = "Estimate the failure rate of Agile projects based on industry statistics.",
                        DisplayOrder = 4,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 6,
                        Value = "Label: Key roles in Agile methodology.",
                        Description = "Identify the important roles in an Agile team, such as Product Owner and Scrum Master.",
                        DisplayOrder = 5,
                        Options = new List<Option>() // No options needed
                    },

                    // What we do (Section ID: 7)
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Which of the following is a phase in the Waterfall model?",
                        Description = "Choose the correct phase from the Waterfall model.",
                        DisplayOrder = 6,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 6, Value = "Requirements analysis", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 6, Value = "Continuous testing", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "User feedback", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 6, Value = "Sprint planning", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "True or False: Waterfall methodology is best suited for projects with well-defined requirements.",
                        Description = "Evaluate the appropriateness of Waterfall for certain project types.",
                        DisplayOrder = 7,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 7, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 7, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Describe the differences between Agile and Waterfall methodologies.",
                        Description = "Compare and contrast these two popular development methodologies.",
                        DisplayOrder = 8,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "What percentage of projects are considered successful using Agile compared to Waterfall?",
                        Description = "Analyze the success rates of Agile vs. Waterfall methodologies.",
                        DisplayOrder = 9,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 7,
                        Value = "Label: Common Agile frameworks.",
                        Description = "Identify well-known Agile frameworks, such as Scrum and Kanban.",
                        DisplayOrder = 10,
                        Options = new List<Option>() // No options needed
                    },

                    // How we do it (Section ID: 8)
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 1, // Multiple Choice
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What technique is commonly used in Agile to manage workload?",
                        Description = "Identify the workload management technique associated with Agile.",
                        DisplayOrder = 11,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 11, Value = "Kanban", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 11, Value = "Gantt chart", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 11, Value = "Critical path", Point = 0, DisplayOrder = 3, ISInCorrectMatch = false },
                            new Option { Option_QuestionID = 11, Value = "Risk analysis", Point = 0, DisplayOrder = 4, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 2, // True/False
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "True or False: Agile promotes team collaboration and flexibility.",
                        Description = "Evaluate Agile's emphasis on teamwork and adaptability.",
                        DisplayOrder = 12,
                        Options = new List<Option>
                        {
                            new Option { Option_QuestionID = 12, Value = "True", Point = 1, DisplayOrder = 1, ISInCorrectMatch = true },
                            new Option { Option_QuestionID = 12, Value = "False", Point = 0, DisplayOrder = 2, ISInCorrectMatch = false }
                        }
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 3, // Text
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "How do we handle changes to project scope in Agile?",
                        Description = "Explain the process for managing scope changes in Agile projects.",
                        DisplayOrder = 13,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 4, // Percentage
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "What percentage of Agile teams use sprints?",
                        Description = "Provide an estimate of the use of sprints among Agile teams.",
                        DisplayOrder = 14,
                        Options = new List<Option>() // No options needed
                    },
                    new Question
                    {
                        Question_AssessmentId = 3,
                        GeneralLookUp_QuestionTypeId = 5, // Label
                        GeneralLookUp_SectionTypeId = 8,
                        Value = "Label: Key metrics for Agile projects.",
                        Description = "Identify important metrics used to measure Agile project success.",
                        DisplayOrder = 15,
                        Options = new List<Option>() // No options needed
                    }
                }
            },

            // ECommerce Assessments
            new Assessment
            {
                Assessment_IndustryID = 3,
                Name = "ECommerce Strategy Evaluation",
                Title = "Maximizing Online Sales",
                Description = "Assess the effectiveness of various eCommerce strategies aimed at maximizing online sales.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    new Question { /* Fill in question details */ },
                    new Question { /* Fill in question details */ }
                }
            },
            new Assessment
            {
                Assessment_IndustryID = 3,
                Name = "Customer Retention Strategies",
                Title = "Building Loyalty in ECommerce",
                Description = "This assessment measures knowledge of customer retention strategies in the eCommerce sector.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    new Question { /* Fill in question details */ },
                    new Question { /* Fill in question details */ }
                }
            },

            // Banking Assessments
            new Assessment
            {
                Assessment_IndustryID = 4,
                Name = "Risk Management in Banking",
                Title = "Identifying and Mitigating Risks",
                Description = "This assessment focuses on risk management strategies and compliance in the banking industry.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    new Question { /* Fill in question details */ },
                    new Question { /* Fill in question details */ }
                }
            },
            new Assessment
            {
                Assessment_IndustryID = 4,
                Name = "Customer Service in Banking",
                Title = "Providing Exceptional Financial Services",
                Description = "Evaluate customer service practices in the banking sector to ensure client satisfaction.",
                TotalScore = 300,
                IsLive = true,
                Questions = new List<Question>
                {
                    new Question { /* Fill in question details */ },
                    new Question { /* Fill in question details */ }
                }
            }
        };
    }

    public static List<UserCollection> GetDemoUserCollection()
    {
        return new List<UserCollection>()
        {
            new UserCollection() { User = new ApplicationUser() { ApplicationUser_GenderId = 9, ApplicationUser_IndustryId = null, UserName = "Bimlesh@Kumar.com", Email = "Bimlesh@Kumar.com", EmailConfirmed = true, PhoneNumber = "8654595588", FirstName = "Bimlesh", LastName = "kumar", ProfilePictureUrl = "https://randomuser.me/api/portraits/men/82.jpg"}, Role = "Admin", Password ="Asd@123" },
            new UserCollection() { User = new ApplicationUser() { ApplicationUser_GenderId = 10, ApplicationUser_IndustryId = 1, UserName = "Rani@Kumar.com", Email = "Rani@Kumar.com", EmailConfirmed = true, PhoneNumber = "8658565588", FirstName = "Rani", LastName = "Kumari", ProfilePictureUrl = "https://randomuser.me/api/portraits/women/82.jpg"}, Role = "User" , Password ="Asd@123" },
            new UserCollection() { User = new ApplicationUser() { ApplicationUser_GenderId = 9, ApplicationUser_IndustryId = 2, UserName = "Raja@Babu.com", Email = "Raja@Babu.com", EmailConfirmed = true, PhoneNumber = "8658565588", FirstName = "Raja", LastName = "Babu", ProfilePictureUrl = "https://randomuser.me/api/portraits/men/14.jpg"}, Role = "User" , Password ="Asd@123" },
            new UserCollection() { User = new ApplicationUser() { ApplicationUser_GenderId = 10, ApplicationUser_IndustryId = 3, UserName = "Kamini@Devi.com", Email = "Kamini@Devi.com", EmailConfirmed = true, PhoneNumber = "8658565588", FirstName = "Kamini", LastName = "Devi", ProfilePictureUrl = "https://randomuser.me/api/portraits/women/14.jpg"}, Role = "User" , Password ="Asd@123" },
            new UserCollection() { User = new ApplicationUser() { ApplicationUser_GenderId = 9, ApplicationUser_IndustryId = 4, UserName = "Kaksha@Shrivastav.com", Email = "Kaksha@Shrivastav.com", EmailConfirmed = true, PhoneNumber = "8658565588", FirstName = "Kaksha", LastName = "Shrivastav", ProfilePictureUrl = "https://randomuser.me/api/portraits/women/14.jpg"}, Role = "User" , Password ="Asd@123" },
        };
    }
}

public class UserCollection
{
    public required ApplicationUser User { get; set; }
    public required string Role { get; set; }
    public required string Password { get; set; }
}