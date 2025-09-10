/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : ExternalAccountsUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Use cases for retrieve accounts from external systems and update them as financial accounts.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.Services;

using Empiria.Financial.Adapters;
using Empiria.Financial.Projects;

namespace Empiria.Financial.UseCases {

  /// <summary>Use cases for retrieve accounts from external systems and update
  /// them as financial accounts.</summary>
  public class ExternalAccountsUseCases : UseCase {

    #region Constructors and parsers

    protected ExternalAccountsUseCases() {
      // no-op
    }

    static public ExternalAccountsUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ExternalAccountsUseCases>();
    }

    #endregion Constructors and parsers

    #region Credit system use cases

    public FinancialAccountDto CreateAccountFromCreditSystem(string accountNo, string projectUID) {
      Assertion.Require(accountNo, nameof(accountNo));
      Assertion.Require(projectUID, nameof(projectUID));

      ICreditAccountData externalAccount = GetAccountFromCreditSystem(accountNo, true);

      var accountType = FinancialAccountType.CreditAccount;
      var project = FinancialProject.Parse(projectUID);
      var stdAccount = project.GetStandardAccounts()[0];
      var orgUnit = (OrganizationalUnit) project.BaseOrgUnit;

      var account = new FinancialAccount(accountType, stdAccount, orgUnit, project);

      account.Update(externalAccount);

      account.Save();

      return FinancialAccountMapper.Map(account);
    }


    private ICreditAccountData GetAccountFromCreditSystem(string accountNo, bool forCreation = false) {

      var service = new ExternalCreditSystemServices();

      ICreditAccountData externalAccount = service.TryGetCreditWithAccountNo(accountNo);

      Assertion.Require(externalAccount, $"Unrecognized external credit system's account: '{accountNo}'");

      if (!forCreation) {
        return externalAccount;
      }

      var current = FinancialAccount.GetList(x => x.FinancialAccountType.Equals(FinancialAccountType.CreditAccount) &&
                                                  x.AccountNo == externalAccount.AccountNo);

      Assertion.Require(current.Count == 0,
                        $"La cuenta de crédito '{externalAccount}' ya está registrada en el sistema.");

      return externalAccount;
    }


    public FinancialAccountDto TryGetAccountFromCreditSystem(string accountNo) {
      Assertion.Require(accountNo, nameof(accountNo));

      var service = new ExternalCreditSystemServices();

      ICreditAccountData account = service.TryGetCreditWithAccountNo(accountNo);

      if (account == null) {
        return null;
      }

      return FinancialAccountMapper.Map(account);
    }

    #endregion Credit system use cases

  }  // class ExternalAccountsUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
