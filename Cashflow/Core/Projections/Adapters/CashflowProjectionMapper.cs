/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Mapping class                           *
*  Type     : CashflowProjectionMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps CashflowProjection instances to data transfer objects.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Cashflow.Projections.Adapters {

  /// <summary>Maps CashflowProjection instances to data transfer objects.</summary>
  static public class CashflowProjectionMapper {

    #region Public mappers

    static public FixedList<CashflowProjectionDescriptorDto> MapToDescriptor(FixedList<CashflowProjection> projections) {
      return projections.Select(x => MapToDescriptor(x))
                        .ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private CashflowProjectionDescriptorDto MapToDescriptor(CashflowProjection projection) {
      return new CashflowProjectionDescriptorDto {
        UID = projection.UID,
        PlanName = projection.Plan.Name,
        CategoryName = projection.Category.Name,
        //ClassificationName = projection.Classification.Name,
        ProjectionNo = projection.ProjectionNo,
        BasePartyName = projection.BaseParty.Name,
        SourceName = projection.Source.Name,
        RecordingTime = projection.RecordingTime,
        RecordedByName = projection.RecordedBy.Name,
        AuthorizationTime = projection.AuthorizationTime,
        AuthorizedByName = projection.AuthorizedBy.Name,
        // Total = projection.Total,
        StatusName = projection.Status.GetName(),
      };
    }

    #endregion Helpers

  }  // class CashflowProjectionMapper

}  // namespace Empiria.Cashflow.Projections.Adapters
