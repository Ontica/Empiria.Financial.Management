/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Use case interactor class               *
*  Type     : BudgetExplorerUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for retrieve budget information.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;

using Empiria.Budgeting.Adapters;

namespace Empiria.Budgeting.UseCases {

  /// <summary>Use cases for retrieve budget information.</summary>
  public class BudgetExplorerUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetExplorerUseCases() {
      // no-op
    }

    static public BudgetExplorerUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BudgetExplorerUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public BudgetExplorerResultDto RetrievePlannedBudget(BudgetExplorerQuery query) {
      Assertion.Require(query, nameof(query));

      return new BudgetExplorerResultDto();
    }

    #endregion Use cases

  }  // class BudgetExplorerUseCases

}  // namespace Empiria.Budgeting.UseCases
