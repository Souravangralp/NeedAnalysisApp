namespace NeedAnalysisApp.Shared.Common.Utilities;

public static class SectionUtility
{
    public static QuestionDto GetDemoQuestion(int questionTypeId, int sectionTypeId)
    {
        switch (questionTypeId)
        {
            case 1:
                return new QuestionDto()
                {
                    UniqueId = "",
                    Value = "Multiple choice",
                    GeneralLookUp_QuestionTypeId = questionTypeId,
                    GeneralLookUp_SectionTypeId = sectionTypeId,
                    DisplayOrder = 0,
                    Description = string.Empty,
                    Options = {
                        new OptionDto()
                        {
                            DisplayOrder = 1,
                            Value = "option 1",
                            Point = 0.0
                        },
                        new OptionDto()
                        {
                            DisplayOrder = 2,
                            Value = "option 2",
                            Point = 0.0
                        },
                        new OptionDto()
                        {
                            DisplayOrder = 3,
                            Value = "option 3",
                            Point = 0.0
                        },
                        new OptionDto()
                        {
                            DisplayOrder = 4,
                            Value = "option 4",
                            Point = 0.0
                        }
                    }
                };
            case 2:
                return new QuestionDto()
                {
                    UniqueId = "",
                    Value = "True False",
                    GeneralLookUp_QuestionTypeId = questionTypeId,
                    GeneralLookUp_SectionTypeId = sectionTypeId,
                    DisplayOrder = 0,
                    Description = string.Empty,
                    Options = {
                        new OptionDto()
                        {
                            DisplayOrder = 1,
                            Value = "true",
                            Point = 0.0
                        },
                        new OptionDto()
                        {
                            DisplayOrder = 2,
                            Value = "false",
                            Point = 0.0
                        }
                    }
                };
            case 3:
                return new QuestionDto()
                {
                    UniqueId = "",
                    Value = "Text",
                    GeneralLookUp_QuestionTypeId = questionTypeId,
                    GeneralLookUp_SectionTypeId = sectionTypeId,
                    DisplayOrder = 0,
                    Description = string.Empty,
                    Options = { new OptionDto() { DisplayOrder = 1, Value = "", Point = 0 } }
                };
            case 4:
                return new QuestionDto()
                {
                    UniqueId = "",
                    Value = "Percentage",
                    GeneralLookUp_QuestionTypeId = questionTypeId,
                    GeneralLookUp_SectionTypeId = sectionTypeId,
                    DisplayOrder = 0,
                    Description = string.Empty,
                    Options = { new OptionDto() { DisplayOrder = 1, Value = "", Point = 0 } }
                };
            case 5:
                return new QuestionDto()
                {
                    UniqueId = "",
                    Value = "Label",
                    GeneralLookUp_QuestionTypeId = questionTypeId,
                    GeneralLookUp_SectionTypeId = sectionTypeId,
                    DisplayOrder = 0,
                    Description = string.Empty,
                };
            default: return new QuestionDto();
        }
    }

    public static int GetSectionId(string sectionName)
    {
        switch (sectionName)
        {
            case "Who we are":
                return 6;
            case "What we do":
                return 7;
            case "How we do it":
                return 8;
            default: return 0;
        }
    }

    public static int GetNewDisplayOrder(List<QuestionDto> questions)
    {
        var lastDisplayOrder = questions.Count > 0 ? questions.Max(x => x.DisplayOrder) : 1;

        return lastDisplayOrder + 1;
    }
}