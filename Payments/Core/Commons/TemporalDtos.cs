/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : External Services Layer                 *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service connector                       *
*  Type     : ExternalServices                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : QConnect Empiria Payments with external services providers.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments {

  // <summary>Output DTO used to return minimal information about Bills.</summary>
  public class BillDto {

    public string UID {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public DateTime Date {
      get; internal set;
    }


    public string CFDIGUID {
      get; internal set;
    }


    public FixedList<BillItemDto> Items {
      get; internal set;
    }

  } // BillDto


  // <summary>Output DTO used to return payable entity item minimal information.</summary>
  public class BillItemDto {

    public string UID {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Unit {
      get; internal set;
    }


    public decimal Quantity {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }

  } // class BillItemDto

}  // namespace Empiria.Payments
