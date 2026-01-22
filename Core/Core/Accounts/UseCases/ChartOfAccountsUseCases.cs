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


    public FixedList<ChartOfAccountsDefinitionDto> GetChartsOfAccountsList() {
      var chartsOfAccounts = ChartOfAccounts.GetList();

      return ChartOfAccountsMapper.Map(chartsOfAccounts);
    }


    public StandardAccountHolder GetStandardAccount(string chartOfAccountsUID, string stdAccountUID) {
      Assertion.Require(chartOfAccountsUID, nameof(chartOfAccountsUID));
      Assertion.Require(stdAccountUID, nameof(stdAccountUID));

      var chartOfAccounts = ChartOfAccounts.Parse(chartOfAccountsUID);

      StandardAccount stdAccount = chartOfAccounts.GetStandardAccount(stdAccountUID);

      return StandardAccountMapper.Map(stdAccount);
    }


    public FixedList<NamedEntityDto> GetStandardAccounts(string chartOfAccountsUID, string stdAccountTypeUID) {
      Assertion.Require(chartOfAccountsUID, nameof(chartOfAccountsUID));
      Assertion.Require(stdAccountTypeUID, nameof(stdAccountTypeUID));

      var chartOfAccounts = ChartOfAccounts.Parse(chartOfAccountsUID);

      StandardAccountType stdAccountType = StandardAccountType.Parse(stdAccountTypeUID);

      FixedList<StandardAccount> stdAccounts = chartOfAccounts.GetStandardAccounts()
                                                              .FindAll(x => x.StandardAccountType.Equals(stdAccountType) &&
                                                                       x.RoleType == AccountRoleType.Detalle &&
                                                                       x.Status != StateEnums.EntityStatus.Suspended);

      return stdAccounts.MapToNamedEntityList();
    }


    public ChartOfAccountsDto SearchChartOfAccounts(ChartOfAccountsQuery query) {
      Assertion.Require(query, nameof(query));

      var chartOfAccounts = ChartOfAccounts.Parse(query.ChartOfAccountsUID);

      FixedList<StandardAccount> stdAccounts = query.Execute();

      return ChartOfAccountsMapper.Map(chartOfAccounts, stdAccounts);
    }


    #endregion Use cases

  }  // class ChartOfAccountsUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
