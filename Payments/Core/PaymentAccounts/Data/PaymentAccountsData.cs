/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PaymentAccountsData                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write services for payee's payment accounts.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.Payments.Data {

  /// <summary>Provides data read and write services for payment accounts.</summary>
  static internal class PaymentAccountsData {

    static internal List<PaymentAccount> GetFor(Payee payee) {
      var sql = "SELECT * FROM FMS_Payment_Accounts " +
               $"WHERE PYMT_ACCT_PARTY_ID = {payee.Id} AND " +
               $"PYMT_ACCT_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<PaymentAccount>(op);
    }


    static internal void Write(PaymentAccount o) {

      var op = DataOperation.Parse("write_FMS_Payment_Account",
        o.Id, o.UID, o.AccountType.Id, o.Payee.Id, o.Currency.Id,
        o.PaymentMethod.Id, o.Institution.Id, o.AccountNo, o.ExtData.ToString(),
        o.Keywords, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class PaymentAccountsData

} // namespace Empiria.Payments.Data
