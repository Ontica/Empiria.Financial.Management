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
using Empiria.Financial.Projects.Adapters;

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

    public FinancialAccountOperationsDto AddAccountOperation(string accountUID, string stdAccountUID) {
      Assertion.Require(accountUID, nameof(accountUID));
      Assertion.Require(stdAccountUID, nameof(stdAccountUID));

      FinancialAccount account = FinancialAccount.Parse(accountUID);
      StandardAccount stdAccount = StandardAccount.Parse(stdAccountUID);

      FinancialAccount operation = account.AddOperation(stdAccount);

      operation.Save();

      return FinancialAccountMapper.MapAccountOperations(account);

    }


    public FinancialAccountOperationsDto RemoveAccountOperation(string accountUID, string operationAccountUID) {
      Assertion.Require(accountUID, nameof(accountUID));
      Assertion.Require(operationAccountUID, nameof(operationAccountUID));


      FinancialAccount account = FinancialAccount.Parse(accountUID);

      FinancialAccount operation = account.RemoveOperation(operationAccountUID);

      operation.Save();

      return FinancialAccountMapper.MapAccountOperations(account);
    }


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


    public void DeleteAccount(string UID) {
      Assertion.Require(UID, nameof(UID));

      var account = FinancialAccount.Parse(UID);

      account.Delete();

      account.Save();
    }


    public FixedList<FinancialAccountDto> SearchAccount(FinancialAccountQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<FinancialAccount> accounts = FinancialAccountDataService.SearchAccounts(filter, sort);

      return FinancialAccountMapper.Map(accounts);
    }


    public FixedList<NamedEntityDto> SearchAccounts(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<FinancialAccount> acccounts =
          FinancialAccountDataService.SearchAccounts(keywords)
                                     .FindAll(x =>
                                        x.FinancialAccountType.PlaysRole(FinancialProject.PROJECT_BASE_ACCOUNTS_ROLE)
                                     );

      return acccounts.MapToNamedEntityList();
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
