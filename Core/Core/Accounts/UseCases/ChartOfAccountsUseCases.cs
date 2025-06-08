/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases interactor                  *
*  Type     : ChartOfAccountsUseCases                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve chart of accounts.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Adapters;
using Empiria.Financial.Data;

namespace Empiria.Financial.UseCases {

  /// <summary>Provides use cases for update and retrieve chart of accounts.</summary>
  public class ChartOfAccountsUseCases : UseCase {

    #region Constructors and parsers

    protected ChartOfAccountsUseCases() {
      // no-op
    }

    static public ChartOfAccountsUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ChartOfAccountsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ChartOfAccountsDto GetChartOfAccounts(string chartOfAccountsUID) {
      Assertion.Require(chartOfAccountsUID, nameof(chartOfAccountsUID));

      var chartOfAccounts = ChartOfAccounts.Parse(chartOfAccountsUID);

      return ChartOfAccountsMapper.Map(chartOfAccounts);
    }


    public FixedList<NamedEntityDto> GetChartsOfAccountsList() {
      var chartOfAccounts = ChartOfAccounts.GetList();

      return chartOfAccounts.MapToNamedEntityList();
    }


    public StandardAccountHolder GetStandardAccount(string chartOfAccountsUID, string stdAccountUID) {
      Assertion.Require(chartOfAccountsUID, nameof(chartOfAccountsUID));
      Assertion.Require(stdAccountUID, nameof(stdAccountUID));

      var chartOfAccounts = ChartOfAccounts.Parse(chartOfAccountsUID);

      StandardAccount stdAccount = chartOfAccounts.GetStandardAccount(stdAccountUID);

      return StandardAccountMapper.Map(stdAccount);
    }


    public ChartOfAccountsDto SearchChartOfAccounts(ChartOfAccountsQuery query) {
      Assertion.Require(query, nameof(query));

      var chartOfAccounts = ChartOfAccounts.Parse(query.ChartOfAccountsUID);

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<StandardAccount> stdAccounts = StandardAccountDataService.SearchStandardAccounts(filter, sort);

      stdAccounts = query.ApplyRemainingFilters(stdAccounts);

      return ChartOfAccountsMapper.Map(chartOfAccounts, stdAccounts);
    }

    #endregion Use cases

  }  // class ChartOfAccountsUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
