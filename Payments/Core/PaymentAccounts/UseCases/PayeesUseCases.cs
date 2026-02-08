/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Use cases Layer                         *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Use case interactor class               *
*  Type     : PayeesUseCases                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for payees management.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Payments.Adapters;
using Empiria.Payments.Data;

namespace Empiria.Payments.UseCases {

  /// <summary>Use cases for payees management.</summary>
  public class PayeesUseCases : UseCase {

    #region Constructors and parsers

    protected PayeesUseCases() {

    }

    static public PayeesUseCases UseCaseInteractor() {
      return CreateInstance<PayeesUseCases>();
    }

    #endregion Constructors and parsers

    #region Query use cases

    public Payee AddPayee(PayeeFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var payee = new Payee(fields);

      payee.Save();

      return payee;
    }


    public Payee DeletePayee(string payeeUID) {
      Assertion.Require(payeeUID, nameof(payeeUID));

      var payee = Payee.Parse(payeeUID);

      payee.Delete();

      payee.Save();

      return payee;
    }


    public FixedList<Payee> SearchPayees(PayeesQuery query) {
      Assertion.Require(query, nameof(query));

      string filter = query.MapToFilterString();
      string sortBy = query.MapToSortString();

      return PayeesData.SearchPayees(filter, sortBy);
    }


    public Payee UpdatePayee(Payee payee, PayeeFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      payee.Update(fields);

      payee.Save();

      return payee;
    }

    #endregion Command use cases

  }  // class PayeesUseCases

}  // namespace Empiria.Payments.UseCases
