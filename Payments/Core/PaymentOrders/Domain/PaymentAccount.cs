/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : Payment Order                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a paymentAccount.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Financial;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Payments.Orders {

  /// Represents a payment account.
  public class PaymentAccount : BaseObject {

    #region Constructors and parsers

    private PaymentAccount() {
      // Required by Empiria Framework.
    }

    static public PaymentAccount Parse(int id) => ParseId<PaymentAccount>(id);

    static internal PaymentAccount Parse(string UID) => ParseKey<PaymentAccount>(UID);

    static public PaymentAccount Empty => ParseEmpty<PaymentAccount>();

    #endregion Constructors and parsers

    #region Properties

    public int Index {
      get; private set;
    }


    public int PaymentAccountNo {
      get; private set;
    }


    public string Institution {
      get; private set;
    }


    public Party Owner {
      get; private set;
    }


    public string Alias {
      get; private set;
    }


    public int PaymentAccountType {
      get; private set;
    }


    public string Description {
      get; private set;
    }


    public Currency Currency {
      get; private set;
    }


    private JsonObject ExtData {
      get; set;
    }


    public Contact PostedBy {
      get; private set;
    }


    public DateTime PostingTime {
      get; private set;
    }


    public EntityStatus Status {
      get; private set;
    }

    #endregion Properties

  } // class PaymentAccount

} // namespace Empiria.Payments.Orders
