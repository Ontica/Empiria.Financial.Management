/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Mapper                                  *
*  Type     : CashFlowPlanMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps CashFlowPlan instances to data transfer objects.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Maps CashFlowPlan instances to data transfer objects.</summary>
  static public class CashFlowPlanMapper {

    #region Public mappers


    static internal FixedList<CashFlowPlanDto> Map(FixedList<CashFlowPlan> plans) {
      return plans.Select(x => Map(x))
                  .ToFixedList();
    }

    #endregion Public mappers

    #region Helpers

    static private CashFlowPlanDto Map(CashFlowPlan plan) {
      return new CashFlowPlanDto {
        UID = plan.UID,
        Name = plan.Name,
        BaseCurrency = plan.BaseCurrency.MapToNamedEntity(),
        Categories = plan.AvailableCategories.MapToNamedEntityList(),
        ProjectionsColumns = plan.ProjectionsColumns.MapToNamedEntityList(),
        Years = plan.Years,
        EditionAllowed = plan.EditionAllowed,
      };
    }

    #endregion Helpers

  }  // class CashFlowPlanMapper

}  // namespace Empiria.CashFlow.Projections.Adapters
