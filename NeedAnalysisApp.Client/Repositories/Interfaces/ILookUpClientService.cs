﻿namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface ILookUpClientService
{
    Task<List<LookUpType>> GetAllTypes();
}
