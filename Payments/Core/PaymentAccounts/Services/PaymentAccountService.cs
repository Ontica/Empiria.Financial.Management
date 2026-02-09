/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Services Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Static service                          *
*  Type     : PaymentAccountService                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Static services used to manage payments accounts.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Adapters;
using Empiria.Payments.Data;

namespace Empiria.Payments.UseCases {

  /// <summary>Static services used to manage payments accounts.</summary>
  static public class PaymentAccountService {

    #region Services

    static public Payee AddPayee(PayeeFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payee = new Payee(fields);

      payee.Save();

      return payee;
    }


    static public PaymentAccountDto AddPaymentAccount(Payee payee, PaymentAccountFields fields) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      PaymentAccount account = payee.AddAccount(fields);

      account.Save();

      return new PaymentAccountDto(account);
    }


    static public Payee RemovePayee(Payee payee) {
      Assertion.Require(payee, nameof(payee));

      payee.Delete();

      payee.Save();

      return payee;
    }


    static public PaymentAccountDto RemovePaymentAccount(Payee payee, PaymentAccount account) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(account, nameof(account));

      payee.RemoveAccount(account);

      account.Save();

      return new PaymentAccountDto(account);
    }


    static public FixedList<Payee> SearchPayees(PayeesQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();
      string sortBy = query.MapToSortString();

      return PayeesData.SearchPayees(filter, sortBy);
    }


    static public Payee UpdatePayee(Payee payee, PayeeFields fields) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      payee.Update(fields);

      payee.Save();

      return payee;
    }


    static public PaymentAccountDto UpdatePaymentAccount(Payee payee, PaymentAccountFields fields) {
      Assertion.Require(payee, nameof(payee));
      Assertion.Require(fields, nameof(fields));

      var account = PaymentAccount.Parse(fields.UID);

      payee.UpdateAccount(account, fields);

      account.Save();

      return new PaymentAccountDto(account);
    }

    #endregion Services

  }  // class PaymentAccountService

}  // namespace Empiria.Payments.UseCases
