/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PaymentAccountsUseCases                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payments accounts.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.Financial.Services;

namespace Empiria.Payments.UseCases {

  /// <summary>Use cases for payments accounts.</summary>
  public class PaymentAccountsUseCases : UseCase {

    #region Constructors and parsers

    protected PaymentAccountsUseCases() {

    }

    static public PaymentAccountsUseCases UseCaseInteractor() {
      return CreateInstance<PaymentAccountsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public PaymentAccountDto AddPaymentAccount(Payee payee, PaymentAccountFields fields) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(fields, nameof(fields));

      var account = new PaymentAccount(payee, fields);

      account.Save();

      return new PaymentAccountDto(account);
    }


    public FixedList<PaymentAccountDto> GetPaymentAccounts(string payeeUID) {
      Assertion.Require(payeeUID, nameof(payeeUID));

      var payee = Payee.Parse(payeeUID);

      return PaymentAccountServices.GetPaymentAccounts(payee);
    }


    public PaymentAccountDto RemovePaymentAccount(Payee payee, PaymentAccount account) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(account, nameof(account));

      Assertion.Require(account.Party.Equals(payee), "La cuenta no pertenece al beneficiario especificado.");

      account.Delete();

      account.Save();

      return new PaymentAccountDto(account);
    }


    public PaymentAccountDto UpdatePaymentAccount(Payee payee, PaymentAccountFields fields) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(fields, nameof(fields));

      var account = PaymentAccount.Parse(fields.UID);

      Assertion.Require(account.Party.Equals(payee), "La cuenta no pertenece al beneficiario especificado.");

      account.Update(fields);

      account.Save();

      return new PaymentAccountDto(account);
    }

    #endregion Use cases

  }  // class PaymentAccountsUseCases

}  // namespace Empiria.Payments.UseCases
