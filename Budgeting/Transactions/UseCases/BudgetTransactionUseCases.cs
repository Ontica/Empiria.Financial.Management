/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Use case interactor class               *
*  Type     : BudgetTransactionUseCases                   License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Use cases used to retrive budget transactions.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

namespace Empiria.Budgeting.Transactions.UseCases {

  /// <summary>Use cases used to record budget transactions.</summary>
  public class BudgetTransactionUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetTransactionUseCases() {
      // no-op
    }

    static public BudgetTransactionUseCases UseCaseInteractor() {
      return CreateInstance<BudgetTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> GetTransactionTypes(string budgetTypeUID) {
      Assertion.Require(budgetTypeUID, nameof(budgetTypeUID));

      var budgetType = BudgetType.Parse(budgetTypeUID);

      return BudgetTransactionType.GetList(budgetType)
                                  .MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class BudgetTransactionUseCases

}  // namespace Empiria.Budgeting.Transactions.UseCases
