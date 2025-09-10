/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialAccountUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial accounts.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.Services;

using Empiria.Financial.Projects;

using Empiria.Financial.Adapters;
using Empiria.Financial.Data;

namespace Empiria.Financial.UseCases {

  /// <summary>Provides use cases for update and retrieve financial accounts.</summary>
  public class FinancialAccountUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialAccountUseCases() {
      // no-op
    }

    static public FinancialAccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialAccountUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FinancialAccountDto CreateAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var accountType = FinancialAccountType.Parse(fields.FinancialAccountTypeUID);
      var stdAccount = StandardAccount.Parse(fields.StandardAccountUID);
      var orgUnit = OrganizationalUnit.Parse(fields.OrganizationalUnitUID);

      var account = new FinancialAccount(accountType, stdAccount, orgUnit);

      account.Update(fields);

      account.Save();

      return FinancialAccountMapper.Map(account);
    }


    public void DeleteAccount(string accountUID) {
      Assertion.Require(accountUID, nameof(accountUID));

      var account = FinancialAccount.Parse(accountUID);

      account.Delete();

      account.Save();
    }


    public FinancialAccountHolderDto GetAccount(string accountUID) {
      Assertion.Require(accountUID, nameof(accountUID));

      var account = FinancialAccount.Parse(accountUID);

      return FinancialAccountMapper.MapToHolderDto(account);
    }


    public FixedList<NamedEntityDto> SearchAccounts(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<FinancialAccount> acccounts =
          FinancialAccountDataService.SearchAccountByKeywords(keywords)
                                     .FindAll(x =>
                                        x.FinancialAccountType.PlaysRole(FinancialProject.PROJECT_BASE_ACCOUNTS_ROLE)
                                     );

      return acccounts.MapToNamedEntityList();
    }


    public FixedList<FinancialAccountDescriptor> SearchAccounts(FinancialAccountQuery query) {
      Assertion.Require(query, nameof(query));

      FixedList<FinancialAccount> accounts = query.Execute()
                                                  .FindAll(x =>
                                                    x.FinancialAccountType.PlaysRole(FinancialProject.PROJECT_BASE_ACCOUNTS_ROLE)
                                                  );

      return FinancialAccountMapper.MapToDescriptor(accounts);
    }


    public FinancialAccountDto UpdateAccount(string accountUID, FinancialAccountFields fields) {
      Assertion.Require(accountUID, nameof(accountUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var account = FinancialAccount.Parse(accountUID);

      account.Update(fields);

      account.Save();

      return FinancialAccountMapper.Map(account);
    }

    #endregion Use cases

  }  // class FinancialAccountUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
