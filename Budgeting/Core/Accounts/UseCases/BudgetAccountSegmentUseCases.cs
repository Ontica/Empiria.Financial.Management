/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : BudgetAccountSegmentUseCases               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for budget account segments.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Budgeting.Adapters;
using Empiria.Financial;

namespace Empiria.Budgeting.UseCases {

  /// <summary>Use cases for budget account segments.</summary>
  public class BudgetAccountSegmentUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetAccountSegmentUseCases() {
      // no-op
    }

    static public BudgetAccountSegmentUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BudgetAccountSegmentUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<BudgetAccountSegmentDto> GetBudgetStandardAccounts(string stdAccountCategoryUID,
                                                                        string keywords) {
      Assertion.Require(stdAccountCategoryUID, nameof(stdAccountCategoryUID));
      keywords = keywords ?? string.Empty;

      var category = StandardAccountCategory.Parse(stdAccountCategoryUID);

      FixedList<StandardAccount> stdAccounts = category.GetStandardAccounts(keywords);

      return BudgetAccountSegmentMapper.Map(stdAccounts);
    }

    #endregion Use cases

  }  // class BudgetAccountSegmentUseCases

}  // namespace Empiria.Budgeting.UseCases
