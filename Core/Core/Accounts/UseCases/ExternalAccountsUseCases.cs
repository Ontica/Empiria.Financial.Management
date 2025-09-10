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
