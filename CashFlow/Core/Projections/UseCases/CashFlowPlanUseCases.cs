/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : CashFlowPlanUseCases                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrieve cash flow plans.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.CashFlow.Projections.Adapters;

namespace Empiria.CashFlow.Projections.UseCases {

  /// <summary>Use cases used to retrieve cash flow plans.</summary>
  public class CashFlowPlanUseCases : UseCase {

    #region Constructors and parsers

    protected CashFlowPlanUseCases() {
      // no-op
    }

    static public CashFlowPlanUseCases UseCaseInteractor() {
      return CreateInstance<CashFlowPlanUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<CashFlowPlanDto> GetPlans() {
      FixedList<CashFlowPlan> plans = CashFlowPlan.GetList();

      return CashFlowPlanMapper.Map(plans);
    }

    #endregion Use cases

  }  // class CashFlowPlanUseCases

}  // namespace Empiria.CashFlow.Projections.UseCases
