/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PayableItemDto                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payable items.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Data transfer objects used to return payable items.</summary>
  public class PayableItemDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Payable {
      get; internal set;
    }

    public decimal InputTotal {
      get; internal set;
    }

    public decimal OutputTotal {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }


    public decimal ExchangeRate {
      get; internal set;
    } = 1;

    public NamedEntityDto Status {
      get; internal set;
    }

  } // class PayableItemDto

}  // namespace Empiria.Payments.Payables.Adapters
