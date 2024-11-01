/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PayablePaymentDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payable payment objects.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Data transfer objects used to return payable payment account. </summary>
  public class PaymentAccountDto {

    public NamedEntityDto PaymentMethod {
      get; internal set;
    }


    public NamedEntityDto Institution {
      get; internal set;
    }


    public NamedEntityDto Currency {
      get; internal set;
    }

    public string AccountNo {
      get; internal set;
    }


    public string CLABE {
      get; internal set;
    }

  } // class PayablePaymentAccountDto

}  // namespace Empiria.Payments.Payables.Adapters
