/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PayableEntityDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payable entities data.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Adapters;

namespace Empiria.Payments.Adapters {

  /// <summary>Data transfer objects used to return payable entities data.</summary>
  public class PayableEntityDto {

    public string UID {
      get; set;
    }

    public NamedEntityDto Type {
      get; set;
    }

    public string EntityNo {
      get; set;
    }

    public string Name {
      get; set;
    }

    public string Description {
      get; set;
    }

    public NamedEntityDto Budget {
      get; set;
    }

    public NamedEntityDto Currency {
      get; set;
    }

    public decimal Total {
      get; set;
    }

    public NamedEntityDto PayTo {
      get; internal set;
    }

    public FixedList<PaymentAccountDto> PaymentAccounts {
      get; internal set;
    }

    public FixedList<PayableEntityItemDto> Items {
      get; internal set;
    }

  } // class PayableEntityDto

}  // namespace Empiria.Payments.Adapters
