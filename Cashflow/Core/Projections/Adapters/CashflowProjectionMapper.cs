﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Mapping class                           *
*  Type     : CashFlowProjectionMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps CashFlowProjection instances to data transfer objects.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.StateEnums;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Maps CashFlowProjection instances to data transfer objects.</summary>
  static public class CashFlowProjectionMapper {

    #region Public mappers

    static internal CashFlowProjectionHolderDto Map(CashFlowProjection projection) {
      // var byYearProjection = new CashFlowProjectionByYear(projection);

      return new CashFlowProjectionHolderDto {
        Projection = MapProjection(projection),
        //Entries = CashFlowProjectionEntryMapper.MapToDescriptor(projection.Entries),
        //GroupedEntries = new CashFlowProjectionEntriesByYearTableDto(byYearTransaction.GetEntries()),
        Documents = DocumentServices.GetAllEntityDocuments(projection),
        History = HistoryServices.GetEntityHistory(projection),
        Actions = MapActions(projection.Rules)
      };
    }


    static public FixedList<CashFlowProjectionDescriptorDto> MapToDescriptor(FixedList<CashFlowProjection> projections) {
      return projections.Select(x => MapToDescriptor(x))
                        .ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private CashFlowProjectionActions MapActions(CashFlowProjectionRules rules) {
      return new CashFlowProjectionActions {
        CanAuthorize = rules.CanAuthorize,
        CanClose = rules.CanClose,
        CanDelete = rules.CanDelete,
        CanEditDocuments = rules.CanEditDocuments,
        CanReject = rules.CanReject,
        CanSendToAuthorization = rules.CanSendToAuthorization,
        CanUpdate = rules.CanUpdate
      };
    }


    static private CashFlowProjectionDescriptorDto MapToDescriptor(CashFlowProjection projection) {
      return new CashFlowProjectionDescriptorDto {
        UID = projection.UID,
        PlanName = projection.Plan.Name,
        CategoryName = projection.Category.Name,
        ProjectionNo = projection.ProjectionNo,
        BasePartyName = projection.BaseParty.Name,
        BaseProjectName = projection.BaseProject.Name,
        BaseProjectCategoryName = projection.BaseProject.Category.Name,
        BaseAccountName = projection.BaseAccount.Name,
        OperationSourceName = projection.OperationSource.Name,
        AuthorizationTime = projection.AuthorizationTime,
        AuthorizedByName = projection.AuthorizedBy.Name,
        RecordingTime = projection.RecordingTime,
        RecordedByName = projection.RecordedBy.Name,
        // Total = projection.GetTotal(),
        StatusName = projection.Status.GetName(),
      };
    }


    static private CashFlowProjectionDto MapProjection(CashFlowProjection projection) {
      return new CashFlowProjectionDto {
        UID = projection.UID,
        Plan = projection.Plan.MapToNamedEntity(),
        Category = projection.Category.MapToNamedEntity(),
        ProjectionNo = projection.ProjectionNo,
        BaseParty = projection.BaseParty.MapToNamedEntity(),
        BaseProject = projection.BaseProject.MapToNamedEntity(),
        BaseProjectCategory = projection.BaseProject.Category.MapToNamedEntity(),
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

  }  // class CashFlowProjectionMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
