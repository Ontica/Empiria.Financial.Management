/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Use case interactor class               *
*  Type     : BudgetExplorerUseCases                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for retrieve budget information.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DynamicData;
using Empiria.Parties;
using Empiria.Services;

using Empiria.Budgeting.Transactions;

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

    public object BreakdownBudget(ExplorerBreakdownQuery query) {
      Assertion.Require(query, nameof(query));

      string[] parts = query.Entry.UID.Split('|');

      OrganizationalUnit orgUnit = OrganizationalUnit.Parse(int.Parse(parts[0]));
      BudgetAccount budgetAccount = BudgetAccount.Parse(int.Parse(parts[1]));

      BudgetExplorerResult result;

      switch (query.SubQuery.ReportType) {

        case "budget-transactions":

          FixedList<BudgetTransaction> txns = BudgetExplorerDataService.GetBudgetTransactions(orgUnit, budgetAccount,
                                                                                              query.Entry.Year, query.Entry.Month);

          var dtoTxn = new DynamicDto<object>(
            new DataTableColumn[] {
                new DataTableColumn("transactionNo", "Mes", "text-nowrap"),
                new DataTableColumn("description", "descripción", "text"),
                new DataTableColumn("applicationDate", "Fecha aplicación", "date"),
              }.ToFixedList(),
            txns.Select(x => (object) new {
              x.TransactionNo,
              Description = EmpiriaString.FirstWithValue(x.Justification, x.Description),
              x.ApplicationDate,
              x.UID,
              ClickableEntry = true,
              ItemType = "Entry"
            }).ToFixedList());

          return dtoTxn;

        case "budget-entries":

          FixedList<BudgetEntry> entries = BudgetExplorerDataService.GetBudgetEntries(orgUnit, budgetAccount,
                                                                                      query.Entry.Year, query.Entry.Month);

          var dto = new DynamicDto<object>(
            new DataTableColumn[] {
            new DataTableColumn("transactionNo", "Transacción", "text-nowrap"),
            new DataTableColumn("description", "Descripción", "text"),
            new DataTableColumn("monthName", "Mes", "text"),
            new DataTableColumn("columnName", "Movimiento", "text"),
            new DataTableColumn("controlNo", "Num Verif", "text-nowrap"),
            new DataTableColumn("deposit", "Ampliación", "decimal"),
            new DataTableColumn("withdrawal", "Reducción", "decimal"),
            new DataTableColumn("applicationDate", "Fecha aplicación", "date"),
          }.ToFixedList(),
            entries.Select(x => (object) new {
              x.Transaction.TransactionNo,
              Description = EmpiriaString.FirstWithValue(x.Description, x.ProductName, x.Transaction.Justification),
              x.MonthName,
              ColumnName = x.BalanceColumn.Name,
              x.ControlNo,
              x.Deposit,
              x.Withdrawal,
              x.Transaction.ApplicationDate,
              x.Transaction.UID,
              ClickableEntry = true,
              ItemType = "Entry"
            }).ToFixedList());

          return dto;

        case "monthly-balance":

          BudgetExplorerCommand command = BudgetExplorerQueryMapper.Map(query.Query);

          var explorer = new BudgetBreakdown(command);

          result = explorer.Execute(orgUnit, budgetAccount);

          return BudgetExplorerResultMapper.Map(query.Query, result);

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized report type '{query.SubQuery.ReportType}'.");

      }
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
