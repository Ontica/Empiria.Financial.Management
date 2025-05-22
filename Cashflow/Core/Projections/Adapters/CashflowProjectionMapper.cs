/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Mapping class                           *
*  Type     : CashflowProjectionMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps CashflowProjection instances to data transfer objects.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.StateEnums;

namespace Empiria.Cashflow.Projections.Adapters {

  /// <summary>Maps CashflowProjection instances to data transfer objects.</summary>
  static public class CashflowProjectionMapper {

    #region Public mappers

    static internal CashflowProjectionHolderDto Map(CashflowProjection projection) {
      // var byYearProjection = new CashflowProjectionByYear(projection);

      return new CashflowProjectionHolderDto {
        Projection = MapProjection(projection),
        //Entries = CashflowProjectionEntryMapper.MapToDescriptor(projection.Entries),
        //GroupedEntries = new CashflowProjectionEntriesByYearTableDto(byYearTransaction.GetEntries()),
        Documents = DocumentServices.GetAllEntityDocuments(projection),
        History = HistoryServices.GetEntityHistory(projection),
        Actions = MapActions(projection.Rules)
      };
    }


    static public FixedList<CashflowProjectionDescriptorDto> MapToDescriptor(FixedList<CashflowProjection> projections) {
      return projections.Select(x => MapToDescriptor(x))
                        .ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private CashflowProjectionActions MapActions(CashflowProjectionRules rules) {
      return new CashflowProjectionActions {
        CanAuthorize = rules.CanAuthorize,
        CanClose = rules.CanClose,
        CanDelete = rules.CanDelete,
        CanEditDocuments = rules.CanEditDocuments,
        CanReject = rules.CanReject,
        CanSendToAuthorization = rules.CanSendToAuthorization,
        CanUpdate = rules.CanUpdate
      };
    }


    static private CashflowProjectionDescriptorDto MapToDescriptor(CashflowProjection projection) {
      return new CashflowProjectionDescriptorDto {
        UID = projection.UID,
        PlanName = projection.Plan.Name,
        CategoryName = projection.Category.Name,
        ClassificationName = projection.Classification.Name,
        ProjectionNo = projection.ProjectionNo,
        BasePartyName = projection.BaseParty.Name,
        OperationSourceName = projection.OperationSource.Name,
        AuthorizationTime = projection.AuthorizationTime,
        AuthorizedByName = projection.AuthorizedBy.Name,
        RecordingTime = projection.RecordingTime,
        RecordedByName = projection.RecordedBy.Name,
        // Total = projection.GetTotal(),
        StatusName = projection.Status.GetName(),
      };
    }


    static private CashflowProjectionDto MapProjection(CashflowProjection projection) {
      return new CashflowProjectionDto {
        UID = projection.UID,
        Plan = projection.Plan.MapToNamedEntity(),
        Category = projection.Category.MapToNamedEntity(),
        Classification = projection.Classification.MapToNamedEntity(),
        ProjectionNo = projection.ProjectionNo,
        BaseParty = projection.BaseParty.MapToNamedEntity(),
        BaseProject = projection.BaseProject.MapToNamedEntity(),
        BaseAccount = projection.BaseAccount.MapToNamedEntity(),
        OperationSource = projection.OperationSource.MapToNamedEntity(),
        Description = projection.Description,
        Justification = projection.Justification,
        Tags = projection.Tags.Split(' '),
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
        // Total = projection.GetTotal(),
        Status = projection.Status.MapToNamedEntity(),
      };
    }

    #endregion Helpers

  }  // class CashflowProjectionMapper

}  // namespace Empiria.Cashflow.Projections.Adapters
