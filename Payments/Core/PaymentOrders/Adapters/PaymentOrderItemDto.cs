/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PaymentOrderDto, PaymentOrderDescriptor    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payment orders.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Orders.Adapters {

  /// <summary>Data transfer objects used to return payment order items.</summary>
  public class PaymentOrderItemDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public decimal Subtotal {
      get; internal set;
    }

  } // class PaymentOrderItemDto


}  // namespace Empiria.Payments.Orders.Adapters
