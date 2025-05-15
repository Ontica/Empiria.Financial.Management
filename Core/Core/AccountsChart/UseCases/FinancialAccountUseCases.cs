/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : AccountUseCases                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial accounts.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Accounts.Adapters;
using Empiria.Financial.Data;
using Empiria.Financial.Projects.Adapters;
using Empiria.Services;

namespace Empiria.Financial.Accounts.UseCases
{

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

      var account = new FinancialAccount(fields);

      account.Save();

      return FinancialAccountMapper.Map(account);
    }



    public FixedList<FinancialAccountDto> SearchAccount(FinancialAccountQuery query) {
      Assertion.Require(query, nameof(query));

      query.EnsureIsValid();

      string filter = query.MapToFilterString();
      string sort = query.MapToSortString();

      FixedList<FinancialAccount> accounts = FinancialAccount.SearchAccount(filter, sort);

      return FinancialAccountMapper.Map(accounts);
    }


    public FixedList<NamedEntityDto> SearchAccounts(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<FinancialAccount> acccounts = FinancialAccountDataService.SearchAccount(keywords);

      return acccounts.MapToNamedEntityList();
    }

    public FixedList<NamedEntityDto> SearchStandardAccounts(string keywords) {
      keywords = keywords ?? string.Empty;

      FixedList<StandardAccount> stdAcccounts = StandardtAccountDataService.SearchStandardtAccounts(keywords);

      return stdAcccounts.MapToNamedEntityList();
    }


    public StandardAccount GetStandardAccount(string uid) {
      Assertion.Require(uid, nameof(uid));

     return StandardAccount.Parse(uid);
    }


    public FixedList<NamedEntityDto> GetStandardAccountCategories() {
      return StandardAccountCategory.GetList().MapToNamedEntityList();
    }


    public StandardAccountCategory GetStandardAccountCategory(string uid) {
      Assertion.Require(uid, nameof(uid));

      return StandardAccountCategory.Parse(uid);
    }

    public void DeleteAccount(string UID) {
      Assertion.Require(UID, nameof(UID));
      var account = FinancialAccount.Parse(UID);

      account.Delete();
      account.Save();
    }


    public FinancialAccountDto UpdateAccount(string UID, FinancialAccountFields fields) {

      Assertion.Require(UID, nameof(UID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var account = FinancialAccount.Parse(UID);

      account.Update(fields);

      account.Save();

      return FinancialAccountMapper.Map(account);
    }


    #endregion Use cases

  }  // class AccountUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
