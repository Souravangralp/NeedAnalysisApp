namespace NeedAnalysisApp.Shared.Common;

public class Result
{
    public bool Success { get; set; }

    public object? Model { get; set; } = null;

    public List<Error> Errors { get; set; } = [];
}

public class Error
{
    public string Message { get; set; } = string.Empty;
}