/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : BudgetTypesUseCases                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for budget types searching and retriving.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Budgeting.Adapters;

namespace Empiria.Budgeting.UseCases {

  /// <summary>Use cases for budget types searching and retriving.</summary>
  public class BudgetTypesUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetTypesUseCases() {
      // no-op
    }

    static public BudgetTypesUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BudgetTypesUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<BudgetTypeDto> BudgetTypesList() {
      FixedList<BudgetType> budgetTypes = BudgetType.GetList();

      FixedList<Budget> budgets = Budget.GetList();

      return BudgetTypeMapper.Map(budgetTypes, budgets);
    }

    #endregion Use cases

  }  // class BudgetTypesUseCases

}  // namespace Empiria.Budgeting.UseCases
