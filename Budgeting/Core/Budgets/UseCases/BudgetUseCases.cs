/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : BudgetUseCases                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for budgets searching and updating.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Budgeting.Adapters;

namespace Empiria.Budgeting.UseCases {

  /// <summary>Use cases for budgets searching and updating.</summary>
  public class BudgetUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetUseCases() {
      // no-op
    }

    static public BudgetUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BudgetUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<BudgetDto> BudgetsList() {
      FixedList<Budget> budgets = Budget.GetList();

      return BudgetMapper.Map(budgets);
    }

    #endregion Use cases

  }  // class BudgetUseCases

}  // namespace Empiria.Budgeting.UseCases
