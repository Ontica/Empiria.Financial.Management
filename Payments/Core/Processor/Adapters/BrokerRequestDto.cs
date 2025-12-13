/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration Data Transfer Object        *
*  Type     : BrokerRequestDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Integration DTO used to send requests to payment broker providers.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Integration DTO used to send requests to payment broker providers.</summary>
  public class BrokerRequestDto {

    public PaymentOrder PaymentOrder {
      get; internal set;
    }

    public string RequestUniqueNo {
      get; internal set;
    }

    public string ReferenceNo {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

  }  // class BrokerRequestDto

} // namespace Empiria.Payments.Processor.Adapters
