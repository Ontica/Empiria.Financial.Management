/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Use case interactor class               *
*  Type     : BudgetExplorerUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for retrieve budget information.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Parties;

using Empiria.Budgeting.Explorer.Adapters;
using Empiria.Budgeting.Explorer.Data;

namespace Empiria.Budgeting.Explorer.UseCases {

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

    public BudgetExplorerResultDto BreakdownBudget(BudgetExplorerQuery query, string uid) {
      Assertion.Require(uid, nameof(uid));

      string[] parts = uid.Split('|');

      OrganizationalUnit orgUnit = OrganizationalUnit.Parse(int.Parse(parts[0]));
      BudgetAccount budgetAccount = BudgetAccount.Parse(int.Parse(parts[1]));

      BudgetExplorerCommand command = BudgetExplorerQueryMapper.Map(query);

      var explorer = new BudgetBreakdown(command);

      BudgetExplorerResult result = explorer.Execute(orgUnit, budgetAccount);

      return BudgetExplorerResultMapper.Map(query, result);
    }


    public FixedList<BudgetDataInColumns> GetAvailableBudget(AvailableBudgetQuery query) {
      Assertion.Require(query, nameof(query));

      var builder = new AvailableBudgetBuilder(query);

      return builder.Build();
    }


    public BudgetExplorerResultDto ExploreBudget(BudgetExplorerQuery query) {
      Assertion.Require(query, nameof(query));

      BudgetExplorerCommand command = BudgetExplorerQueryMapper.Map(query);

      var explorer = new BudgetExplorer(command);

      BudgetExplorerResult result = explorer.Execute();

      return BudgetExplorerResultMapper.Map(query, result);
    }


    public FixedList<BudgetDataInColumns> GetMonthBalances(Budget budget) {
      Assertion.Require(budget, nameof(budget));

      return BudgetExplorerDataService.GetMonthBalances(budget);

    }

    #endregion Use cases

  }  // class BudgetExplorerUseCases

}  // namespace Empiria.Budgeting.Explorer.UseCases
