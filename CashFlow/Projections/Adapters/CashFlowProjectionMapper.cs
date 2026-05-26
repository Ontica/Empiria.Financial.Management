/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Projections                       Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Projections.dll           Pattern   : Mapper                                  *
*  Type     : CashFlowProjectionMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps CashFlowProjection instances to data transfer objects.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.Financial;
using Empiria.History;

using Empiria.StateEnums;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Maps CashFlowProjection instances to data transfer objects.</summary>
  static public class CashFlowProjectionMapper {

    #region Public mappers

    static internal CashFlowProjectionHolderDto Map(CashFlowProjection projection) {

      var byYearProjection = new CashFlowProjectionByYear(projection);

      return new CashFlowProjectionHolderDto {
        Projection = MapProjection(projection),
        Entries = CashFlowProjectionEntryMapper.MapToDescriptor(projection.Entries),
        GroupedEntries = new CashFlowProjectionEntriesByYearTableDto(byYearProjection),
        Documents = DocumentServices.GetAllEntityDocuments(projection),
        History = HistoryServices.GetEntityHistory(projection),
        Actions = MapActions(projection)
      };
    }


    static public FixedList<CashFlowProjectionDescriptorDto> MapToDescriptor(FixedList<CashFlowProjection> projections) {
      return projections.Select(x => MapToDescriptor(x))
                        .ToFixedList();
    }


    static internal FixedList<CashFlowProjectionAccountDto> Map(FixedList<FinancialAccount> accounts) {
      return accounts.Select(x => Map(x))
                     .ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private CashFlowProjectionActions MapActions(CashFlowProjection projection) {
      return new CashFlowProjectionActions {
        CanAuthorize = projection.Rules.CanAuthorize,
        CanClose = projection.Rules.CanClose,
        CanDelete = projection.Rules.CanDelete,
        CanEditDocuments = projection.Rules.CanEditDocuments,
        CanReject = projection.Rules.CanReject,
        CanSendToAuthorization = projection.Rules.CanSendToAuthorization,
        CanUpdate = projection.Rules.CanUpdate,
        ShowVariables = projection.BaseAccount.FinancialAccountType.UID.Contains("Credit")
      };
    }


    static private CashFlowProjectionDescriptorDto MapToDescriptor(CashFlowProjection projection) {
      return new CashFlowProjectionDescriptorDto {
        UID = projection.UID,
        PlanName = projection.Plan.Name,
        ProjectionTypeCategoryName = projection.Category.Name,
        ProjectionNo = projection.ProjectionNo,
        ProjectionName = projection.BaseAccount.Name,
        PartyName = projection.BaseParty.Name,
        ProjectName = ((INamedEntity) projection.BaseProject).Name,
        ProjectTypeCategoryName = projection.BaseProject.Category.Name,
        AccountName = projection.BaseAccount.Name,
        SourceName = projection.OperationSource.Name,
        AuthorizationTime = projection.AuthorizationTime,
        AuthorizedByName = projection.AuthorizedBy.Name,
        RecordingTime = projection.RecordingTime,
        RecordedByName = projection.RecordedBy.Name,
        Currency = projection.BaseAccount.Currency.ISOCode,
        InflowsTotal = projection.InflowsTotal,
        OutflowsTotal = projection.OutflowsTotal,
        Total = projection.GetTotal(),
        StatusName = projection.Status.GetName(),
      };
    }


    static private CashFlowProjectionDto MapProjection(CashFlowProjection projection) {
      return new CashFlowProjectionDto {
        UID = projection.UID,
        Plan = CashFlowPlanMapper.Map(projection.Plan),
        ProjectionTypeCategory = projection.Category.MapToNamedEntity(),
        ProjectionNo = projection.ProjectionNo,
        Party = projection.BaseParty.MapToNamedEntity(),
        Project = ((INamedEntity) projection.BaseProject).MapToNamedEntity(),
        ProjectTypeCategory = projection.BaseProject.Category.MapToNamedEntity(),
        Account = projection.BaseAccount.MapToNamedEntity(),
        Source = projection.OperationSource.MapToNamedEntity(),
        Description = projection.Description,
        Justification = projection.Justification,
        Tags = projection.Tags.Split(' '),
        AccountAttributes = projection.BaseAccountAttributes,
        FinancialData = projection.FinancialData,
        ProjectGoals = projection.ProjectGoals,
        ApplicationDate = projection.ApplicationDate,
        AppliedBy = projection.AppliedBy.MapToNamedEntity(),
        RecordingTime = projection.RecordingTime,
        RecordedBy = projection.RecordedBy.MapToNamedEntity(),
        AuthorizationTime = projection.AuthorizationTime,
        AuthorizedBy = projection.AuthorizedBy.MapToNamedEntity(),
        RequestedTime = projection.RequestedTime,
        RequestedBy = projection.RequestedBy.MapToNamedEntity(),
        AdjustmentOf = projection.AdjustmentOf.IsEmptyInstance ?
                                  NamedEntityDto.Empty : projection.AdjustmentOf.MapToNamedEntity(),
        Currency = projection.BaseAccount.Currency.ISOCode,
        InflowsTotal = projection.InflowsTotal,
        OutflowsTotal = projection.OutflowsTotal,
        Total = projection.GetTotal(),
        Status = projection.Status.MapToNamedEntity(),
      };
    }


    static private CashFlowProjectionAccountDto Map(FinancialAccount account) {

      return new CashFlowProjectionAccountDto {
        UID = account.UID,
        Name = ((INamedEntity) account).Name,
        Currencies = new[] { account.Currency }.MapToNamedEntityList()
      };
    }

    #endregion Helpers

  }  // class CashFlowProjectionMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
