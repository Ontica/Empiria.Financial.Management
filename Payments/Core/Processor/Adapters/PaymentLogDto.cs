/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PaymentLogDto                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payment logs.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Processor.Services {
  
  /// <summary>Data transfer objects used to return payment log.</summary>
  public class PaymentLogDto {

    public string UID {
      get; internal set;
    }


    public string PaymentOrderTypeName {
      get; internal set;
    }


    public string PaymentOrderNo {
      get; internal set;
    }


    public string PayTo {
      get; internal set;
    }

    public string RequestedBy {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime DueTime {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public string Currency {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  } // class PaymentLogDto

}  // namespace Empiria.Payments.Processor.Services
