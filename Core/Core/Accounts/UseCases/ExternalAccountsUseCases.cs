/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : ExternalAccountsUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Use cases for retrieve accounts from external systems and update them as financial accounts.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

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

    public FinancialAccountDto CreateAccountFromCreditSystem(string accountNo,
                                                             string projectUID,
                                                             string standardAccountUID) {
      Assertion.Require(accountNo, nameof(accountNo));
      Assertion.Require(projectUID, nameof(projectUID));
      Assertion.Require(standardAccountUID, nameof(standardAccountUID));

      ICreditAccountData externalAccount = GetAccountFromCreditSystem(accountNo, true);

      var accountType = FinancialAccountType.CreditAccount;
      var project = FinancialProject.Parse(projectUID);
      var orgUnit = externalAccount.OrganizationalUnit;

      var stdAccount = StandardAccount.Parse(standardAccountUID);

      Assertion.Require(project.GetStandardAccounts().Contains(stdAccount),
                        $"No se puede asignar esta cuenta de crédito al proyecto " +
                        $"{project.Name}, debido a que todas sus cuentas deben ser del tipo " +
                        $"({project.Subprogram.StandardAccount.StdAcctNo}) " +
                        $"{project.Subprogram.StandardAccount.FullName}.");

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

      var current = FinancialAccount.TryParseWithAccountNo(FinancialAccountType.CreditAccount,
                                                           externalAccount.CreditNo);

      Assertion.Require(current == null,
                        $"La cuenta de crédito '{externalAccount.CreditNo} {externalAccount.CustomerName}' " +
                        $"ya está registrada en el sistema.");

      return externalAccount;
    }


    public FinancialAccountDto RefreshAccountFromCreditSystem(string accountUID) {
      Assertion.Require(accountUID, nameof(accountUID));

      var account = FinancialAccount.Parse(accountUID);

      ICreditAccountData externalAccount = GetAccountFromCreditSystem(account.AccountNo);

      account.Update(externalAccount);

      account.Save();

      return FinancialAccountMapper.Map(account);
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
