/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Output DTO                              *
*  Type     : PayableEntityItemDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return payable entity item information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Adapters {

  /// <summary>Output DTO used to return payable entity item information.</summary>
  public class PayableEntityItemDto {

    public string UID {
      get; internal set;
    }

    public decimal Quantity {
      get; internal set;
    }

    public NamedEntityDto Unit {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public decimal UnitPrice {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto BudgetAccount {
      get; internal set;
    }

  } // class PayableEntityItemDto

}  // namespace Empiria.Payments.Adapters
